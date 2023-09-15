using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class KnifeConvict : MonoBehaviour
{
    private float timeBtwAttack;//����������, ������� �������� �� ������� ����� �������
    [SerializeField] private float startTimeBtwAttack;//����������, � ������� ������ ������ �������� ����� ���������

    [SerializeField] private int health;//�������� ����������
    [SerializeField] private float speed;//�������� ����������
    public int damage;//����, ������� ������� ���� �� �������� ����� (������ ��� �������� ���)
    [SerializeField] private float lifetime;//����� ��� �������� �������� ����� ��������� �����
    [SerializeField] private GameObject deathEffet;//������ ��� �����
    private Player player;//���������� ������ Player
    private Animator animator;//���������� ��� ��������� ������ ��������

    private void Awake()
    {
        animator = GetComponent<Animator>();//��������� �������� ��� ����������
        player = FindObjectOfType<Player>();//�������� � ���������� ���������� � ����\���� �����
    }
    void Update()
    {
        DeathOfTheEnemy();
        FlipSpriteEnemy();
        GoingOnHero();
    }
    public void TakeDamage(int damage)//������� ��������� ����� �� �����
    {
        health -= damage;
    }
    private void OnTriggerStay2D(Collider2D other)//������� ��� �������� ���������� �� �����
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
    public void OnENemyAttack()//������� ��� ��������� ����� �� �����
    {
        SpecialEffectForDamage();
        player.HeatlthChange(-damage);
        timeBtwAttack = startTimeBtwAttack;
    }
    private void SpecialEffectForDamage()//����������� ������ ��� ������ ��� ��������� �����
    {
        GameObject deathEffect = Instantiate(deathEffet, transform.position, Quaternion.identity);
        Destroy(deathEffect, lifetime);
    }
    private void GoingOnHero() => transform.position = Vector2.MoveTowards(transform.position,
        player.transform.position, speed * Time.deltaTime);//������ �������� ���������� � ����������� �����
    private void FlipSpriteEnemy() => transform.eulerAngles = player.transform.position.x > transform.position.x ? new Vector3(0, 180, 0) :
        new Vector3(0, 0, 0);//������� ��� ��������� ������� ���������� ��� ����� ����������� ��������
    private void DeathOfTheEnemy()//������� ������ ����������
    {
        if (health <= 0)//��� ���������� ����� �������� 0 ��������� �������
        {
            SpecialEffectForDamage();
            Destroy(gameObject);
        }
    }
}
