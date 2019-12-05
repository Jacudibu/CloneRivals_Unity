using UnityEngine;

namespace Extensions
{
    public static class ComponentExtensions
    {
        public static T FindInAllParents<T>(this Component component) where T : Component
        {
            if (component.transform.parent == null)
            {
                return null;
            }
        
            var x = component.transform;
            T obj;
            do
            {
                x = x.parent;
                obj = x.GetComponent<T>();
            } while (x.parent != null && obj == null);

            return obj;
        }
    }
}