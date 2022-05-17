using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region Static Field
    public static GameManager current;
    #endregion

    #region Serializable Field
    [SerializeField] private GameObject startPanel;
    [SerializeField] private GameObject endPanel;
    [SerializeField] private GameObject gamePanel;
    [SerializeField] private GameObject restartPanel;
    #endregion

    #region Unity
    private void Awake()
    {
        current = this;
        SetPanel(true, false, false, false);
        Time.timeScale = 0;
    }
    #endregion

    #region Button Functions
    public void StartGameButton()
    {
        SetPanel(false, false, true, false);
        Time.timeScale = 1;
    } 

    public void NextGame()
    {
        int index = SceneManager.GetActiveScene().buildIndex + 1;
        
        if (index < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(index);
        }
        else
        {
            SceneManager.LoadScene(0);
        }

    }

    public void RestartGame()
    {
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    #endregion

    #region Public Functions
    public void SetPanel(bool sp, bool ep, bool gp, bool rp)
    {
        startPanel.SetActive(sp);
        endPanel.SetActive(ep);
        gamePanel.SetActive(gp);
        restartPanel.SetActive(rp);
    }
    #endregion
}
