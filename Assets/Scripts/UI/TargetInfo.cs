using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class TargetInfo : MonoBehaviour
    {
        [SerializeField] private TargetManager targetManager;

        [SerializeField] private Image targetImage;
        [SerializeField] private Image shieldBar;
        [SerializeField] private Image structureBar;
        [SerializeField] private TextMeshProUGUI namePlate;

        private GameObject _contentParent;    
    
        void Start()
        {
            _contentParent = transform.GetChild(0).gameObject;
            _contentParent.SetActive(false);
        
            targetManager.OnTargetLock.AddListener(OnTargetLock);
            targetManager.OnTargetUnLock.AddListener(OnTargetUnLock);
        }

        private void OnTargetLock(TargetableObject targetable)
        {
            targetable.OnHealthChanged.AddListener(OnTargetHealthChange);
            namePlate.text = targetable.name;
            OnTargetHealthChange(targetable);
        
            _contentParent.SetActive(true);
        }

        private void OnTargetUnLock(TargetableObject targetable)
        {
            targetable.OnHealthChanged.RemoveListener(OnTargetHealthChange);
        
            _contentParent.SetActive(false);
        }

        private void OnTargetHealthChange(TargetableObject targetable)
        {
            structureBar.fillAmount = targetable.GetStructurePercentage();
            shieldBar.fillAmount = targetable.GetShieldPercentage();
        }
    }
}
