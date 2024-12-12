using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndPoint : MonoBehaviour
{
    [SerializeField] GameObject endMenu;
   private void OnTriggerEnter2D(Collider2D collider)
   {
        if (collider.CompareTag("endpoint") )
       {
            Debug.Log(PlayerPrefs.GetInt("UnlockedLevel"));
            endMenu.SetActive(true);
            unlockedNewLevel();
            Time.timeScale = 0;
       }
    }
   void unlockedNewLevel()
   {
    if(SceneManager.GetActiveScene().buildIndex >= PlayerPrefs.GetInt("ReachedIndex"))
    {
        //PlayerPrefs.SetInt("ReachedIndex",SceneManager.GetActiveScene().buildIndex + 1);
        PlayerPrefs.SetInt("UnlockedLevel",PlayerPrefs.GetInt("UnlockedLevel")+1);
        PlayerPrefs.Save();
        Debug.Log(PlayerPrefs.GetInt("UnlockedLevel"));
    }
   }
}
