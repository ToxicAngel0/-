using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class quest_window_open : MonoBehaviour
{
    public GameObject window;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            if (window.activeSelf)
            {
                window.SetActive(false);
            }
            else
            {
                window.SetActive(true);
            }
        }
    }
}
