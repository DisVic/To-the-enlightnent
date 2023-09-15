using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Player : MonoBehaviour
{
    [Header("Controls")]//менюшка для внесения данных о платформе пользователя и виде джойстика для Андроид версии
    public ControlType controlType;
    public enum ControlType { PC, Android }//список поддерживаемых платформ, отчего зависит управление
    [SerializeField] private Joystick joystick;

    [Header("Stats")]//меню для внесения характеристик главного героя
    [SerializeField] private float speed;
    [SerializeField] private int health;
    [SerializeField] private Text Health;

    [Header("Weapons")]//меню для создания массива доступных оружий и для визуализации смены оружия
    [SerializeField] private List<GameObject> unlockedWeapons;
    [SerializeField] private Image weaponIcon;   
    
    private Rigidbody2D rb;//введение переменной для физического взаимодействия с главным героем
    private Animator animator;//переменная, которая будет активировать анимации
    private Vector2 moveInput;//переменная для ввода движения пользователя
    private Vector2 moveVelocity;//переменная для сохранения вектора движения героя
    private bool face = true;//герой стоит лицом так, как изначально на спрайте

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
        rb = GetComponent<Rigidbody2D>();//необходимые манипуляции для "активирования" переменных для физ.водействия и анимаций  
        animator = GetComponent<Animator>();
    }
    private void FixedUpdate()
    {
        Movement();
    }
    private void OffJoystickOrNot()//при конфигурации для ПК джойстик отключается
    {
        if (controlType == ControlType.PC)
            OffJoystickonPC();
    }
    private void OffJoystickonPC() => joystick.gameObject.SetActive(false);//функция для отключения джойстика
    private void Flip()//функция разворота спрайта лицом в сторону направления движения героя
    {
        face = !face;
        Vector3 Scaler = transform.localScale;
        Scaler.x *= -1;
        transform.localScale = Scaler;
    }
    private void WhenFlip()//условие для отзеркаливания спрайта героя
    {
        if (!face && moveInput.x > 0 || face && moveInput.x < 0)
            Flip();//разворот персонажа при несоответствии направлении движения и лица спрайта
    }
    public void HeatlthChange(int healthValue)//функция для отслеживания очков здоровья героя и отображения их в качестве интерфейса
    {
        health += healthValue;
        Health.text = "HP: " + health;
    }
    private void WhenSwap()//условие для смены оружия
    {
        if (Input.GetKeyDown(KeyCode.Q))//смена оружия на ПК при нажатии на клавишу Q
        {
            SwapWeapons();
        }
    }
    private void ControlMovement() => moveInput = controlType == ControlType.PC ? new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"))
        : new Vector2(joystick.Horizontal, joystick.Vertical);//зависимость передвижения персонажа от платформы пользователя
    private void Movement() => rb.MovePosition(rb.position + moveVelocity * Time.fixedDeltaTime);//обновление направления движения героя
    private void Go() => moveVelocity = moveInput.normalized * speed;//привидение персонажа в движение
    private void IsRunning() => animator.SetBool("IsRunning", moveInput.x != 0);//включение/отключение анимаций для бега
    private void HeroDeath()//функция для смерти игрока
    {
        if (health <= 0)//при достижении очков здоровья "0" герой уничтожается, а игра закрывается
        {
            Destroy(gameObject);
            RestartGame();
        }
    }
    public void RestartGame() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);//функция выхода из игры
    private void SwapWeapons()//функция смены оружия
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
