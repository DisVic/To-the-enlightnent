using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed;//���������� ��� �������� �������� ����������
    [SerializeField] private float lifetime;//���������� ��� ����������� ������� ������������� �������� � ��������
    [SerializeField] private float distance;//���������� ��� ����������� ��������� ����� �������
    public int damage;//������� ������ ������ ����� ����������
    [SerializeField] private GameObject Fire;//������ ��������
    [SerializeField] bool EnemyBullet;//�������� ������� - ��������� ��� �����

    public LayerMask solid;//�������� ���� ��� ��������� ����� �� �����������
    void Update()
    {
        HitInfo();
    }
    private void Awake()
    {
        SpecialEffectForShoot();
    }
    private void SpecialEffectForShoot()
    {
        GameObject fire = Instantiate(Fire, transform.position, Quaternion.identity);//��������� � �������� ���� ������� (�������������)
        Destroy(fire, lifetime);
        Destroy(gameObject, lifetime);
    }
    private void TrajectoryOfBullet() => transform.Translate(Vector2.right * speed * Time.deltaTime);//������ ��������
    private void HitInfo()//������������ ��������� ����� �� ���������
    {
        RaycastHit2D hitsinfo = Physics2D.Raycast(transform.position, transform.up, distance, solid);//����������, ������������� ��������� �� ���� ����������
        if (hitsinfo.collider != null)
        {
            if (hitsinfo.collider.CompareTag("Enemy"))
            {
                hitsinfo.collider.GetComponent<KnifeConvict>().TakeDamage(damage);//��������� ����� �� ����������
                SpecialEffectForShoot();
                Destroy(gameObject);
            }
            else if (hitsinfo.collider.CompareTag("Player") && EnemyBullet)
            {
                hitsinfo.collider.GetComponent<Player>().HeatlthChange(-damage);//��������� ����� �� �������� �����
                SpecialEffectForShoot();
                Destroy(gameObject);
            }          
        }
        TrajectoryOfBullet();
    }
}
