using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class Mobs : MonoBehaviour
{
    static int AnimatorWalk = Animator.StringToHash("Walk"); // Хэш для параметра анимации "Walk"
    static int AnimatorAttack = Animator.StringToHash("Attack"); // Хэш для параметра анимации "Attack"
    Animator _animator; // Переменная для компонента аниматора

    void Awake()
    {
        _animator = GetComponentInChildren<Animator>(); // Получение компонента Animator из дочерних объектов
    }

    void Start()
    {
        StartCoroutine(Animate()); // Запуск корутины Animate() при запуске скрипта
    }

    IEnumerator Animate()
    {
        yield return new WaitForSeconds(5f); // Ожидание 5 секунд перед началом анимации

        while (true) // Бесконечный цикл для выполнения анимаций
        {
            _animator.SetBool(AnimatorWalk, true); // Установка параметра анимации "Walk" в true, начало ходьбы
            yield return new WaitForSeconds(1f); // Ожидание 1 секунды

            // Отражение моба по оси X для создания эффекта поворота
            _animator.transform.localScale = new Vector3(-_animator.transform.localScale.x,
                                                         _animator.transform.localScale.y,
                                                         _animator.transform.localScale.z);
            yield return new WaitForSeconds(1f); // Ожидание 1 секунды

            _animator.SetBool(AnimatorWalk, false); // Установка параметра анимации "Walk" в false, окончание ходьбы
            yield return new WaitForSeconds(1f); // Ожидание 1 секунды

            _animator.SetTrigger(AnimatorAttack); // Запуск анимации атаки путем установки триггера "Attack"
            yield return new WaitForSeconds(1f); // Ожидание 1 секунды

            // Повторное использование анимации атаки три раза
            _animator.SetTrigger(AnimatorAttack);
            yield return new WaitForSeconds(1f);

            _animator.SetTrigger(AnimatorAttack);
            yield return new WaitForSeconds(5f); // Ожидание 5 секунд перед повторным запуском цикла анимации
        }
    }
}
