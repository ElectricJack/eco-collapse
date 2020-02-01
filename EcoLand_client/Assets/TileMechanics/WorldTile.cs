using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Josh
{
    public class WorldTile : IStepable
    {
        private const float temperatureConvectionSpeed =        0.001f;
        private const float temperatureSpaceRadiationSpeed =    0.001f;
        private const float temperatureSunWarmingSpeed =        0.001f;

        private const float waterTileFlowSpeed =                0.002f;
        private const float waterFreezingTemperature =          0.32f;
        private const float waterEvaporationSpeed =             0.001f;

        private const float rainfallSpeed =                     0.001f;
        private const float humidityTransferSpeed =             0.001f;


        // Tile stats from 0 - 1
        public float temperature = 0f;
        public float fertility = 0f;
        public float hydration = 0f;
        public float humidity = 0f;
        public float elevation = 0f;
        public float brightness = 0f;

        public Cell myCell;

        private List<EntitySystem.Entity> entities = new List<EntitySystem.Entity>();

        public WorldTile(Cell cell) {
            myCell = cell;
        }

        public void RunCellularAutomata() {
            RunTemperatureSimulation();
            RunHydrationSimulation();
            RunHumiditySimulation();
            UpdateAnimator();
        }

        private void RunTemperatureSimulation() {
            // Bleed x0.0001 (1/10th of a percent) into adjacent cells per tick
            float tempLossToAdjacentTile = temperature * temperatureConvectionSpeed;
            foreach (Cell neighbor in myCell.GetNeighbors().Values) {
                neighbor.GetWorldTile().temperature += tempLossToAdjacentTile;
            }

            // Radiate temperature into space based on elevation and hydration
            float temperatureLossToSpace = (temperature * temperatureSpaceRadiationSpeed * elevation) * (1 - Mathf.Log(hydration + 1)); // Hydration = 0 : (1 - Mathf.Log(hydration + 1)) = 1 // Hydration = 1 : (1 - Mathf.Log(hydration + 1)) = 0.7

            // Gain temperature based on brightness
            float temperatureGainDueToBrightness = (1 - temperature) * temperatureSunWarmingSpeed * brightness;

            temperature += temperatureGainDueToBrightness;
            temperature -= temperatureLossToSpace;
            temperature -= tempLossToAdjacentTile * 8;
        }

        private void RunHydrationSimulation() {
            // Water bleeds out, but only downhill, and only if it's warm enough
            float waterFlowLoss = 0f;
            if (temperature > waterFreezingTemperature) {
                float waterLossToAdjacentTile = hydration * waterTileFlowSpeed;
                foreach (Cell neighbor in myCell.GetNeighbors().Values) {
                    if (neighbor.GetWorldTile().elevation - elevation > 0) {
                        float tileFlowLoss = waterLossToAdjacentTile * (neighbor.GetWorldTile().elevation - elevation);
                        neighbor.GetWorldTile().hydration += tileFlowLoss;
                        waterFlowLoss += tileFlowLoss;
                    } else if(hydration > 1f && neighbor.GetWorldTile().elevation - (elevation + (1-hydration)) > 0) {
                        // hydration > 1 means underwater tile so add additional hydration to elevation for flow
                        float tileFlowLoss = waterLossToAdjacentTile * (neighbor.GetWorldTile().elevation - (elevation + (1 - hydration)));
                        neighbor.GetWorldTile().hydration += tileFlowLoss;
                        waterFlowLoss += tileFlowLoss;
                    }
                }
            }

            // Evaporation
            float evaporation = temperature * waterEvaporationSpeed * hydration;
            humidity += evaporation;

            hydration -= evaporation;
            hydration -= waterFlowLoss;
        }

        private void RunHumiditySimulation() {
            // Bleed x0.0001 (1/10th of a percent) into adjacent cells per tick
            float humidityLossToAdjacentTile = humidity * humidityTransferSpeed;
            foreach (Cell neighbor in myCell.GetNeighbors().Values) {
                neighbor.GetWorldTile().humidity += humidityLossToAdjacentTile;
            }

            // Rainfall => more rain when more cold
            float rainfall = humidity * (1 - temperature);
            hydration += rainfall;

            humidity -= humidityLossToAdjacentTile * 8;
            humidity -= rainfall;
        }

        private void UpdateAnimator() {
            Animator animator = myCell.cellObject.GetComponentInChildren<Animator>();
            animator.SetFloat("Wetness", hydration);
            animator.SetFloat("Fertility", fertility);
        }

        public void RegisterEntity(EntitySystem.Entity entity) {
            entities.Add(entity);
        }

        public void UnregisterEntity(EntitySystem.Entity entity) {
            entities.Remove(entity);
        }

        public List<EntitySystem.Entity> GetRegisteredEntities() {
            return entities;
        }

        public void Step()
        {
            RunCellularAutomata();
        }
    }
}
