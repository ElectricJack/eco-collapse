using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace EntitySystem
{
    public enum VoreType
    {
        herbivore,
        carnivore,
        omnivore
    }
    
    public class Eats : BaseComponent, IEatStep
    {
        public    VoreType    edibleType;

        [MinMax(0.01f, 4f)] public Vector2 EdibleSizeRange;

        public void EatStep()
        {
            // TODO: Is there Something close enough to eat;
        }

        public IEdible WillEat(Entity prey)
        {
            if (prey.Edibles == null)
                return null;
            
            foreach(var edible in prey.Edibles)
            {
                switch(edibleType)
                {
                    case VoreType.carnivore:
                        if (edible.GetType() == typeof(Fauna))
                            return edible;
                        break;
                    case VoreType.herbivore:
                        if(edible.GetType() == typeof(Flora))
                            return edible;
                        break;
                    case VoreType.omnivore:
                        return edible;
                }
            }

            return null;
        }

        public void OnEat(Entity prey)
        {
            var target = WillEat(prey);
            if (target != null)
            {
                target.OnEaten(entity);
                entity.stomachFullness += target.GetFilling();
            }

        }

        public void Step()
        {
            // TODO: Do i need to step?
        }
    }

    
    
}

