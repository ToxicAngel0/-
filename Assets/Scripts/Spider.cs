using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Spider : MonoBehaviour, IDamaged
{
    private float HP = 100f;
    private UnityEngine.Object enemyRef;
    public NPC_Task statue;
    public Transform target;
    public HpBar hp;
    public float Exp_after_death = 20;
    public GameObject coin;
    private void Start()
    {
        enemyRef = Resources.Load("Павук");
        target = GetComponent<Pathfinding.AIDestinationSetter>().target;
        hp = GameObject.FindWithTag("Player").GetComponent<HpBar>();
        statue = GameObject.FindWithTag("Statue").GetComponent<NPC_Task>();
    }

    void Respawn()
    {
        GameObject enemyCopy = (GameObject)Instantiate(enemyRef);
        enemyCopy.transform.position = transform.position;
        enemyCopy.GetComponent<Pathfinding.AIDestinationSetter>().target = target;


        Destroy(gameObject);
    }

    public void Damage(int count)
    {
        HP -= count;

        if (HP <= 0)
        {   
            if (statue.step == 3)
            {
                statue.smiles++;
            }
            gameObject.SetActive(false);
             Instantiate(coin, this.transform.position, Quaternion.identity);
            Invoke("Respawn", 10f);
            hp.Now_exp += Exp_after_death;
            
        }
       
    }
}