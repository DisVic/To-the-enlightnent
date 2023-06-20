using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : MonoBehaviour
{
    [SerializeField]private float offset;//���������� ��� ������������� ����������� ���� ������/������� ������� ���� ������

    public GunType gunType;//����������, ������������ ��� ������ (���������/�����)
    public GameObject bullet;//�������� ������� ����
    public Transform startPoint;//��������� ������� �������� �������
    public Joystick joystick;//���������� ��������� ��� �������� �� �������

    private float Cooldown;//������� ����� ����������
    public float startCooldown;//������ �������� ����� ����������
    private float rZ;//���������� ��� ���������� ���� ������ �� �������� ����

    private Player player;//���������� ���������� �� ������� ������� "�����"
    private Vector3 difference;//���������� ����������, ���������� �� ������� ����� �������� � ����� ������

    public enum GunType { Default,Enemy}//������ ��� �������������� ������

    private Camera cam;//���������� ���� ������ ����� ��� ���������� ������ �������� ������
    private void Start()
    {
        player=GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();//������������ ���������� ������� ������� � ����� "Player"
        if (player.controlType == Player.ControlType.PC && gunType==GunType.Default)//��� ���� �� �� � �������, ��� �� ������� �� �����, �������� �����������
        {
            joystick.gameObject.SetActive(false);
        }
        cam = Camera.main;//������� ������ ����� ������ ��  ����� ������� � ���� ������
    }
    void Update()
    {
        if (!PauseMenu.isPaused)
        {
            if (gunType == GunType.Default)
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
            else if (gunType == GunType.Enemy)
            {
                difference = player.transform.position - transform.position;//������ ���������� ���������� � ������� �������� �����
                rZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
            }
            transform.rotation = Quaternion.Euler(0, 0, rZ + offset);//������������� �������� ��� ������ rotation �������� ������ � ��������
            if (Cooldown <= 0)
            {
                if (Input.GetMouseButton(0) && player.controlType == Player.ControlType.PC || gunType == GunType.Enemy)//�������� �� �� ��� ����� ��� ���������� ��� ���������� �������� ����� ����������
                {
                    Shoot();
                }
                else if (player.controlType == Player.ControlType.Android)//���� �������� ������ �� ������� �� ��������, �� ������ �������� �� ����������
                {
                    if (joystick.Vertical != 0 || joystick.Horizontal != 0)
                    {
                        Shoot();
                    }
                }
            }
            else
            {
                Cooldown -= Time.deltaTime;//������� ���������� ����������� � ������� ���� ���������
            }
        }
    }
    public void Shoot()//������� ��������
    {
        Instantiate(bullet, startPoint.position, startPoint.rotation);
        Cooldown = startCooldown;
    }
}
