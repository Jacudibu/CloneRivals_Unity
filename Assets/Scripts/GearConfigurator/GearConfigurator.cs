using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GearConfigurator
{
    public class GearConfigurator : MonoBehaviour
    {
        public Button engineButton;
        public EngineConfiguration[] engineConfigurations;
        public int currentEngine = 0;

        public GameObject engineSelectionPopup;
        public GameObject engineSelectionButtonPrefab;

        private HotbarConfigurator _hotbarConfigurator;
        
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
            _hotbarConfigurator = FindObjectOfType<HotbarConfigurator>();
            
            for (var i = 0; i < engineConfigurations.Length; i++)
            {
                var config = engineConfigurations[i];
                var button = Instantiate(engineSelectionButtonPrefab, engineSelectionPopup.transform);
                
                var currentIndex = i;
                button.GetComponent<Button>().onClick.AddListener(() => SelectEngine(currentIndex));
                button.GetComponent<Image>().sprite = config.sprite;
                button.GetComponentInChildren<Text>().text = config.engineName;
                button.GetComponent<MouseOverObject>().SetData(config.GetData());
            }

            SelectEngine(currentEngine);
            foreach (var skillId in engineConfigurations[currentEngine].skills)
            {
                _hotbarConfigurator.AddSkill(skillId);
            }
        }

        private void SelectEngine(int index)
        {
            _hotbarConfigurator.SwapSkills(engineConfigurations[currentEngine].skills, engineConfigurations[index].skills);
            
            currentEngine = index;
            engineButton.GetComponentInChildren<Text>().text = engineConfigurations[currentEngine].engineName;
            engineButton.GetComponent<Image>().sprite = engineConfigurations[currentEngine].sprite;
            
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