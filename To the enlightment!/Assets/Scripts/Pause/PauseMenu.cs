using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;//меню паузы
    public static bool isPaused;//проверка состояния игры на паузу
    void Start()
    {
        PauseMenuOff();
    }
    void Update()
    {
        PauseMenuOn();
    }
    private void PauseGame()//функция приостановки игры
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isPaused= true;
    }
    private void ResumeGame()//функция продолжения игры
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }
    private void PauseMenuOff() => pauseMenu.SetActive(false);//изначально пауза выключена
    private void StatusPauseMenu()//зависимость состояния игры
    {
        if (isPaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }
    private void PauseMenuOn()
    {
        if (Input.GetKeyDown(KeyCode.Escape))//при нажатии на Escape игра останавливается или возобновляется
        {
            StatusPauseMenu();
        }
    }
}
