using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager current;

    [SerializeField] private GameObject startPanel;
    [SerializeField] private GameObject endPanel;
    [SerializeField] private GameObject gamePanel;
    [SerializeField] private GameObject restartPanel;

    private void Awake()
    {
        current = this;
        SetPanel(true, false, false, false);
        Time.timeScale = 0;
    }

    public void StartGameButton()
    {
        SetPanel(false, false, true, false);
        Time.timeScale = 1;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void SetPanel(bool sp, bool ep, bool gp, bool rp)
    {
        startPanel.SetActive(sp);
        endPanel.SetActive(ep);
        gamePanel.SetActive(gp);
        restartPanel.SetActive(rp);
    }
}
