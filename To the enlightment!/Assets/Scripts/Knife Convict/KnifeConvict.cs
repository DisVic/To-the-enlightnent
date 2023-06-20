using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class KnifeConvict : MonoBehaviour
{
    private float timeBtwAttack;//����������, ������� �������� �� ������� ����� �������
    public float startTimeBtwAttack;//����������, � ������� ������ ������ �������� ����� ���������

    public int health;//�������� ����������
    public float speed;//�������� ����������
    public int damage;//����, ������� ������� ���� �� �������� ����� (������ ��� �������� ���)
    public float lifetime;//����� ��� �������� �������� ����� ��������� �����
    public GameObject deathEffet;//������ ��� �����
    private Player player;//���������� ������ Player
    private Animator animator;//���������� ��� ��������� ������ ��������

    private void Start()
    {
        animator = GetComponent<Animator>();//��������� �������� ��� ����������
        player = FindObjectOfType<Player>();//�������� � ���������� ���������� � ����\���� �����
    }

    public void TakeDamage(int damage)//������� ��������� ����� �� �����
    {
        health -= damage;
    }

    void Update()
    {
        if (health <= 0)//��� ���������� ����� �������� 0 ��������� �������
        {
            GameObject deathEffect = Instantiate(deathEffet, transform.position, Quaternion.identity);//��������� ������� ������ � ��� �������� � ����� �����������
            Destroy(deathEffect, lifetime);
            Destroy(gameObject);
        }
        if(player.transform.position.x>transform.position.x)//������� ��� ��������� ������� ���������� ��� ����� ����������� ��������
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
        else
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);//������ �������� ���������� � ����������� �����
    }

    public void OnTriggerStay2D(Collider2D other)//������� ��� �������� ���������� �� �����
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
        GameObject deathEffect = Instantiate(deathEffet, transform.position, Quaternion.identity);
        Destroy(deathEffect, lifetime);
        player.HeatlthChange(-damage);
        timeBtwAttack = startTimeBtwAttack;
    }
}
