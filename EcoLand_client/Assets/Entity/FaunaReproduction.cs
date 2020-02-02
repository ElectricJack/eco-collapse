using System.Collections.Generic;
using UnityEngine;
using Josh;
using Vector3 = UnityEngine.Vector3;


namespace EntitySystem
{
    public class FaunaReproduction : BaseComponent, IStatusStep
    {
        public float conceiveProbability = 0.05f;
        public float conceiveRadius      = 1.0f;
        public float requiredFertilityPerChild = 0.1f;

        public AlertBehavior reproAlert;
        public void StatusStep()
        {
            if (World.worldInstance == null)
                return;

            // Make sure we are mature enough
            if (entity.currentAge < entity.typeInfo.matureAge * 0.7f)
                return;

            // Pass probability test
            if (UnityEngine.Random.value >= conceiveProbability)
                return;

            // Be near one another AND have enough fertility (sum 50% from each > required amount per child)
            List<Entity> entities = new List<Entity>();
            World.worldInstance.GatherEntities(entity.currentTile, conceiveRadius, ref entities);
            Entity bestMate = null;
            float highestFertility = 0;
            entities.Remove(entity);
            foreach(var other in entities)
            {
                if (entity.typeInfo != other.typeInfo)
                    continue;

                var fertilityPool = other.fertilityReservoir * 0.5f + entity.fertilityReservoir * 0.5f;
                if (fertilityPool > requiredFertilityPerChild && fertilityPool > highestFertility)
                {
                    highestFertility = fertilityPool;
                    bestMate = other;
                }
            }

            if (bestMate == null)
                return;

            // Deduct 50% fertility from both
            entity.fertilityReservoir *= 0.5f;
            bestMate.fertilityReservoir *= 0.5f;

            var midPoint = (entity.position + bestMate.position) * 0.5f;
            // Spawn x children (firtilityPool / required amount per child)
            int   childCount = (int)(highestFertility / requiredFertilityPerChild);
            if(childCount > 0) {
                float fertilityPerChild = highestFertility / childCount;
                for(int i=0; i<childCount; ++i)
                {
                    Vector3 randomOffset = new Vector3(Random.value, 0, Random.value);
                    Entity child = EntityManager.instance.SpawnEntity(midPoint, entity.typeInfo);
                    AlertManager.instance.SpawnAlertForEntity(child, reproAlert, 4f);
                }
            }
            
        }
    }
}