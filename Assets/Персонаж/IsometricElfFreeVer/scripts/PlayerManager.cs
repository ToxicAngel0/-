using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;
    // Публичные переменные для точек и префабов
    public Transform ItenPoint;
    public Transform ShotPoint;
    public GameObject ItemPrefab;
    public GameObject ThrowPrefab;
    public GameObject BowPrefab;
    public GameObject Inventory;
    public bool CanAttack = false;
    Rigidbody2D rb; // Переменные для компонентов Rigidbody2D и Animator
    Animator animator;
    public float moveSpeed = 1f; // Скорость движения персонажа
    public item item_in_cell;
    [SerializeField]// Приватное поле для точки стрельбы
    private Transform shotPointTransform = null;
    public GameObject[] chestLoot = new GameObject[12];
    public HpBar hp;
    public int Arrows;

    void Start()
    {
        instance = this;
        // Установка желаемой частоты кадров и инициализация Rigidbody2D и Animator
        Application.targetFrameRate = 60;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        Arrows = 10;
    }
    void Update()
    {// Получение ввода от пользователя для оси X и Y
        float x = Input.GetAxisRaw("Horizontal");
        float y = (x == 0) ? Input.GetAxisRaw("Vertical") : 0.0f;
        // Управление анимацией ходьбы в зависимости от ввода
        if (x != 0 || y != 0)
        {
            animator.SetFloat("x", x);
            animator.SetFloat("y", y);
            animator.SetBool("Walk", true);
        }
        else
        {
            animator.SetBool("Walk", false);
        }
        // Выполнение действий игрока в корутине
        StartCoroutine(Action());
        Shot();// Выстрелы из оружия при нажатии соответствующих клавиш
        for (int i = 0; i < chestLoot.Length; i++)
        {
            if (chestLoot[i].GetComponent<cell>().status==false)
            {

                item_in_cell = chestLoot[i].GetComponentInChildren<item>();
                if (item_in_cell.Name == "Bow")
                {
                    CanAttack = true;
                }
                else if (item_in_cell.Name == "Meat") //еда исчезает сразу же, поэтому статус становится фалс
                {
                    hp.HP += 20;
                    Destroy(item_in_cell.gameObject);
                    chestLoot[i].GetComponent<cell>().status = true;
                }
                else if (item_in_cell.Name == "Apple") //еда исчезает сразу же, поэтому статус становится фалс
                {
                    hp.HP += 10;
                    Destroy(item_in_cell.gameObject);
                    chestLoot[i].GetComponent<cell>().status = true;
                }
                else if (item_in_cell.Name == "Arrow") //еда исчезает сразу же, поэтому статус становится фалс
                {
                    Arrows += 10;
                    Destroy(item_in_cell.gameObject);
                    chestLoot[i].GetComponent<cell>().status = true;
                }
            }
        }
    }
    IEnumerator Action()// Корутина для обработки различных действий игрока
    { // Действия при нажатии определенных клавиш
        if (Input.GetKeyDown(KeyCode.Z))
        {
            animator.SetTrigger("Slash");
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            animator.SetTrigger("Guard");
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            animator.SetTrigger("Item");
            Instantiate(ItemPrefab, ItenPoint.position, transform.rotation);
        }
                              

        if (Input.GetKeyDown(KeyCode.M))
        {
            animator.SetTrigger("Dead");
            this.transform.position = new Vector2(0f, -0.12f);
            for (var i = 0; i < 64; i++)
            {
                yield return null;
            }
            this.transform.position = Vector2.zero;
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (Inventory.activeSelf==true)
            {
                Inventory.SetActive(false);
            }
            else if (Inventory.activeSelf==false) 
            {
                Inventory.SetActive(true);
            }
        }
    }
    private float GetRotation()// Метод для получения угла поворота при стрельбе
    {
        if (animator.GetFloat("x") == -1 && animator.GetFloat("y") == 0)
        {
            return 0;
        }
        else if (animator.GetFloat("x") == 1 && animator.GetFloat("y") == 0)
        {
            return 180;
        }
        else if (animator.GetFloat("x") == 0 && animator.GetFloat("y") == 1)
        {
            return -90;
        }
        else
        {
            return 90;
        }
    }
    // Переменные для управления частотой стрельбы

    private float cooldown = 0;
    void Shot()
    {
        if (CanAttack)
        {
            if (Input.GetKeyDown(KeyCode.X) && cooldown <= 0)
            {
                cooldown = 0.5f;
                animator.SetTrigger("Throw");
                Instantiate(ThrowPrefab, transform.position, Quaternion.Euler(new Vector3(0, 0, GetRotation())), null);
            }

            if (Input.GetKeyDown(KeyCode.C) && cooldown <= 0 && Arrows>=1)
            {
                cooldown = 0.5f;
                animator.SetTrigger("Bow");
                Arrows--;
                // Создаем экземпляр объекта BowPrefab в позиции текущего объекта (transform.position)
                // с поворотом, заданным через Quaternion.Euler с помощью GetRotation() для оси Z
                Instantiate(BowPrefab, transform.position, Quaternion.Euler(new Vector3(0, 0, GetRotation())), null);
            }
            if (cooldown >= 0)    // Если cooldown больше или равен нулю, уменьшаем его на значение, пропорциональное времени прошедшему с предыдущего кадра (Time.deltaTime)
                cooldown -= Time.deltaTime;
        }
    }

    public void Check()
    {
        for (int i = 0; i < chestLoot.Length; i++)
        {
            if (chestLoot[i].GetComponent<cell>() != null)
            {
                item item_in_cell = chestLoot[i].GetComponentInChildren<item>();
                if (item_in_cell.name == "Bow")
                {
                    CanAttack = true;
                }
            }
        }
    }
}




