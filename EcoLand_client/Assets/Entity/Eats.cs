using System;
using System.Collections.Generic;
using System.Linq;
using Josh;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using Random = UnityEngine.Random;

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

        [SerializeField]
        private float maxDistance;
        
        [MinMax(0.01f, 4f)] public Vector2 EdibleSizeRange;

        private List<Entity> neighbors = new List<Entity>();
        
        public void EatStep()
        {    
            World.worldInstance.GatherEntities(
                entity.currentTile, 
                maxDistance,
                ref neighbors);

            neighbors.Remove(entity);
            
            foreach (var target in neighbors.Select(x => WillEat(x)))
            {
                if(target == null)
                    continue;
                
                var myHunger = entity.stomachFullness / entity.typeInfo.stomachSize;
                var eatChance = Random.Range(0.5f, 1f);

                if (myHunger < eatChance)
                {
                    OnEat(target);
                }
            }
        }

        public Tuple<Entity, IEdible> WillEat(Entity prey, bool ignoreDist = false)
        {
            if (prey.Edibles == null)
                return null;

            if (prey.isDead)
                return null;

            if (!ignoreDist 
                && (prey.typeInfo.edibleSize < entity.eats.EdibleSizeRange.x 
                    || prey.typeInfo.edibleSize > entity.eats.EdibleSizeRange.y))
            {
                return null;
            }
            
            foreach(var edible in prey.Edibles)
            {
                switch(edibleType)
                {
                    case VoreType.carnivore:
                        if (edible.GetType() == typeof(Fauna))
                            return Tuple.Create(prey, edible);
                        break;
                    case VoreType.herbivore:
                        if(edible.GetType() == typeof(Flora))
                            return Tuple.Create(prey, edible);
                        break;
                    case VoreType.omnivore:
                        return Tuple.Create(prey, edible);
                }
            }

            return null;
        }

        public void OnEat(Tuple<Entity, IEdible> target)
        {
            if (target != null)
            {
                target.Item2.OnEaten(entity);
                entity.stomachFullness += target.Item2.GetFilling();

                entity.fertilityReservoir += target.Item1.fertilityReservoir / 2;
            }
        }
    }

    
    
}

