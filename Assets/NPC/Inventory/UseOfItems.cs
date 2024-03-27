using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseOfItems : MonoBehaviour
{
    public static UseOfItems instance;
    private void Start()
    {
        instance = this;
    }

    public void Use(Item item)
    {
        if(item.isHealing)
        {
            Debug.Log("Вы исцелились на" + item.HealingPower);
        }
    }
}
