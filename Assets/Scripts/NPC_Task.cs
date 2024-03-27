using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Properties;
using UnityEngine;
using UnityEngine.UI;

public class NPC_Task : MonoBehaviour
{
    public int step = 0;
    public Text Quest;
    public Text QuestName;
    public Text QuestDescription;
    public GameObject windoww;
    public bool status = false;
    public Animator window_quest;
    public Animator statue;
    public Animator Banana;
    public int smiles;
    void Start()
    {
         // Скрыть окно при старте
    }

    private void Update()
    {
        if (step==3 && smiles>=3 && !status)
        {
            status = true;

        }
    }

    private void Set_Quest(string name, string description)
    {
        QuestName.text = name;
        QuestDescription.text = description;
    }

    private void Complete_Quest()
    {
        QuestName.text = "";
        QuestDescription.text = "";
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            window_quest.SetTrigger("open 0");
            
            if (step == 0)
            {
                Quest.text = "О великий герой, я прошу тебя найти мою древнюю реликвию. Её когда-то давно украли авантюристы, но они не покинули границ моего храма.";
                step = 1;
                Set_Quest("Древняя реликвия", "Найди реликвию древней каменнной стаути");
                statue.SetTrigger("stop");
                Banana.SetTrigger("start");
            }
            else if (step == 1 && status)
            {
                Quest.text = "О, великий герой, ты справился с этой непосильной задачей!";
                Complete_Quest();
                statue.SetTrigger("stop");
                step = 2;
                status = false;
            }
            else if (step == 1 && !status)
            {
                Quest.text = "Прошу тебя, герой, поспеши. В реликвии заточена древняя огромная сила. Не позволь ей попасть в недостойные руки.";
            }
            else if (step == 2)
            {
                Quest.text = "Мерзкая слизь поселилась на могилах бравых войнов. Прошу, убей чудовищ.";
                statue.SetTrigger("stop");
                Set_Quest("Геноцид слизней", "Найдите 3 слизней в руинах храма и убейте их.");
                
                step = 3;
            }
            else if (step == 3 && !status)
            {
                Quest.text = "Славный герой, не бойся этих порождений тьмы";
            }
            else if (step == 3 && status)
            {
                Quest.text = "О, ты справился с этой непосильной задачей!";
                Complete_Quest();
                statue.SetTrigger("stop");
                step = 4;
                status = false;
            }
            else if (step == 4)
            {
                Quest.text = "О великий герой, отправляйся на юг, к новому кладбищу. Зло, затаившееся там, не дает мне покоя";
                Set_Quest("Древнее зло или новый друг", "Выяснить, что происходит на кладбище.");
                statue.SetTrigger("stop");
               
                step = 5;
            }
            else if (step == 5 && !status)
            {
                Quest.text = "Не бойся, герой. Если в мои владения пробрался тот, кого ты победить не сможешь, я защищу тебя";
                
            }
            else if (step == 5 && status)
            {
                Quest.text = "О, ты справился с этим испытанием! Благодарю тебя за помощь в очищении храма. Теперь я уверена в твоей силе. Отправляйся на юг, к месту, где появился. Взойди на каменый постамент. Он отправит тебя в замок, где скрывается приспешник Первого.";
                Set_Quest("Новое приключение", "Отправиться к каменному постаменту. Богиня говорит, что он отправит меня в новое место.");
                Complete_Quest();
                statue.SetTrigger("stop");
                status = false;
            }

        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            window_quest.SetTrigger("close"); // Скрыть окно при выходе из области NPC
        }

    }
}