using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class SettingsButtons : MonoBehaviour
    {
        public void SaveAndExit()
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}