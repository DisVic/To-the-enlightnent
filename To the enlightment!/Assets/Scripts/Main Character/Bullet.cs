using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed;//переменна€ дл€ скорости движени€ противника
    public float lifetime;//переменна€ дл€ определени€ времени существовани€ эффектов и снар€дов
    public float distance;//переменна€ дл€ определени€ дистанции полЄта снар€да
    public int damage;//сколько снар€д нанесЄт урона противнику
    public GameObject Fire;//эффект выстрела
    public LayerMask solid;//создание сло€ дл€ нанесени€ урона по противникам

    [SerializeField] bool EnemyBullet;//проверка снар€да - вражеский или геро€

    void Update()
    {
        RaycastHit2D hitsinfo = Physics2D.Raycast(transform.position, transform.up, distance, solid);//переменна€, отслеживающа€ попадание по телу противника
        if (hitsinfo.collider != null)
        {
            if (hitsinfo.collider.CompareTag("Enemy"))
            {
                hitsinfo.collider.GetComponent<KnifeConvict>().TakeDamage(damage);//нанесение урона по противнику
            }
            else if (hitsinfo.collider.CompareTag("Player") && EnemyBullet)
            {
                hitsinfo.collider.GetComponent<Player>().HeatlthChange(-damage);//нанесение урона по главному герою
            }
            GameObject fire = Instantiate(Fire, transform.position, Quaternion.identity);//по€вление спец эффекта при выстреле и уничтожение его в угоду оптимизации
            Destroy(fire, lifetime);
            Destroy(gameObject);
        }
        transform.Translate(Vector2.right * speed * Time.deltaTime);//расчЄт выстрела

    }

    private void Awake()
    {
        GameObject fire = Instantiate(Fire, transform.position, Quaternion.identity);//по€вление и удаление спец эффекта (перестраховка)
        Destroy(fire, lifetime);
        Destroy(gameObject, lifetime);
    }
}
