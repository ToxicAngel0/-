using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    [Header("Базовые характеистики")]
    public string Name = " ";
    public string Description = "Описание предмета";
    public Sprite icon = null;
    public GameObject Prefab;
    int number = 8;

    public bool isHealing;
    public float HealingPower;
}
