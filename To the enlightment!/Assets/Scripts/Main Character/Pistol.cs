using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : MonoBehaviour
{
    [SerializeField]private float offset;//переменная для корректировки направления дула оружия/снаряда данного вида оружия

    public GunType gunType;//переменная, определяющая вид оружия (вражеский/героя)
    public GameObject bullet;//создание объекта пули
    public Transform startPoint;//начальная позиция выстрела снаряда
    public Joystick joystick;//объявление джойстика для стрельбы на Андроид

    private float Cooldown;//перерыв между выстрелами
    public float startCooldown;//начало перерыва между выстрелами
    private float rZ;//переменная для следования дула оружия за курсором мыши

    private Player player;//объявление переменной из другого скрипта "Игрок"
    private Vector3 difference;//объявление переменной, отвечающей за разницу между курсором и дулом оружия

    public enum GunType { Default,Enemy}//список для принадлежности оружий

    private Camera cam;//переменная типа Камера также для корректной работы стрельбы оружий
    private void Start()
    {
        player=GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();//присваивание переменной свойств объекта с тэгом "Player"
        if (player.controlType == Player.ControlType.PC && gunType==GunType.Default)//при игре на ПК и условии, что вы играете за героя, джойстик отключается
        {
            joystick.gameObject.SetActive(false);
        }
        cam = Camera.main;//главная камера также влияет на  связи курсора и дула оружия
    }
    void Update()
    {
        if (!PauseMenu.isPaused)
        {
            if (gunType == GunType.Default)
            {
                if (player.controlType == Player.ControlType.PC)
                {
                    difference = cam.ScreenToWorldPoint(Input.mousePosition) - transform.position;//следование дула пистолета за курсором на ПК при условии, что герой, обладающий оружием, является героем
                    rZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
                }
                else if (player.controlType == Player.ControlType.Android && Mathf.Abs(joystick.Horizontal) > 0.3f || Mathf.Abs(joystick.Vertical) > 0.3f)
                {
                    rZ = Mathf.Atan2(joystick.Vertical, joystick.Horizontal) * Mathf.Rad2Deg;//На Андроид оружие стреляет туда, куда направлен джойстик
                }
            }
            else if (gunType == GunType.Enemy)
            {
                difference = player.transform.position - transform.position;//оружие противника направлено в сторону главного героя
                rZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
            }
            transform.rotation = Quaternion.Euler(0, 0, rZ + offset);//корректировка стрельбы при разном rotation спрайтов оружия и снарядов
            if (Cooldown <= 0)
            {
                if (Input.GetMouseButton(0) && player.controlType == Player.ControlType.PC || gunType == GunType.Enemy)//Стрельба на ПК для героя или противника при отсутствии перерыва между выстрелами
                {
                    Shoot();
                }
                else if (player.controlType == Player.ControlType.Android)//если джойстик игрока на Андроид не покоится, то оружие стреляет по напрвлению
                {
                    if (joystick.Vertical != 0 || joystick.Horizontal != 0)
                    {
                        Shoot();
                    }
                }
            }
            else
            {
                Cooldown -= Time.deltaTime;//перерыв продолжает действовать с момента свой остановки
            }
        }
    }
    public void Shoot()//функция стрельбы
    {
        Instantiate(bullet, startPoint.position, startPoint.rotation);
        Cooldown = startCooldown;
    }
}
