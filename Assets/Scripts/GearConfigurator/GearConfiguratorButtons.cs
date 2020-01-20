using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GearConfigurator
{
    public class GearConfiguratorButtons : MonoBehaviour
    {
        public Button engineButton;
        public EngineConfiguration[] engineConfigurations;
        public int currentEngine = 0;

        public GameObject engineSelectionPopup;
        public GameObject engineSelectionButtonPrefab;

        [Serializable]
        public class IntUnityEvent : UnityEvent<int>
        {
            public int intParam;

            public IntUnityEvent(int i)
            {
                intParam = i;
            }
        }
        
        private void Start()
        {
            for (var i = 0; i < engineConfigurations.Length; i++)
            {
                var config = engineConfigurations[i];
                var button = Instantiate(engineSelectionButtonPrefab, engineSelectionPopup.transform);
                
                var currentIndex = i;
                button.GetComponent<Button>().onClick.AddListener(() => SelectEngine(currentIndex));
                button.GetComponentInChildren<Text>().text = config.engineName;
                button.GetComponent<MouseOverObject>().SetData(config.GetData());
            }

            SelectEngine(currentEngine);
        }
        
        private void SelectEngine(int index)
        {
            currentEngine = index;
            engineButton.GetComponentInChildren<Text>().text = engineConfigurations[index].engineName;
            CloseEngineDialogue();
        }

        public void OpenEngineDialogue()
        {
            engineSelectionPopup.SetActive(true);
            SetEngineButtonOnClick(CloseEngineDialogue);
        }

        public void CloseEngineDialogue()
        {
            engineSelectionPopup.SetActive(false);
            SetEngineButtonOnClick(OpenEngineDialogue);
            EventSystem.current.SetSelectedGameObject(engineButton.gameObject);
            MouseOverDrawer.ClearMouseOver();
        }

        private void SetEngineButtonOnClick(UnityAction action)
        {
            var button = engineButton.GetComponent<Button>();
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(action);
        }
    }
}