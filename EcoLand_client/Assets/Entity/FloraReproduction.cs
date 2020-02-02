using System.Collections.Generic;
using UnityEngine;
using Josh;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;


namespace EntitySystem
{
    public class FloraReproduction : BaseComponent, IStatusStep
    {
        public float reproduceRadius;
        public float tooCloseToGrowAnyType; // Any type should usually be smaller or equal to same type
        public float tooCloseToGrowSameType;
        public float seedProbability;
        public float seedMinFertility;
        public int   foliageType;


        public void StatusStep()
        {
            if (World.worldInstance == null)
                return;

            // Make sure we are mature enough
            if (entity.currentAge < entity.typeInfo.matureAge * 0.7f)
                return;

            // If we didn't hit our seed probability, then just skip this update
            if (UnityEngine.Random.value >= seedProbability)
                return;

            // Choose random position around the plant
            var     minRadius = Mathf.Min(tooCloseToGrowAnyType, tooCloseToGrowSameType);
            var     maxRadius = Mathf.Max(tooCloseToGrowAnyType, tooCloseToGrowSameType);
            Vector3 offset    = new Vector3(1,0,0) * UnityEngine.Random.Range(minRadius, reproduceRadius);
            offset = Quaternion.AngleAxis(UnityEngine.Random.Range(0,360), Vector3.up) * offset;
            var seedLocation = transform.position + offset;

            var cell = World.worldInstance.GetCellFromPosition(seedLocation);
            var tile = cell.GetWorldTile();

            // If this location isn't fertile enough then bail
            if (tile.fertility < seedMinFertility)
                return;

            List<Entity> entities = new List<Entity>();
            World.worldInstance.GatherEntities(tile, maxRadius, ref entities);

            bool tooClose = false;
            foreach(var ent in entities)
            {
                // Only care about growing too close to the same type?
                if (ent.floraReproduction == null)
                    continue;

                var distToOther = (ent.transform.position - seedLocation).magnitude;
                if (distToOther < (foliageType == ent.floraReproduction.foliageType? tooCloseToGrowSameType : tooCloseToGrowAnyType))
                {
                    tooClose = true; break;
                }
            }

            if (!tooClose)
            {
                Debug.Log("New seedling!");
                
            }
        }

        public void SpawnChild(Vector3 seedLocation) {
            FloraFertilityInteraction floraFert = GetComponent<FloraFertilityInteraction>();
            if (floraFert != null && entity.fertilityReservoir > floraFert.fertilityConsumptionRate / 10) {
                entity.fertilityReservoir -= floraFert.fertilityConsumptionRate / 10;
                Entity newEnt = EntityManager.instance.SpawnEntity(seedLocation, entity.typeInfo);
                newEnt.fertilityReservoir += floraFert.fertilityConsumptionRate / 10;
            } else if(floraFert == null) {
                // This plant ignores fertility for some reason, spawn it, I guess
                Entity newEnt = EntityManager.instance.SpawnEntity(seedLocation, entity.typeInfo);
            }
        }
    }
}