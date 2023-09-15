using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;//���� �����
    public static bool isPaused;//�������� ��������� ���� �� �����
    void Start()
    {
        PauseMenuOff();
    }
    void Update()
    {
        PauseMenuOn();
    }
    private void PauseGame()//������� ������������ ����
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isPaused= true;
    }
    private void ResumeGame()//������� ����������� ����
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }
    private void PauseMenuOff() => pauseMenu.SetActive(false);//���������� ����� ���������
    private void StatusPauseMenu()//����������� ��������� ����
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
        if (Input.GetKeyDown(KeyCode.Escape))//��� ������� �� Escape ���� ��������������� ��� ��������������
        {
            StatusPauseMenu();
        }
    }
}
