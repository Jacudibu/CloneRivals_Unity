using System;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class SettingsButtons : MonoBehaviour
    {
        [SerializeField] private GameObject tabParent;

        public void SaveAndExit()
        {
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