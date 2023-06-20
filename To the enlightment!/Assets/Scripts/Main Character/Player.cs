using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.UI;
using UnityEngine;


public class Player : MonoBehaviour
{
    [Header("Controls")]//менюшка для внесения данных о платформе пользователя и виде джойстика для Андроид версии
    public ControlType controlType;
    public Joystick joystick;

    [Header("Stats")]//меню для внесения характеристик главного героя
    public float speed;
    public int health;
    public Text Health;

    [Header("Weapons")]//меню для создания массива доступных оружий и для визуализации смены оружия
    public List<GameObject> unlockedWeapons;
    public Image weaponIcon;
    public enum ControlType{PC, Android}//список поддерживаемых платформ, отчего зависит управление
    
    private Rigidbody2D rb;//введение переменной для физического взаимодействия с главным героем
    private Animator animator;//переменная, которая будет активировать анимации
    private Vector2 moveInput;//переменная для ввода движения пользователя
    private Vector2 moveVelocity;//переменная для сохранения вектора движения героя
    private bool face = true;//герой стоит лицом так, как изначально на спрайте
    void Start()
    {
        if(controlType == ControlType.PC)//при конфигурации для ПК джойстик отключается
        {
            joystick.gameObject.SetActive(false);
        }
        rb = GetComponent<Rigidbody2D>();//необходимые манипуляции для "активирования" переменных для физ.водействия и анимаций  
        animator= GetComponent<Animator>();
    }
    void Update()
    {
        if (controlType == ControlType.PC) 
        {
            moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));//при управлении на ПК герой перемещается в 4 стороны
        }
        else if(controlType==ControlType.Android)
        {
            moveInput = new Vector2(joystick.Horizontal, joystick.Vertical);//при управлении на Андроид герой управляется джойстиков
        }
        moveVelocity=moveInput.normalized*speed;//привидение персонажа в движение
        if (moveInput.x == 0)
        {
            animator.SetBool("IsRunning", false);//анимация бега отключена, включена анимация покоя
        }
        else
        {
            animator.SetBool("IsRunning", true);//анимация бега включена, отключена анимация покоя
        }
        if(!face && moveInput.x > 0 || face && moveInput.x < 0)
        {
            Flip();//разворот персонажа при несоответствии направлении движения и лица спрайта
        }
        if (health <= 0)//при достижении очков здоровья "0" герой уничтожается, а игра закрывается
        {
            Destroy(gameObject);
            Quit();
        }
        if (Input.GetKeyDown(KeyCode.Q))//смена оружия на ПК при нажатии на клавишу Q
        {
            SwapWeapons();

        }
    }
    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + moveVelocity * Time.fixedDeltaTime);//обновление направления движения героя
    }
    private void Flip()//функция разворота спрайта лицом в сторону направления движения героя
    {
       face=!face;
        Vector3 Scaler=transform.localScale;
        Scaler.x *= -1;
        transform.localScale = Scaler;
    }
    public void HeatlthChange(int healthValue)//функция для отслеживания очков здоровья героя и отображения их в качестве интерфейса
    {
        health += healthValue;
        Health.text = "HP: " + health;
    }
    public void SwapWeapons()//функция смены оружия
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
    public void Quit()//функция выхода из игры
    {
        Application.Quit();
    }
}
