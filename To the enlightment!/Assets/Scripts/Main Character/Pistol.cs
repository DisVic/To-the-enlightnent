using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : MonoBehaviour
{
    [SerializeField] private float offset;//���������� ��� ������������� ����������� ���� ������/������� ������� ���� ������
    [SerializeField] private GunType gunType;//����������, ������������ ��� ������ (���������/�����)
    [SerializeField] private GameObject bullet;//�������� ������� ����
    [SerializeField] private Transform startPoint;//��������� ������� �������� �������
    [SerializeField] private Joystick joystick;//���������� ��������� ��� �������� �� �������

    private float Cooldown;//������� ����� ����������
    [SerializeField] private float startCooldown;//������ �������� ����� ����������
    private float rZ;//���������� ��� ���������� ���� ������ �� �������� ����

    private Player player;//���������� ���������� �� ������� ������� "�����"
    private Vector3 difference;//���������� ����������, ���������� �� ������� ����� �������� � ����� ������

    public enum GunType { Default,Enemy}//������ ��� �������������� ������

    private Camera cam;//���������� ���� ������ ����� ��� ���������� ������ �������� ������

    private void Start()
    {
        PlayerIsPlayer();
        OffJoystickOrNot();
        �amIsCamera();
    }
    void Update()
    {
        GameNotOnPause();
    }
    public void Shoot()//������� ��������
    {
        Instantiate(bullet, startPoint.position, startPoint.rotation);
        Cooldown = startCooldown;
    }
    private void �amIsCamera() => cam = Camera.main;//������� ������ ����� ������ ��  ����� ������� � ���� ������
    private void OffJoystick() => joystick.gameObject.SetActive(false);//���������� ��������� ��� ��������
    private void OffJoystickOrNot()//��� ���� �� �� � �������, ��� �� ������� �� �����, �������� �����������
    {
        if (player.controlType == Player.ControlType.PC && gunType == GunType.Default)
            OffJoystick();
    }
    private void PlayerIsPlayer() => player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();//������������ ���������� ������� ������� � ����� "Player"
    private void DirectionOfHeroGun()//����������� ������
    {
        if (player.controlType == Player.ControlType.PC)
        {
            difference = cam.ScreenToWorldPoint(Input.mousePosition) - transform.position;//���������� ���� ��������� �� �������� �� �� ��� �������, ��� �����, ���������� �������, �������� ������
            rZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        }
        else if (player.controlType == Player.ControlType.Android && Mathf.Abs(joystick.Horizontal) > 0.3f || Mathf.Abs(joystick.Vertical) > 0.3f)
        {
            rZ = Mathf.Atan2(joystick.Vertical, joystick.Horizontal) * Mathf.Rad2Deg;//�� ������� ������ �������� ����, ���� ��������� ��������
        }
    }
    private void DirectionEnemyGun()//������ ���������� ���������� � ������� �������� �����
    {
        difference = player.transform.position - transform.position;
        rZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
    }
    private void AdjustmentOfShooting() => transform.rotation = Quaternion.Euler(0, 0, rZ + offset);//������������� �������� ��� ������ rotation �������� ������ � ��������
    private void CheckTimeOnCooldown() => Cooldown -= Time.deltaTime;//������� ���������� ����������� � ������� ���� ���������
    private void ShootingOnAndroid()//�������� ��� ���������� ��������� �� Android
    {
        if (joystick.Vertical != 0 || joystick.Horizontal != 0)
        {
            Shoot();
        }
    }
    private void DifferentShooting()//�������� �� �� ��� ����� ��� ���������� ��� ���������� �������� ����� ����������
    {
        if (Cooldown <= 0 && Input.GetMouseButton(0) && player.controlType == Player.ControlType.PC || gunType == GunType.Enemy)
        {
            Shoot();
        }
        else if (player.controlType == Player.ControlType.Android)//���� �������� ������ �� ������� �� ��������, �� ������ �������� �� ����������
        {
            ShootingOnAndroid();
        }
    }
    private void DependenceOfCooldown()//���������� �������� �� ����������� ��������
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
    private void GameNotOnPause()//���� �� �����
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
