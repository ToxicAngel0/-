using System.Collections;
using System.Collections.Generic;
using Unity.Collections;

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
         // ������ ���� ��� ������
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
                Quest.text = "� ������� �����, � ����� ���� ����� ��� ������� ��������. Ÿ �����-�� ����� ������ �����������, �� ��� �� �������� ������ ����� �����.";
                step = 1;
                Set_Quest("������� ��������", "����� �������� ������� ��������� ������");
                statue.SetTrigger("stop");
                Banana.SetTrigger("start");
            }
            else if (step == 1 && status)
            {
                Quest.text = "�, ������� �����, �� ��������� � ���� ����������� �������!";
                Complete_Quest();
                statue.SetTrigger("stop");
                step = 2;
                status = false;
            }
            else if (step == 1 && !status)
            {
                Quest.text = "����� ����, �����, �������. � �������� �������� ������� �������� ����. �� ������� �� ������� � ����������� ����.";
            }
            else if (step == 2)
            {
                Quest.text = "������� ����� ���������� �� ������� ������ ������. �����, ���� �������.";
                statue.SetTrigger("stop");
                Set_Quest("������� �������", "������� 3 ������� � ������ ����� � ������ ��.");
                
                step = 3;
            }
            else if (step == 3 && !status)
            {
                Quest.text = "������� �����, �� ����� ���� ���������� ����";
            }
            else if (step == 3 && status)
            {
                Quest.text = "�, �� ��������� � ���� ����������� �������!";
                Complete_Quest();
                statue.SetTrigger("stop");
                step = 4;
                status = false;
            }
            else if (step == 4)
            {
                Quest.text = "� ������� �����, ����������� �� ��, � ������ ��������. ���, ����������� ���, �� ���� ��� �����";
                Set_Quest("������� ��� ��� ����� ����", "��������, ��� ���������� �� ��������.");
                statue.SetTrigger("stop");
               
                step = 5;
            }
            else if (step == 5 && !status)
            {
                Quest.text = "�� �����, �����. ���� � ��� �������� ��������� ���, ���� �� �������� �� �������, � ������ ����";
                
            }
            else if (step == 5 && status)
            {
                Quest.text = "�, �� ��������� � ���� ����������! ��������� ���� �� ������ � �������� �����. ������ � ������� � ����� ����. ����������� �� ��, � �����, ��� ��������. ������ �� ������� ���������. �� �������� ���� � �����, ��� ���������� ���������� �������.";
                Set_Quest("����� �����������", "����������� � ��������� ����������. ������ �������, ��� �� �������� ���� � ����� �����.");
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
            window_quest.SetTrigger("close"); // ������ ���� ��� ������ �� ������� NPC
        }

    }
}