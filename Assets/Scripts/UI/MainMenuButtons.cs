using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButtons : MonoBehaviour
{
    public void PlayOffline()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void Settings()
    {
        SceneManager.LoadScene("Settings");
    }

    public void Quit()
    {
        Application.Quit();
    }
    
}
