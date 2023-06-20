using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed;//���������� ��� �������� �������� ����������
    public float lifetime;//���������� ��� ����������� ������� ������������� �������� � ��������
    public float distance;//���������� ��� ����������� ��������� ����� �������
    public int damage;//������� ������ ������ ����� ����������
    public GameObject Fire;//������ ��������
    public LayerMask solid;//�������� ���� ��� ��������� ����� �� �����������

    [SerializeField] bool EnemyBullet;//�������� ������� - ��������� ��� �����

    void Update()
    {
        RaycastHit2D hitsinfo = Physics2D.Raycast(transform.position, transform.up, distance, solid);//����������, ������������� ��������� �� ���� ����������
        if (hitsinfo.collider != null)
        {
            if (hitsinfo.collider.CompareTag("Enemy"))
            {
                hitsinfo.collider.GetComponent<KnifeConvict>().TakeDamage(damage);//��������� ����� �� ����������
            }
            else if (hitsinfo.collider.CompareTag("Player") && EnemyBullet)
            {
                hitsinfo.collider.GetComponent<Player>().HeatlthChange(-damage);//��������� ����� �� �������� �����
            }
            GameObject fire = Instantiate(Fire, transform.position, Quaternion.identity);//��������� ���� ������� ��� �������� � ����������� ��� � ����� �����������
            Destroy(fire, lifetime);
            Destroy(gameObject);
        }
        transform.Translate(Vector2.right * speed * Time.deltaTime);//������ ��������

    }

    private void Awake()
    {
        GameObject fire = Instantiate(Fire, transform.position, Quaternion.identity);//��������� � �������� ���� ������� (�������������)
        Destroy(fire, lifetime);
        Destroy(gameObject, lifetime);
    }
}
