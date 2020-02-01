using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Josh
{
    public class WorldTile
    {
        // Tile stats from 0 - 1
        public float temperature = 0f;
        public float fertility = 0f;
        public float hydration = 0f;
        public float elevation = 0f;
        public float brightness = 0f;

        public Cell myCell;

        public WorldTile(Cell cell) {
            myCell = cell;
        }

        public void RunCellularAutomata() {
            RunTemperatureSimulation();
        }

        private void RunTemperatureSimulation() {
            // Bleed x0.0001 (1/10th of a percent) into adjacent cells per tick
            float tempLossToAdjacentTile = temperature * 0.001f;
            foreach (Cell neighbor in myCell.GetNeighbors().Values) {
                neighbor.GetWorldTile().temperature += tempLossToAdjacentTile;
            }

            // Radiate temperature into space based on elevation and hydration
            float temperatureLossToSpace = (temperature * 0.001f * elevation) / (1 - Mathf.Log(hydration + 1)); // Hydration = 0 : (1 - Mathf.Log(hydration + 1)) = 1 // Hydration = 0 : (1 - Mathf.Log(hydration + 1)) = 0.7

            // Gain temperature based on brightness
            float temperatureGainDueToBrightness = (1 - temperature) * 0.001f * brightness;

            temperature += temperatureGainDueToBrightness;
            temperature -= temperatureLossToSpace;
            temperature -= tempLossToAdjacentTile * 8;
        }
    }
}
