using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Josh;
using System;

namespace EntitySystem
{

    public interface IMoveInfluencer
    {
        float MaxDistance {get;}
        void Setup(List<Entity> neighbors);
        Vector3 GetInfluenceVector();
    }


    public class Entity : MonoBehaviour
    {
        public GameObject    instance;
        public Vector3       position;
        public Vector3       velocity;
        public EntityProfile typeInfo;
	    public int                     currentAge;
	    public int                     deathAge;
        public int                     stomachFullness;
        ISteppable[]                   stepables;
        IMoveInfluencer[]              movementInfluencers;
        public MoveInfluencer_Cohesion cohesion;

        public WorldTile    currentTile;
        public float        maxNeighborRadius = 0;
        

        void Awake() {
            currentTile = World.worldInstance.GetCellFromPosition(new Vector2(transform.position.x, transform.position.z)).GetWorldTile();
            currentTile?.RegisterEntity(this);
        
            stepables           = GetComponents<ISteppable>();
            movementInfluencers = GetComponents<IMoveInfluencer>();

            // Calculate the maximum neighbor radius from the largest influencing distance
            foreach(var moveInfluencer in movementInfluencers)
                maxNeighborRadius = Math.Max(maxNeighborRadius, moveInfluencer.MaxDistance);
        }

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

            foreach(var stepable in stepables)
                stepable.Step();
            
            var neighbors = World.worldInstance.GatherEntities(currentTile, maxNeighborRadius);
            foreach(var moveInfluencer in movementInfluencers)
                moveInfluencer.Setup(neighbors);
            
            Vector3 totalInfluence = Vector3.zero;
            foreach(var moveInfluencer in movementInfluencers)
                totalInfluence += moveInfluencer.GetInfluenceVector();

            velocity += totalInfluence * Time.deltaTime;
            position += velocity * Time.deltaTime;

            if (World.worldInstance != null)
            {
                var size = World.worldInstance.worldSize;
                if (position.x < 0)    position.x += size;
                if (position.x > size) position.x -= size;
                if (position.y < 0)    position.y += size;
                if (position.y > size) position.y -= size;
            }

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