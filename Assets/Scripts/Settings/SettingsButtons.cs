using Settings.InputConfiguration;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Settings
{
    public class SettingsButtons : MonoBehaviour
    {
        [SerializeField] private GameObject tabParent;

        public void Start()
        {
            OpenTab("Gameplay");
        }
        
        public void SaveAndExit()
        {
            KeyBindings.SaveToDisk();
            SceneManager.LoadScene("MainMenu");
        }

        public void RevertAndExit()
        {
            KeyBindings.LoadFromDisk();
            SceneManager.LoadScene("MainMenu");
        }

        public void OpenTab(string tabName)
        {
            foreach (Transform child in tabParent.transform)
            {
                child.gameObject.SetActive(child.name.Equals(tabName));
            }
        }
    }
}