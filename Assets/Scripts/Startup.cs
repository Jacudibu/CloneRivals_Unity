using InputConfiguration;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Startup : MonoBehaviour
{
    public void Start()
    {
        KeyBindings.LoadFromDisk();
        SceneManager.LoadScene("MainMenu");
    }
}