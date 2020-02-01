using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EntitySystem
{
    public class Entity : MonoBehaviour
    {
        public GameObject   instance;
        public Vector3      position;
        public Vector3      velocity;

	    public int          currentAge;
	    public int          deathAge;

        public int          stomachFullness;

        public Josh.WorldTile currentTile;


        public virtual void Step()
        {
            // First check if we have died of old age
            ++currentAge;
            if(currentAge == deathAge)
            {
                Die();
            }
            else if (currentAge > deathAge)
                return;

            // Update our active neighbors
            // @Todo ask the world what our neighbors are

            // Apply changes to our position
            // Update what tile we are in
        }

        public virtual void Die(Entity killer = null)
        {
           
        }

        //public virtual void MoveTile(WorldTile tile)
        //{
        //    UpdateTile();
        //}

        private void UpdateTile() {
            if(Josh.World.worldInstance != null) {
                Vector2 myPosition = new Vector2(transform.position.x, transform.position.z);
                Josh.WorldTile newWorldTile = Josh.World.worldInstance.GetCellFromPosition(myPosition).GetWorldTile();
                if (newWorldTile != currentTile) {
                    currentTile.UnregisterEntity(this);
                    newWorldTile.RegisterEntity(this);
                    currentTile = newWorldTile;
                }
            }
        }

        public virtual void OnDestroy()
        {
            currentTile.UnregisterEntity(this);
        }
    }
}