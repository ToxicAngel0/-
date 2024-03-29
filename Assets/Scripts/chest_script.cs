using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using Unity.VisualScripting;
using UnityEngine;

using UnityEngine.UIElements;
public class chest_script : MonoBehaviour
{
    public UIDocument uiDocument;
    [SerializeField] GameObject Player;
    [SerializeField] GameObject help_message;
    [SerializeField] Animator anim;
    [SerializeField] bool CanOpen = false;
    [SerializeField] bool isOpen = false;
    [SerializeField] GameObject[] chestLoot = new GameObject[4];
    [SerializeField] GameObject[] chestCells = new GameObject[4];
    [SerializeField] GameObject Window_chest;
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
       
        anim = GetComponent<Animator>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == Player.gameObject)
        {
            help_message.SetActive(true);
            CanOpen= true;
            
        }

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == Player.gameObject)
        {
            help_message.SetActive(false);
            CanOpen= false;
            Window_chest.SetActive(false);
        }

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && CanOpen && !isOpen) 
        {
            anim.SetTrigger("open");
            isOpen= true;
            Window_chest.SetActive(true);
            for (int i = 0; i < chestLoot.Length; i++)
            {
                if (chestLoot[i])
                {
                    GameObject item = Instantiate(chestLoot[i], chestCells[i].transform.position, Quaternion.identity) as GameObject;
                    /*var button =  chestCells[i].GetComponent<Button>();*/
                    /*item.AddComponent<UnityEngine.UI.Button>(); 
                    item.GetComponent<Button>().clicked += item.gameObject.GetComponent<item>().OnMouseDown;*/
                    
                    if (chestLoot[i] != null)
                    {
                        chestLoot[i] = null;
                    }
                    item.transform.SetParent(chestCells[i].transform, true);
                }
            }

        }
        else if (Input.GetKeyDown(KeyCode.E) && CanOpen && isOpen)
        {
            anim.SetTrigger("close");
            isOpen = false;
            Window_chest.SetActive(false);
        }
    }

    void OnClick()
    {
        Debug.Log("Clicked!");
    }
}
