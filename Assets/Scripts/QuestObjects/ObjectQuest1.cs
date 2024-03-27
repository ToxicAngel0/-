using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ObjectQuest1 : MonoBehaviour
{
    public NPC_Task Taskk;
    public Text QuestName;
    public Text QuestDescription;
    public Animator statue;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            Taskk.status = true;
            QuestDescription.text = "Не будьте врединой, мастер, не говорите старой карге обо мне";
            statue.SetTrigger("start");
        }
    }
}
