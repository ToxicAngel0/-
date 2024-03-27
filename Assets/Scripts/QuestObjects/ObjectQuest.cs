using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ObjectQuest : MonoBehaviour
{
    public NPC_Task Taskk;
    public Text QuestName;
    public Text QuestDescription;
    public Animator statue;
    void Start()
    {
        
    }
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            Taskk.status = true;
            Destroy(gameObject);
            QuestDescription.text = "Раскумарься оболдуй, тащи валыну каменной женщине и не очкуй";
            statue.SetTrigger("start");
        }
    }
}

