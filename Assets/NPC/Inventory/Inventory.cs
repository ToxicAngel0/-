using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;
    public Item[] items;
    public Transform SlotsParent;
    bool isOpened;
    public InventorySlot[] inventorySlots = new InventorySlot[12];

    private void Start()
    {
        instance = this;
        for(int i = 0; i < inventorySlots.Length; i++)
        {
            inventorySlots[i] = SlotsParent.GetChild(i).GetComponent<InventorySlot>();
        }
    }
    public void PitInEmptySlot(Item item, GameObject obj)
    {
        for (int i = 0; i < inventorySlots.Length;i++)
        {
            if (inventorySlots[i].SlotItem == null)
            {
                inventorySlots[i].PutInSlot(item, obj);
                return;
            }
        }
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.I))
        {
            if (isOpened)
                Close();
            else
                Open();
        }
    }
    void Open()
    {
        gameObject.transform.localScale = Vector3.one;
        isOpened = true;
    }

    void Close()
    {
        gameObject.transform.localScale = Vector3.zero;
        isOpened = false;
    }
}
