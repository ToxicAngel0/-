using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class item : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject Player;
    public string Name;
    GameObject[] chestLoot = new GameObject[12];
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        chestLoot = Player.GetComponent<PlayerManager>().chestLoot;
    }
    public void OnMouseDown()
    {
        Debug.Log("норм");
        for (int i = 0; i < Player.GetComponent<PlayerManager>().chestLoot.Length; i++)
        {
            if (Player.GetComponent<PlayerManager>().chestLoot[i].GetComponent<cell>().status)
            {

                GameObject item = Instantiate(this.gameObject,chestLoot[i].gameObject.transform.position, Quaternion.identity) as GameObject;
                
                item.transform.SetParent(chestLoot[i].transform, true);
                item.transform.localScale = new Vector3(131f, 131f);
                chestLoot[i].GetComponent<cell>().status = false;
                Destroy(this.gameObject);
                break;
            }
        }

    }
}
