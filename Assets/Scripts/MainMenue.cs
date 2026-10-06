using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenue : MonoBehaviour
{
    public string mainScene;

    [Header("Menu Panels")]
    [SerializeField] private GameObject mainPausePanel;
    [SerializeField] private GameObject settingsPanel;

    void Start()
    {
        if (AudioManager.instance != null)
        {
            AudioManager.instance.StartTitleMusic();
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene(mainScene);
    }

    public void OpenSettings()
    {
        mainPausePanel.SetActive(false);
        settingsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        settingsPanel.SetActive(true);
        mainPausePanel.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit The Game");
    }
}