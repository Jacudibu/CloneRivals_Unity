using UnityEngine;
using UnityEngine.Events;

public static class EventHub
{
    [System.Serializable]
    public class GameObjectEvent : UnityEvent<GameObject>
    {
    }
    
    
    public static readonly GameObjectEvent OnTargetableDestroyed = new GameObjectEvent();
}