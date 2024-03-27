using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial_activator : MonoBehaviour
{
    // Start is called before the first frame update
    public Tutorial tutorial;
    public int step;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D player)
    {
        if (player.CompareTag("Player"))
        {
            tutorial.Start_Step(step);
        }
        
    }

    private void OnTriggerExit2D(Collider2D player)
    {
        if (player.CompareTag("Player"))
        {
             tutorial.End_Step(); 
             jopka_murabvya();
        }
       
    }

    public void jopka_murabvya()
    {
        this.gameObject.SetActive(false);
    }
}
