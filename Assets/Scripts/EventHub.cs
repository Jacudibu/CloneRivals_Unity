using UnityEngine;
using UnityEngine.Events;

public static class EventHub
{
    [System.Serializable]
    public class GameObjectEvent : UnityEvent<GameObject>
    {
    }    
    
    [System.Serializable]
    public class TargetableObjectEvent : UnityEvent<TargetableObject>
    {
    }    
    
    [System.Serializable]
    public class IntEvent : UnityEvent<int>
    {
    }
    
    
    public static readonly TargetableObjectEvent OnTargetableDestroyed = new TargetableObjectEvent();
}