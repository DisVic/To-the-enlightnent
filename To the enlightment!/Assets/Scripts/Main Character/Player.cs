using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.UI;
using UnityEngine;


public class Player : MonoBehaviour
{
    [Header("Controls")]//������� ��� �������� ������ � ��������� ������������ � ���� ��������� ��� ������� ������
    public ControlType controlType;
    public Joystick joystick;

    [Header("Stats")]//���� ��� �������� ������������� �������� �����
    public float speed;
    public int health;
    public Text Health;

    [Header("Weapons")]//���� ��� �������� ������� ��������� ������ � ��� ������������ ����� ������
    public List<GameObject> unlockedWeapons;
    public Image weaponIcon;
    public enum ControlType{PC, Android}//������ �������������� ��������, ������ ������� ����������
    
    private Rigidbody2D rb;//�������� ���������� ��� ����������� �������������� � ������� ������
    private Animator animator;//����������, ������� ����� ������������ ��������
    private Vector2 moveInput;//���������� ��� ����� �������� ������������
    private Vector2 moveVelocity;//���������� ��� ���������� ������� �������� �����
    private bool face = true;//����� ����� ����� ���, ��� ���������� �� �������
    void Start()
    {
        if(controlType == ControlType.PC)//��� ������������ ��� �� �������� �����������
        {
            joystick.gameObject.SetActive(false);
        }
        rb = GetComponent<Rigidbody2D>();//����������� ����������� ��� "�������������" ���������� ��� ���.���������� � ��������  
        animator= GetComponent<Animator>();
    }
    void Update()
    {
        if (controlType == ControlType.PC) 
        {
            moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));//��� ���������� �� �� ����� ������������ � 4 �������
        }
        else if(controlType==ControlType.Android)
        {
            moveInput = new Vector2(joystick.Horizontal, joystick.Vertical);//��� ���������� �� ������� ����� ����������� ����������
        }
        moveVelocity=moveInput.normalized*speed;//���������� ��������� � ��������
        if (moveInput.x == 0)
        {
            animator.SetBool("IsRunning", false);//�������� ���� ���������, �������� �������� �����
        }
        else
        {
            animator.SetBool("IsRunning", true);//�������� ���� ��������, ��������� �������� �����
        }
        if(!face && moveInput.x > 0 || face && moveInput.x < 0)
        {
            Flip();//�������� ��������� ��� �������������� ����������� �������� � ���� �������
        }
        if (health <= 0)//��� ���������� ����� �������� "0" ����� ������������, � ���� �����������
        {
            Destroy(gameObject);
            Quit();
        }
        if (Input.GetKeyDown(KeyCode.Q))//����� ������ �� �� ��� ������� �� ������� Q
        {
            SwapWeapons();

        }
    }
    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + moveVelocity * Time.fixedDeltaTime);//���������� ����������� �������� �����
    }
    private void Flip()//������� ��������� ������� ����� � ������� ����������� �������� �����
    {
       face=!face;
        Vector3 Scaler=transform.localScale;
        Scaler.x *= -1;
        transform.localScale = Scaler;
    }
    public void HeatlthChange(int healthValue)//������� ��� ������������ ����� �������� ����� � ����������� �� � �������� ����������
    {
        health += healthValue;
        Health.text = "HP: " + health;
    }
    public void SwapWeapons()//������� ����� ������
    {
        for(int i = 0; i < unlockedWeapons.Count; i++)
        {
            if (unlockedWeapons[i].activeInHierarchy)
            {
                unlockedWeapons[i].SetActive(false);
                if (i != 0)
                {
                    unlockedWeapons[i-1].SetActive(true);
                    weaponIcon.sprite = unlockedWeapons[i-1].GetComponent<SpriteRenderer>().sprite;
                }
                else
                {
                    unlockedWeapons[unlockedWeapons.Count-1].SetActive(true);
                    weaponIcon.sprite = unlockedWeapons[unlockedWeapons.Count - 1].GetComponent<SpriteRenderer>().sprite;
                }
                weaponIcon.SetNativeSize();
                break;
            }
        }
    }
    public void Quit()//������� ������ �� ����
    {
        Application.Quit();
    }
}
