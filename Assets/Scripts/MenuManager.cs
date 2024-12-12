using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void QuitGame()
    {
        Application.Quit();
    }
    public void NewGame()
    {
        SceneManager.LoadScene("Level "+ 1);
        PlayerPrefs.SetInt("UnlockedLevel",1);
    }
}
