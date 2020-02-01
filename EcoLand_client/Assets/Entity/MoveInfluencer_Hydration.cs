using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EntitySystem
{
    public class MoveInfluencer_Hydration : BaseComponent, IMoveInfluencer
    {
        public float MinDistance;
        [SerializeField]
        private float maxDistance;
        public float MaxDistance { get { return maxDistance; } }

        // Entity will only attempt to get to tiles within it's livable values
        public float minHydration;
        public float maxHydration;

        private List<Josh.Cell> neighborTiles = new List<Josh.Cell>();

        public Vector3 GetInfluenceVector() {
            Vector3 maxVector = Vector3.zero;
            float currentHydration = entity.currentTile.hydration;
            bool unlivableHydration = false;

            if(currentHydration < minHydration || currentHydration > maxHydration) {
                unlivableHydration = true;
                maxVector = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized; // Panic unless you find a better option
            }

            foreach(Josh.Cell cell in neighborTiles) {
                Vector3 distance = cell.cellObject.transform.position - entity.transform.position;
                float hydration = cell.GetWorldTile().hydration;

                if(hydration > minHydration && hydration < maxHydration) {
                    Vector3 tileVector = distance.normalized * (maxDistance - distance.magnitude);
                    if(tileVector.magnitude > maxVector.magnitude) {
                        maxVector = tileVector;
                    }
                }
            }

            // If you are in an unlivible area, prioritize fleeing
            if(unlivableHydration) {
                maxVector *= 10;
            }

            return maxVector;
        }

        public void Setup(List<Entity> neighbors) {
            neighborTiles = Josh.World.worldInstance.GatherNeighborCells(new Vector2(entity.transform.position.x, entity.transform.position.z), (int)MaxDistance);
        }
    }
}

