using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed;//переменная для скорости движения противника
    [SerializeField] private float lifetime;//переменная для определения времени существования эффектов и снарядов
    [SerializeField] private float distance;//переменная для определения дистанции полёта снаряда
    public int damage;//сколько снаряд нанесёт урона противнику
    [SerializeField] private GameObject Fire;//эффект выстрела
    [SerializeField] bool EnemyBullet;//проверка снаряда - вражеский или героя

    public LayerMask solid;//создание слоя для нанесения урона по противникам
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
        GameObject fire = Instantiate(Fire, transform.position, Quaternion.identity);//появление и удаление спец эффекта (перестраховка)
        Destroy(fire, lifetime);
        Destroy(gameObject, lifetime);
    }
    private void TrajectoryOfBullet() => transform.Translate(Vector2.right * speed * Time.deltaTime);//расчёт выстрела
    private void HitInfo()//отслеживание нанесения урона по персонажу
    {
        RaycastHit2D hitsinfo = Physics2D.Raycast(transform.position, transform.up, distance, solid);//переменная, отслеживающая попадание по телу противника
        if (hitsinfo.collider != null)
        {
            if (hitsinfo.collider.CompareTag("Enemy"))
            {
                hitsinfo.collider.GetComponent<KnifeConvict>().TakeDamage(damage);//нанесение урона по противнику
                SpecialEffectForShoot();
                Destroy(gameObject);
            }
            else if (hitsinfo.collider.CompareTag("Player") && EnemyBullet)
            {
                hitsinfo.collider.GetComponent<Player>().HeatlthChange(-damage);//нанесение урона по главному герою
                SpecialEffectForShoot();
                Destroy(gameObject);
            }          
        }
        TrajectoryOfBullet();
    }
}
