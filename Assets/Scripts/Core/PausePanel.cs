using UnityEngine;
using UnityEngine.SceneManagement;

public class PausePanel : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] GameObject pausePanel;
    public void Setting()
    {
        SceneManager.LoadScene("SoundSetting");
        PlayerPrefs.SetInt("priscene",SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
    }
    public void Pause()
    {
        pausePanel.SetActive(true);
        Time.timeScale = 0;
    }
    public void Home()
    {
        PlayerPrefs.SetInt("priscene",SceneManager.GetActiveScene().buildIndex);
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1;
    }
    public void Resume()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1;
    }
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
    }
}
