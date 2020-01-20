using UnityEngine;

namespace GearConfigurator
{
    public class HotbarDragResult
    {
        public enum ResultType
        {
            Success,
            Failed
        }
        
        public Vector3 targetPosition;
        public ResultType resultType;
        
        public static readonly HotbarDragResult Failed = new HotbarDragResult(ResultType.Failed);

        private HotbarDragResult(ResultType resultType)
        {
            this.resultType = ResultType.Failed;
        }

        public HotbarDragResult()
        {
            
        }
    }
}