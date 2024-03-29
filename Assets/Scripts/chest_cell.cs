using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class chest_cell : MonoBehaviour
{
    // Start is called before the first frame update
    public item item_;
    public void TakeItem()
    {
        item_ = this.GetComponentInChildren<item>();
        item_.OnMouseDown();
    }
}
