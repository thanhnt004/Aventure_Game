using UnityEngine;
using UnityEngine.SceneManagement;

public class endgame : MonoBehaviour
{
    public void Next()
    {
        if(SceneManager.GetActiveScene().buildIndex == 5)
        {
            SceneManager.LoadScene("MainMenu");
        }
        else
            SceneManager.LoadScene(PlayerPrefs.GetInt("UnlockedLevel"));
        Time.timeScale = 1;
    }
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
    }
    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1;
    }
}
