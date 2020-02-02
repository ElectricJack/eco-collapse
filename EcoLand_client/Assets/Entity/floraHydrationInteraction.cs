using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EntitySystem
{
    public class FloraHydrationInteraction : BaseComponent, ISteppable
    {
        public float hydrationRetention = 0.0001f;
        public float minHydration = 0f; // Minimum hydration needed for plant to grow and survive

        public void Step() {
            if (entity.currentTile.hydration > minHydration) {
                foreach(Josh.Cell cell in entity.currentTile.myCell.GetNeighbors().Values) {
                    float hydrationRetained = cell.GetWorldTile().hydration * hydrationRetention;
                    entity.currentTile.hydration += hydrationRetained;
                    cell.GetWorldTile().hydration -= hydrationRetained;
                }
            }
            else {
                entity.deathAge--;
            }
        }
    }
}
