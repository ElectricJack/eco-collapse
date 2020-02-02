using UnityEngine;


namespace EntitySystem
{
    class AgeGrowth : BaseComponent, IStatusStep
    {
        float matureSize;
        
        protected virtual void Start()
        {
            matureSize = Random.Range(entity.typeInfo.finalGrowthScalar.x, entity.typeInfo.finalGrowthScalar.y);
        }

        public void StatusStep()
        {
            entity.transform.localScale = Vector3.one * Mathf.Clamp01((float)entity.currentAge / entity.typeInfo.matureAge);
        }
    }
}