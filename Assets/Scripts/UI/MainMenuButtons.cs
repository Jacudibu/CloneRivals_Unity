using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

[assembly:AssemblyVersion ("0.1.*")]
namespace UI
{
    public class MainMenuButtons : MonoBehaviour
    {
        public TextMeshProUGUI versionText;

        private void Start()
        {
            versionText.text = "v" + Assembly.GetExecutingAssembly().GetName().Version;
        }
    
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
}
