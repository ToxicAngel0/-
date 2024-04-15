#pragma warning disable 0168 // variable declared but not used.
#if ENABLE_ANIMATION_COLLECTION && ENABLE_ANIMATION_BURST
#define ENABLE_SPRITESKIN_COMPOSITE
#endif

using System;
using System.Collections.Generic;
using UnityEngine.Scripting;
using UnityEngine.U2D.Common;
using Unity.Collections;
using UnityEngine.Rendering;
using UnityEngine.Scripting.APIUpdating;

namespace UnityEngine.U2D.Animation
{
    /// <summary>
    /// Represents vertex position.
    /// </summary>
    public struct PositionVertex
    {
        /// <summary>
        /// Vertex position.
        /// </summary>
        public Vector3 position;
    }

    /// <summary>
    /// Represents vertex position and tangent.
    /// </summary>
    public struct PositionTangentVertex
    {
        /// <summary>
        /// Vertex position.
        /// </summary>
        public Vector3 position;
        
        /// <summary>
        /// Vertex tangent.
        /// </summary>
        public Vector4 tangent;
    }

    /// <summary>
    /// Deforms the Sprite that is currently assigned to the SpriteRenderer in the same GameObject
    /// </summary>
    [Preserve]
    [ExecuteInEditMode]
    [DefaultExecutionOrder(-1)]
    [DisallowMultipleComponent]
    [RequireComponent(typeof(SpriteRenderer))]
    [AddComponentMenu("2D Animation/Sprite Skin")]
    [MovedFrom("UnityEngine.U2D.Experimental.Animation")]
    [HelpURL("https://docs.unity3d.com/Packages/com.unity.2d.animation@7.0/manual/SpriteSkin.html")]
    public sealed partial class SpriteSkin : MonoBehaviour, ISerializationCallbackReceiver
    {
        [SerializeField]
        private Transform m_RootBone;
        [SerializeField]
        private Transform[] m_BoneTransforms = new Transform[0];
        [SerializeField]
        private Bounds m_Bounds;
        [SerializeField]
        private bool m_UseBatching = true;
        [SerializeField] 
        private bool m_AlwaysUpdate = true;
        [SerializeField] 
        private bool m_AutoRebind = false;

        // The deformed m_SpriteVertices stores all 'HOT' channels only in single-stream and essentially depends on Sprite Asset data.
        // The order of storage if present is POSITION, NORMALS, TANGENTS.
        private NativeByteArray m_DeformedVertices;
        private int m_CurrentDeformVerticesLength = 0;
        private SpriteRenderer m_SpriteRenderer;
        private int m_CurrentDeformSprite = 0;
        private bool m_ForceSkinning;
        private bool m_BatchSkinning = false;
        bool m_IsValid = false;
        int m_TransformsHash = 0;

        internal bool batchSkinning
        {
            get { return m_BatchSkinning; }
            set { m_BatchSkinning = value; }
        }

        internal bool autoRebind
        {
            get => m_AutoRebind;
            set
            {
                m_AutoRebind = value;
                CacheCurrentSprite(m_AutoRebind);
            }
            
        }

#if UNITY_EDITOR
        internal static Events.UnityEvent onDrawGizmos = new Events.UnityEvent();
        private void OnDrawGizmos() { onDrawGizmos.Invoke(); }

        private bool m_IgnoreNextSpriteChange = true;
        internal bool ignoreNextSpriteChange
        {
            get { return m_IgnoreNextSpriteChange; }
            set { m_IgnoreNextSpriteChange = value; }
        }
#endif

        private int GetSpriteInstanceID()
        {
            return sprite != null ? sprite.GetInstanceID() : 0;
        }

        internal void Awake()
        {
            m_SpriteRenderer = GetComponent<SpriteRenderer>();
        }

        void OnEnable()
        {
            Awake();
            m_TransformsHash = 0;
            CacheCurrentSprite(false);
            OnEnableBatch();
        }

        internal void OnEditorEnable()
        {
            Awake();
        }
        
        void CacheValidFlag()
        {
            m_IsValid = isValid;
            if(!m_IsValid)
                DeactivateSkinning();
        }

        void Reset()
        {
            Awake();
            if (isActiveAndEnabled)
            {
                CacheValidFlag();
                OnResetBatch();
            }
        }

        internal void UseBatching(bool value)
        {
            if (m_UseBatching != value)
            {
                m_UseBatching = value;
                UseBatchingBatch();
            }
        }

        internal NativeByteArray GetDeformedVertices(int spriteVertexCount)
        {
            if (sprite != null)
            {
                if (m_CurrentDeformVerticesLength != spriteVertexCount)
                {
                    m_TransformsHash = 0;
                    m_CurrentDeformVerticesLength = spriteVertexCount;
                }
            }
            else
            {
                m_CurrentDeformVerticesLength = 0;
            }
            
            m_DeformedVertices = BufferManager.instance.GetBuffer(GetInstanceID(), m_CurrentDeformVerticesLength);
            return m_DeformedVertices;
        }

        /// <summary>
        /// Returns whether this SpriteSkin has currently deformed vertices.
        /// </summary>
        /// <returns>Returns true if this SpriteSkin has currently deformed vertices. Returns false otherwise.</returns>
        public bool HasCurrentDeformedVertices()
        {
            if (!m_IsValid)
                return false;

#if ENABLE_SPRITESKIN_COMPOSITE
            return m_DataIndex >= 0 && SpriteSkinComposite.instance.HasDeformableBufferForSprite(m_DataIndex);
#else
            return m_CurrentDeformVerticesLength > 0 && m_DeformedVertices.IsCreated;
#endif
        }

        /// <summary>
        /// Gets a byte array to the currently deformed vertices for this SpriteSkin.
        /// </summary>
        /// <returns>Returns a reference to the currently deformed vertices. This is valid only for this calling frame.</returns>
        /// <exception cref="InvalidOperationException">Thrown when there are no currently deformed vertices</exception>
        internal NativeArray<byte> GetCurrentDeformedVertices()
        {
            if (!m_IsValid)
                throw new InvalidOperationException("The SpriteSkin deformation is not valid.");

#if ENABLE_SPRITESKIN_COMPOSITE
            if (m_DataIndex < 0)
            {
                throw new InvalidOperationException("There are no currently deformed vertices.");
            }
            return SpriteSkinComposite.instance.GetDeformableBufferForSprite(m_DataIndex);
#else
            if (m_CurrentDeformVerticesLength <= 0)
                throw new InvalidOperationException("There are no currently deformed vertices.");
            if (!m_DeformedVertices.IsCreated)
                throw new InvalidOperationException("There are no currently deformed vertices.");
            return m_DeformedVertices.array;
#endif
        }

        /// <summary>
        /// Gets an array of currently deformed position vertices for this SpriteSkin.
        /// </summary>
        /// <returns>Returns a reference to the currently deformed vertices. This is valid only for this calling frame.</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when there are no currently deformed vertices or if the deformed vertices does not contain only
        /// position data.
        /// </exception>
        internal NativeSlice<PositionVertex> GetCurrentDeformedVertexPositions()
        {
            if (sprite.HasVertexAttribute(VertexAttribute.Tangent))
                throw new InvalidOperationException("This SpriteSkin has deformed tangents");
            if (!sprite.HasVertexAttribute(VertexAttribute.Position))
                throw new InvalidOperationException("This SpriteSkin does not have deformed positions.");

            var deformedBuffer = GetCurrentDeformedVertices();
            return deformedBuffer.Slice().SliceConvert<PositionVertex>();
        }

        /// <summary>
        /// Gets an array of currently deformed position and tangent vertices for this SpriteSkin.
        /// </summary>
        /// <returns>
        /// Returns a reference to the currently deformed position and tangent vertices. This is valid only for this calling frame.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when there are no currently deformed vertices or if the deformed vertices does not contain only
        /// position and tangent data.
        /// </exception>
        internal NativeSlice<PositionTangentVertex> GetCurrentDeformedVertexPositionsAndTangents()
        {
            if (!sprite.HasVertexAttribute(VertexAttribute.Tangent))
                throw new InvalidOperationException("This SpriteSkin does not have deformed tangents");
            if (!sprite.HasVertexAttribute(VertexAttribute.Position))
                throw new InvalidOperationException("This SpriteSkin does not have deformed positions.");

            var deformedBuffer = GetCurrentDeformedVertices();
            return deformedBuffer.Slice().SliceConvert<PositionTangentVertex>();
        }

        /// <summary>
        /// Gets an enumerable to iterate through all deformed vertex positions of this SpriteSkin.
        /// </summary>
        /// <returns>Returns an IEnumerable to deformed vertex positions.</returns>
        /// <exception cref="InvalidOperationException">Thrown when there is no vertex positions or deformed vertices.</exception>
        public IEnumerable<Vector3> GetDeformedVertexPositionData()
        {
            bool hasPosition = sprite.HasVertexAttribute(Rendering.VertexAttribute.Position);
            if (!hasPosition)
                throw new InvalidOperationException("Sprite does not have vertex position data.");

            var rawBuffer = GetCurrentDeformedVertices();
            var rawSlice = rawBuffer.Slice(sprite.GetVertexStreamOffset(VertexAttribute.Position));
            return new NativeCustomSliceEnumerator<Vector3>(rawSlice, sprite.GetVertexCount(), sprite.GetVertexStreamSize());
        }

        /// <summary>
        /// Gets an enumerable to iterate through all deformed vertex tangents of this SpriteSkin. 
        /// </summary>
        /// <returns>Returns an IEnumerable to deformed vertex tangents.</returns>
        /// <exception cref="InvalidOperationException">Thrown when there is no vertex tangents or deformed vertices.</exception>
        public IEnumerable<Vector4> GetDeformedVertexTangentData()
        {
            bool hasTangent = sprite.HasVertexAttribute(Rendering.VertexAttribute.Tangent);
            if (!hasTangent)
                throw new InvalidOperationException("Sprite does not have vertex tangent data.");

            var rawBuffer = GetCurrentDeformedVertices();
            var rawSlice = rawBuffer.Slice(sprite.GetVertexStreamOffset(VertexAttribute.Tangent));
            return new NativeCustomSliceEnumerator<Vector4>(rawSlice, sprite.GetVertexCount(), sprite.GetVertexStreamSize());
        }

        void OnDisable()
        {
            DeactivateSkinning();
            BufferManager.instance.ReturnBuffer(GetInstanceID());
            OnDisableBatch();
        }

#if ENABLE_SPRITESKIN_COMPOSITE
        internal void OnLateUpdate()
#else
        void LateUpdate()
#endif
        {
            CacheCurrentSprite(m_AutoRebind);
            if (isValid && !batchSkinning && this.enabled && (this.alwaysUpdate || this.spriteRenderer.isVisible))
            {
                var transformHash = SpriteSkinUtility.CalculateTransformHash(this);
                var spriteVertexCount = sprite.GetVertexStreamSize() * sprite.GetVertexCount();
                if (spriteVertexCount > 0 && m_TransformsHash != transformHash)
                {
                    var inputVertices = GetDeformedVertices(spriteVertexCount);
                    SpriteSkinUtility.Deform(sprite, gameObject.transform.worldToLocalMatrix, boneTransforms, inputVertices.array);
                    SpriteSkinUtility.UpdateBounds(this, inputVertices.array);
                    InternalEngineBridge.SetDeformableBuffer(spriteRenderer, inputVertices.array);
                    m_TransformsHash = transformHash;
                    m_CurrentDeformSprite = GetSpriteInstanceID();
                }
            }
            else if(!InternalEngineBridge.IsUsingDeformableBuffer(spriteRenderer, IntPtr.Zero))
            {
                DeactivateSkinning();
            }
        }

        void CacheCurrentSprite(bool rebind)
        {
            if (m_CurrentDeformSprite != GetSpriteInstanceID())
            {
                DeactivateSkinning();
                m_CurrentDeformSprite = GetSpriteInstanceID();
                if (rebind && m_CurrentDeformSprite > 0 && rootBone != null)
                {
                    var spriteBones = sprite.GetBones();
                    var transforms = new Transform[spriteBones.Length];
                    if (GetSpriteBonesTransforms(spriteBones, rootBone, transforms))
                        boneTransforms = transforms;
                }
                UpdateSpriteDeform();
                CacheValidFlag();
                m_TransformsHash = 0;
            }
        }

        internal Sprite sprite => spriteRenderer.sprite;

        internal SpriteRenderer spriteRenderer => m_SpriteRenderer;

        /// <summary>
        /// Returns the Transform Components that is used for deformation.
        /// Do not modify elements of the returned array.
        /// </summary>
        /// <returns>An array of Transform Components.</returns>
        public Transform[] boneTransforms
        {
            get { return m_BoneTransforms; }
            internal set
            {
                m_BoneTransforms = value;
                CacheValidFlag();
                OnBoneTransformChanged();
            }
        }

        /// <summary>
        /// Returns the Transform Component that represents the root bone for deformation
        /// </summary>
        /// <returns>A Transform Component</returns>
        public Transform rootBone
        {
            get { return m_RootBone; }
            internal set
            {
                m_RootBone = value;
                CacheValidFlag();
                OnRootBoneTransformChanged();
            }
        }

        internal Bounds bounds
        {
            get { return m_Bounds; }
            set { m_Bounds = value; }
        }

        /// <summary>
        /// Determines if the SpriteSkin executes even if the associated
        /// SpriteRenderer has been culled from view.
        /// </summary>
        public bool alwaysUpdate
        {
            get => m_AlwaysUpdate;
            set => m_AlwaysUpdate = value;
        }
        
        internal static bool GetSpriteBonesTransforms(SpriteBone[] spriteBones, Transform rootBone, Transform[] outTransform)
        {
            if(rootBone == null)
                throw new ArgumentException("rootBone parameter cannot be null");
            if(spriteBones == null)
                throw new ArgumentException("spritebone parameter cannot be null");
            if(outTransform == null)
                throw new ArgumentException("outTransform parameter cannot be null");
            if(spriteBones.Length != outTransform.Length)
                throw new ArgumentException("spritebone and outTransform array length must be the same");
            
            var boneObjects = rootBone.GetComponentsInChildren<Bone>();
            if (boneObjects != null && boneObjects.Length >= spriteBones.Length)
            {
                int i = 0;
                for (; i < spriteBones.Length; ++i)
                {
                    var boneHash = spriteBones[i].guid;
                    var boneTransform = Array.Find(boneObjects, x => (x.guid == boneHash));
                    if (boneTransform == null)
                        break;

                    outTransform[i] = boneTransform.transform;
                }
                if(i >= spriteBones.Length)
                    return true;
            }
                
            // If unable to successfuly map via guid, fall back to path
            return GetSpriteBonesTranformFromPath(spriteBones, rootBone, outTransform);
        }
        
        
        static bool GetSpriteBonesTranformFromPath(SpriteBone[] spriteBones, Transform rootBone, Transform[] outNewBoneTransform)
        {
            var bonePath = new string[spriteBones.Length];
            for (int i = 0; i < spriteBones.Length; ++i)
            {
                if (bonePath[i] == null)
                    CalculateBoneTransformsPath(i, spriteBones, bonePath);
                if (rootBone.name == spriteBones[i].name)
                     outNewBoneTransform[i] = rootBone;
                else
                {
                    var bone = rootBone.Find(bonePath[i]);
                    if (bone == null)
                        return false;
                    outNewBoneTransform[i] = bone;    
                }
            }

            return true;
        }
        
        private static void CalculateBoneTransformsPath(int index, SpriteBone[] spriteBones, string[] paths)
        {
            var spriteBone = spriteBones[index];
            var parentId = spriteBone.parentId;
            var bonePath = spriteBone.name;
            if (parentId != -1 && spriteBones[parentId].parentId != -1)
            {
                if (paths[parentId] == null)
                    CalculateBoneTransformsPath(spriteBone.parentId, spriteBones, paths);
                paths[index] = string.Format("{0}/{1}", paths[parentId], bonePath);
            }
            else
                paths[index] = bonePath;
        }
        
        internal bool isValid
        {
            get { return this.Validate() == SpriteSkinValidationResult.Ready; }
        }

        void OnDestroy()
        {
            DeactivateSkinning();
        }

        internal void DeactivateSkinning()
        {
            var sprite = spriteRenderer.sprite;
            if (sprite != null)
                InternalEngineBridge.SetLocalAABB(spriteRenderer, sprite.bounds);

            spriteRenderer.DeactivateDeformableBuffer();
        }

        internal void ResetSprite()
        {
            m_CurrentDeformSprite = 0;
            CacheValidFlag();
        }

        /// <summary>
        /// Called before object is serialized.
        /// </summary>
        public void OnBeforeSerialize()
        {
            OnBeforeSerializeBatch();
        }

        /// <summary>
        /// Called after object is deserialized.
        /// </summary>
        public void OnAfterDeserialize()
        {
            OnAfterSerializeBatch();
        }
    }
}
