using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpObject : MonoBehaviour
{
    public Item item;
    private GameObject itemObj;

    private void Start()
    {
        itemObj = gameObject; //нужно, чтобы спавнить предмет из инвентаря
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && (Input.GetKey(KeyCode.E)))
        {
            Inventory.instance.PitInEmptySlot(item, itemObj);
            gameObject.SetActive(false);
        }
    }
    
}
