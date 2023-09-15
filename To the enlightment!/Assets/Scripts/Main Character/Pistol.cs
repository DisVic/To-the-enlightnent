using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : MonoBehaviour
{
    [SerializeField] private float offset;//переменная для корректировки направления дула оружия/снаряда данного вида оружия
    [SerializeField] private GunType gunType;//переменная, определяющая вид оружия (вражеский/героя)
    [SerializeField] private GameObject bullet;//создание объекта пули
    [SerializeField] private Transform startPoint;//начальная позиция выстрела снаряда
    [SerializeField] private Joystick joystick;//объявление джойстика для стрельбы на Андроид

    private float Cooldown;//перерыв между выстрелами
    [SerializeField] private float startCooldown;//начало перерыва между выстрелами
    private float rZ;//переменная для следования дула оружия за курсором мыши

    private Player player;//объявление переменной из другого скрипта "Игрок"
    private Vector3 difference;//объявление переменной, отвечающей за разницу между курсором и дулом оружия

    public enum GunType { Default,Enemy}//список для принадлежности оружий

    private Camera cam;//переменная типа Камера также для корректной работы стрельбы оружий

    private void Start()
    {
        PlayerIsPlayer();
        OffJoystickOrNot();
        СamIsCamera();
    }
    void Update()
    {
        GameNotOnPause();
    }
    public void Shoot()//функция стрельбы
    {
        Instantiate(bullet, startPoint.position, startPoint.rotation);
        Cooldown = startCooldown;
    }
    private void СamIsCamera() => cam = Camera.main;//главная камера также влияет на  связи курсора и дула оружия
    private void OffJoystick() => joystick.gameObject.SetActive(false);//отключение джойстика для стрельбы
    private void OffJoystickOrNot()//при игре на ПК и условии, что вы играете за героя, джойстик отключается
    {
        if (player.controlType == Player.ControlType.PC && gunType == GunType.Default)
            OffJoystick();
    }
    private void PlayerIsPlayer() => player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();//присваивание переменной свойств объекта с тэгом "Player"
    private void DirectionOfHeroGun()//направление оружия
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
    private void DirectionEnemyGun()//оружие противника направлено в сторону главного героя
    {
        difference = player.transform.position - transform.position;
        rZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
    }
    private void AdjustmentOfShooting() => transform.rotation = Quaternion.Euler(0, 0, rZ + offset);//корректировка стрельбы при разном rotation спрайтов оружия и снарядов
    private void CheckTimeOnCooldown() => Cooldown -= Time.deltaTime;//перерыв продолжает действовать с момента свой остановки
    private void ShootingOnAndroid()//стрельба при отклонении джойстика на Android
    {
        if (joystick.Vertical != 0 || joystick.Horizontal != 0)
        {
            Shoot();
        }
    }
    private void DifferentShooting()//Стрельба на ПК для героя или противника при отсутствии перерыва между выстрелами
    {
        if (Cooldown <= 0 && Input.GetMouseButton(0) && player.controlType == Player.ControlType.PC || gunType == GunType.Enemy)
        {
            Shoot();
        }
        else if (player.controlType == Player.ControlType.Android)//если джойстик игрока на Андроид не покоится, то оружие стреляет по напрвлению
        {
            ShootingOnAndroid();
        }
    }
    private void DependenceOfCooldown()//зависимоть стрельбы от перезарядки выстрела
    {
        if (Cooldown <= 0)
        {
            DifferentShooting();
        }
        else
        {
            CheckTimeOnCooldown();
        }
    }
    private void GameNotOnPause()//игра на паузе
    {
        if (!PauseMenu.isPaused)
        {
            switch (gunType)
            {
                case GunType.Default: DirectionOfHeroGun(); break;
                case GunType.Enemy: DirectionEnemyGun(); break;
                default: throw new ArgumentOutOfRangeException();
            }
            AdjustmentOfShooting();
            DependenceOfCooldown();
        }
    }
}
