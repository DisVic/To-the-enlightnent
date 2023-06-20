using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;//меню паузы
    public static bool isPaused;//проверка состояния игры на паузу
    void Start()
    {
        pauseMenu.SetActive(false);//изначально пауза выключена
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))//при нажатии на Escape игра останавливается или возобновляется
        {
            if(isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }
    public void PauseGame()//функция приостановки игры
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isPaused= true;
    }
    public void ResumeGame()//функция продолжения игры
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }
    public void Quit()//функция выхода из игры
    {
        Application.Quit();
    }
}
