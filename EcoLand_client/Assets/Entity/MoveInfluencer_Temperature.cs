using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace EntitySystem
{
    public class MoveInfluencer_Temperature : BaseComponent, IMoveInfluencer
    {
        public float MinDistance;
        [SerializeField]
        private float maxDistance;
        public float MaxDistance { get { return maxDistance; } }
        public float Strength;

        // Entity will only attempt to get to tiles within it's livable values
        public float minTemperature;
        public float maxTemperature;

        private List<Josh.Cell> neighborTiles = new List<Josh.Cell>();



        public Vector3 GetInfluenceVector() {
            Vector3 maxVector = Vector3.zero;
            float currentTemperature = entity.currentTile.temperature;
            bool unlivableTemperature = false;

            if (currentTemperature < minTemperature || currentTemperature > maxTemperature) {
                unlivableTemperature = true;
                maxVector = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized; // Panic unless you find a better option
            }

            foreach (Josh.Cell cell in neighborTiles) {
                Vector3 distance = cell.cellObject.transform.position - entity.transform.position;
                float temperature = cell.GetWorldTile().temperature;

                if (temperature > minTemperature && temperature < maxTemperature) {
                    Vector3 tileVector = distance.normalized * (maxDistance - distance.magnitude);
                    if (tileVector.magnitude > maxVector.magnitude) {
                        maxVector = tileVector;
                    }
                }
            }

            // If you are in an unlivible area, prioritize fleeing
            if (unlivableTemperature) {
                maxVector *= 10;
            }

            return maxVector * Strength;
        }

        public void Setup(List<Entity> neighbors) {
            neighborTiles = Josh.World.worldInstance.GatherNeighborCells(new Vector2(entity.transform.position.x, entity.transform.position.z), (int)MaxDistance);
        }
    }
}
