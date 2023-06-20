using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class KnifeConvict : MonoBehaviour
{
    private float timeBtwAttack;//переменная, которая отвечает за перерыв между атаками
    public float startTimeBtwAttack;//переменная, в которой ведётся отсчёт перерыва между снарядами

    public int health;//здоровье противника
    public float speed;//скорость противника
    public int damage;//урон, который наносит враг по главному герою (только для ближнего боя)
    public float lifetime;//время для удаления эффектов после нанесения урона
    public GameObject deathEffet;//эффект при ударе
    private Player player;//переменная класса Player
    private Animator animator;//переменная для исправной работы анимаций

    private void Start()
    {
        animator = GetComponent<Animator>();//получение анимаций для противника
        player = FindObjectOfType<Player>();//передача в переменную информации о глан\вном герое
    }

    public void TakeDamage(int damage)//функция получения урона от героя
    {
        health -= damage;
    }

    void Update()
    {
        if (health <= 0)//при достижении очков здоровья 0 противник умирает
        {
            GameObject deathEffect = Instantiate(deathEffet, transform.position, Quaternion.identity);//появление эффекта смерти и его удаление в угоду оптимизации
            Destroy(deathEffect, lifetime);
            Destroy(gameObject);
        }
        if(player.transform.position.x>transform.position.x)//условие для разворота спрайта противника для более нормального геймплея
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
        else
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);//расчёт движения противника в направлении героя
    }

    public void OnTriggerStay2D(Collider2D other)//функция для агрессии противника на героя
    {
        if (other.CompareTag("Player"))
        {
            if (timeBtwAttack <= 0)
            {
                animator.SetTrigger("EnemyAttack");
            }
            else
            {
                timeBtwAttack -= Time.deltaTime;
            }
        }
    }
    public void OnENemyAttack()//функция для нанесения урона по герою
    {
        GameObject deathEffect = Instantiate(deathEffet, transform.position, Quaternion.identity);
        Destroy(deathEffect, lifetime);
        player.HeatlthChange(-damage);
        timeBtwAttack = startTimeBtwAttack;
    }
}
