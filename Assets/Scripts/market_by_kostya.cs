using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class market_by_kostya : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject[] marketLoot = new GameObject[4];
    [SerializeField] GameObject[] marketCells = new GameObject[4];
    [SerializeField] GameObject Player;
    [SerializeField] GameObject help_message;
    [SerializeField] bool CanOpen = false;
    [SerializeField] bool isOpen = false;
    [SerializeField] GameObject Window_chest;
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == Player.gameObject)
        {
            help_message.SetActive(true);
            CanOpen = true;

        }

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == Player.gameObject)
        {
            help_message.SetActive(false);
            CanOpen = false;
            Window_chest.SetActive(false);
        }

    }

    //// Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.E) && CanOpen && !isOpen)
        {
            
            isOpen = true;
            Window_chest.SetActive(true);
            for (int i = 0; i < marketLoot.Length; i++)
            {
                if (marketLoot[i])
                {
                    GameObject item = Instantiate(marketLoot[i], marketCells[i].transform.position, Quaternion.identity) as GameObject;
                    /*var button =  chestCells[i].GetComponent<Button>();*/
                    /*item.AddComponent<UnityEngine.UI.Button>(); 
                    item.GetComponent<Button>().clicked += item.gameObject.GetComponent<item>().OnMouseDown;*/

                    if (marketLoot[i] != null)
                    {
                        marketLoot[i] = null;
                    }
                    item.transform.SetParent(marketCells[i].transform, true);
                }
            }

        }
        else if (Input.GetKeyDown(KeyCode.E) && CanOpen && isOpen)
        {
            
            isOpen = false;
            Window_chest.SetActive(false);
        }
    }
}
