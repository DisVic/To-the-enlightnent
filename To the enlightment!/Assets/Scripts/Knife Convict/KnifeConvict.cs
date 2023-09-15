using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class KnifeConvict : MonoBehaviour
{
    private float timeBtwAttack;//переменная, которая отвечает за перерыв между атаками
    [SerializeField] private float startTimeBtwAttack;//переменная, в которой ведётся отсчёт перерыва между снарядами

    [SerializeField] private int health;//здоровье противника
    [SerializeField] private float speed;//скорость противника
    public int damage;//урон, который наносит враг по главному герою (только для ближнего боя)
    [SerializeField] private float lifetime;//время для удаления эффектов после нанесения урона
    [SerializeField] private GameObject deathEffet;//эффект при ударе
    private Player player;//переменная класса Player
    private Animator animator;//переменная для исправной работы анимаций

    private void Awake()
    {
        animator = GetComponent<Animator>();//получение анимаций для противника
        player = FindObjectOfType<Player>();//передача в переменную информации о глан\вном герое
    }
    void Update()
    {
        DeathOfTheEnemy();
        FlipSpriteEnemy();
        GoingOnHero();
    }
    public void TakeDamage(int damage)//функция получения урона от героя
    {
        health -= damage;
    }
    private void OnTriggerStay2D(Collider2D other)//функция для агрессии противника на героя
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
        SpecialEffectForDamage();
        player.HeatlthChange(-damage);
        timeBtwAttack = startTimeBtwAttack;
    }
    private void SpecialEffectForDamage()//специальный эффект для смерти или нанесения урона
    {
        GameObject deathEffect = Instantiate(deathEffet, transform.position, Quaternion.identity);
        Destroy(deathEffect, lifetime);
    }
    private void GoingOnHero() => transform.position = Vector2.MoveTowards(transform.position,
        player.transform.position, speed * Time.deltaTime);//расчёт движения противника в направлении героя
    private void FlipSpriteEnemy() => transform.eulerAngles = player.transform.position.x > transform.position.x ? new Vector3(0, 180, 0) :
        new Vector3(0, 0, 0);//условие для разворота спрайта противника для более нормального геймплея
    private void DeathOfTheEnemy()//функция смерти противника
    {
        if (health <= 0)//при достижении очков здоровья 0 противник умирает
        {
            SpecialEffectForDamage();
            Destroy(gameObject);
        }
    }
}
