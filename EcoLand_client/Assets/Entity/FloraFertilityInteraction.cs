using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EntitySystem
{
    public class FloraFertilityInteraction : BaseComponent, IStatusStep
    {
        public float fertilityConsumptionRate = 0.001f;
        public float minFertility = 0f; // Minimum fertility needed for plant to grow and survive

        public void StatusStep() {
            if (entity.currentTile.fertility > minFertility) {
                entity.fertilityReservoir += fertilityConsumptionRate;
                entity.currentTile.fertility -= fertilityConsumptionRate;
            } else {
                entity.deathAge -= 10;
            }
        }
    }
}
