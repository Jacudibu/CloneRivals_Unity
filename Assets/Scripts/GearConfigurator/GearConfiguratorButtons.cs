using UnityEngine;
using UnityEngine.SceneManagement;

namespace GearConfigurator
{
    public class GearConfiguratorButtons : MonoBehaviour
    {
        public void Play()
        {
            SceneManager.LoadScene("SampleScene");
        }
    }
}