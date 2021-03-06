﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EntitySystem
{
    public class FloraHydrationInteraction : BaseComponent, IStatusStep
    {
        public float hydrationRetention = 0.0001f;
        public float minHydration = 0f; // Minimum hydration needed for plant to grow and survive
        public float maxHydration = 0f;

        public void StatusStep() {
            if (entity.currentTile.hydration > minHydration && entity.currentTile.hydration < maxHydration) {
                foreach(Josh.Cell cell in entity.currentTile.myCell.GetNeighbors().Values) {
                    float hydrationRetained = cell.GetWorldTile().hydration * hydrationRetention;
                    entity.currentTile.hydration += hydrationRetained;
                    cell.GetWorldTile().hydration -= hydrationRetained;
                }
            }
            else {
                entity.deathAge -= 10;
            }
        }
    }
}
