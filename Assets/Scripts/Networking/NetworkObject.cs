using UnityEngine;

namespace Networking
{
    public class NetworkObject : MonoBehaviour
    {
        public readonly int NetworkId;

        public NetworkObject(int networkId)
        {
            NetworkId = networkId;
        }
    }
}