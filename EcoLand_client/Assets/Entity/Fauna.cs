using UnityEngine;

namespace EntitySystem
{
    class Fauna : BaseComponent, IEdible
    {
        [Range(0.01f, 10f)]
        public float HowFilling = 0f;
        
        public float GetFilling()
        {
            return HowFilling;
        }

        public void OnEaten(Entity predator)
        {
            entity.Die(predator);
        }
    }
}

