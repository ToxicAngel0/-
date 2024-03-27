using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionManager : MonoBehaviour
{
    Animator animator;
    float speed = 0.2f;
    public float leftTime;

    void Start()
    {
        // Находим позицию игрока
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Vector3 playerPosition = player.transform.position;

        // Устанавливаем позицию зелья над игроком
        transform.position = new Vector3(playerPosition.x, playerPosition.y + 0.75f, playerPosition.z);

        // Задаем движение зелья
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.up * speed;

        // Уничтожаем зелье по прошествии времени
        Destroy(gameObject, leftTime);
    }

    void Update()
    {

    }
}
