using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cainos.PixelArtTopDown_Basic
{
    public class TopDownCharacterController : MonoBehaviour //класс в Юнити, от которого нследуютс все компоненты
    {
        public float speed;//скорость передвижения персонажа
        private Animator animator;//используется для доступа к компоненту аниматора, привязанному к этому объекту.
        private void Start()//метод, запускается при запуске игры
        {
            animator = GetComponent<Animator>();
        }


        private void Update()//метод вызывается кадр игры
        {
            Vector2 dir = Vector2.zero;//новый вектор dir с начальными значениями (0, 0), который будет представлять направление движения
            if (Input.GetKey(KeyCode.A))//проверка нажатия
            {
                dir.x = -1;//задается направление
                animator.SetInteger("Direction", 3);
            }
            else if (Input.GetKey(KeyCode.D))
            {
                dir.x = 1;
                animator.SetInteger("Direction", 2);
            }

            if (Input.GetKey(KeyCode.W))
            {
                dir.y = 1;
                animator.SetInteger("Direction", 1);
            }
            else if (Input.GetKey(KeyCode.S))
            {
                dir.y = -1;
                animator.SetInteger("Direction", 0);
            }

            dir.Normalize();//чтобы скорость была равномерной
            animator.SetBool("IsMoving", dir.magnitude > 0);//проверка на то, движется ли персонаж

            GetComponent<Rigidbody2D>().velocity = speed * dir;//настройка скорости
            if (Input.GetKey(KeyCode.LeftShift))//ускорение
            {
                GetComponent<Rigidbody2D>().velocity = 2 * speed * dir;
            }

        }
    }
}
