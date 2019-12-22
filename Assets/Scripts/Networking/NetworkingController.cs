using System.Collections;
using Skills;
using UnityEngine;

namespace Networking
{
    [RequireComponent(typeof(PlayerController))]
    public class NetworkingController : MonoBehaviour
    {
        private const float UpdateInterval = 0.1f;
        private PlayerController _playerController;
    
        private void Start()
        {
            _playerController = GetComponent<PlayerController>();
            _playerController.OnSkillUsed += OnSkillUsed;

            StartCoroutine(UpdatePosition());
        }

        private void OnSkillUsed(SkillId skillId, int hotbarIndex, float cooldown)
        {
            NetworkManager.SendMessage(MessageType.SkillUsed, (int) skillId);
        }

        private IEnumerator UpdatePosition()
        {
            while (true)
            {
                NetworkManager.UpdateTransform(transform); 
                yield return new WaitForSeconds(UpdateInterval);
            }
            // ReSharper disable once IteratorNeverReturns
        }
    }
}