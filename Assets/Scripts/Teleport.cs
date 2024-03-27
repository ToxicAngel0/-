using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    public GameObject pointTeleport;
    public GameObject pointTeleport1;
    public GameObject Player;
    public GameObject Window_for_teleport;
    public GameObject Black_screen;
    public bool OnStreet = true;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == Player)
        {
            Window_for_teleport.SetActive(true);
        }
    }

    public void Teleportator()
    {
        Black_screen.SetActive(true);
        if (OnStreet)
        {
            Player.gameObject.transform.position = pointTeleport.gameObject.transform.position;
            OnStreet = false;
        }
        else if (!OnStreet)
        {
            Player.gameObject.transform.position = pointTeleport1.gameObject.transform.position;
            OnStreet = true;
        }
        StartCoroutine(TestCorotine());
        /*Black_screen.SetActive(false);*/
    }

    IEnumerator TestCorotine()
    {
        yield return new WaitForSeconds(1.5f);
        Black_screen.SetActive(false);
    }
    public void Exit_for_window()
    {
        Window_for_teleport.SetActive(false);
    }
}
