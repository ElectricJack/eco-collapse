using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EntitySystem
{
    public class FloraFertilityInteraction : BaseComponent, ISteppable
    {
        public float fertilityConsumptionRate = 0.001f;
        public float minFertility = 0f; // Minimum fertility needed for plant to grow and survive

        public void Step() {
            if (entity.currentTile.fertility > minFertility) {
                entity.fertilityReservoir += fertilityConsumptionRate;
                entity.currentTile.fertility -= fertilityConsumptionRate;
            }
        }
    }
}
