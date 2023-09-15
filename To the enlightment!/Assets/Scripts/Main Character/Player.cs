using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Player : MonoBehaviour
{
    [Header("Controls")]//������� ��� �������� ������ � ��������� ������������ � ���� ��������� ��� ������� ������
    public ControlType controlType;
    public enum ControlType { PC, Android }//������ �������������� ��������, ������ ������� ����������
    [SerializeField] private Joystick joystick;

    [Header("Stats")]//���� ��� �������� ������������� �������� �����
    [SerializeField] private float speed;
    [SerializeField] private int health;
    [SerializeField] private Text Health;

    [Header("Weapons")]//���� ��� �������� ������� ��������� ������ � ��� ������������ ����� ������
    [SerializeField] private List<GameObject> unlockedWeapons;
    [SerializeField] private Image weaponIcon;   
    
    private Rigidbody2D rb;//�������� ���������� ��� ����������� �������������� � ������� ������
    private Animator animator;//����������, ������� ����� ������������ ��������
    private Vector2 moveInput;//���������� ��� ����� �������� ������������
    private Vector2 moveVelocity;//���������� ��� ���������� ������� �������� �����
    private bool face = true;//����� ����� ����� ���, ��� ���������� �� �������

    void Start()
    {
        OffJoystickOrNot();
    }
    void Update()
    {
        ControlMovement();
        Go();
        IsRunning();
        WhenFlip();
        HeroDeath();
        WhenSwap();
    }
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();//����������� ����������� ��� "�������������" ���������� ��� ���.���������� � ��������  
        animator = GetComponent<Animator>();
    }
    private void FixedUpdate()
    {
        Movement();
    }
    private void OffJoystickOrNot()//��� ������������ ��� �� �������� �����������
    {
        if (controlType == ControlType.PC)
            OffJoystickonPC();
    }
    private void OffJoystickonPC() => joystick.gameObject.SetActive(false);//������� ��� ���������� ���������
    private void Flip()//������� ��������� ������� ����� � ������� ����������� �������� �����
    {
        face = !face;
        Vector3 Scaler = transform.localScale;
        Scaler.x *= -1;
        transform.localScale = Scaler;
    }
    private void WhenFlip()//������� ��� �������������� ������� �����
    {
        if (!face && moveInput.x > 0 || face && moveInput.x < 0)
            Flip();//�������� ��������� ��� �������������� ����������� �������� � ���� �������
    }
    public void HeatlthChange(int healthValue)//������� ��� ������������ ����� �������� ����� � ����������� �� � �������� ����������
    {
        health += healthValue;
        Health.text = "HP: " + health;
    }
    private void WhenSwap()//������� ��� ����� ������
    {
        if (Input.GetKeyDown(KeyCode.Q))//����� ������ �� �� ��� ������� �� ������� Q
        {
            SwapWeapons();
        }
    }
    private void ControlMovement() => moveInput = controlType == ControlType.PC ? new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"))
        : new Vector2(joystick.Horizontal, joystick.Vertical);//����������� ������������ ��������� �� ��������� ������������
    private void Movement() => rb.MovePosition(rb.position + moveVelocity * Time.fixedDeltaTime);//���������� ����������� �������� �����
    private void Go() => moveVelocity = moveInput.normalized * speed;//���������� ��������� � ��������
    private void IsRunning() => animator.SetBool("IsRunning", moveInput.x != 0);//���������/���������� �������� ��� ����
    private void HeroDeath()//������� ��� ������ ������
    {
        if (health <= 0)//��� ���������� ����� �������� "0" ����� ������������, � ���� �����������
        {
            Destroy(gameObject);
            RestartGame();
        }
    }
    public void RestartGame() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);//������� ������ �� ����
    private void SwapWeapons()//������� ����� ������
    {
        for (int i = 0; i < unlockedWeapons.Count; i++)
        {
            if (!unlockedWeapons[i].activeInHierarchy) continue;
            var k = i != 0 ? unlockedWeapons[i - 1] : unlockedWeapons[unlockedWeapons.Count - 1];
            unlockedWeapons[i].SetActive(false);
            k.SetActive(true);
            weaponIcon.sprite = k.GetComponent<SpriteRenderer>().sprite;
            weaponIcon.SetNativeSize();
            break;
        }
    }
}
