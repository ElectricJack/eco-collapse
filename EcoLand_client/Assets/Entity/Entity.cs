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
        //    // Unregister with old tile
        //    // Register with new tile
        //}

        public virtual void OnDestroy()
        {
            // Unregister with current tile
        }
    }
}