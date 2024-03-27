using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tp2 : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Player;
    public GameObject Window_for_teleport;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == Player)
        {
            Window_for_teleport.SetActive(true);
        }
    }
}
