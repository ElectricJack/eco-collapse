using System;
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

    public class Entity : MonoBehaviour, IMoveStep
    {
        public GameObject    instance;
        public Vector3       position => transform.position;
        public Vector3       velocity = Vector3.zero;
        public EntityProfile typeInfo;

	    public int           currentAge;
	    public int           deathAge;
        public float         stomachFullness;
        public float         energyDecay;
        
        [HideInInspector]
        public bool          isDead = false;
        
        public ISteppable[]  stepables;
        public IStatusStep[] StatusSteps;
        public IMoveStep[]   MoveSteps;
        public IEatStep[]    EatSteps;
        

        IMoveInfluencer[]               movementInfluencers;
        public MoveInfluencer_Cohesion  cohesion;
        public MoveInfluencer_Alignment alignment;

        [HideInInspector]
        public Eats eats;
        [HideInInspector]
        public IEdible[] Edibles;
        
        List<Entity> _neighbors = new List<Entity>();
        

        public WorldTile     currentTile;
        public float         maxNeighborRadius = 0;

        void Awake() {
            currentTile = Josh.World.worldInstance
                .GetCellFromPosition(new Vector2(transform.position.x, transform.position.z))
                .GetWorldTile();
            currentTile?.RegisterEntity(this);
        
            stepables    = GetComponents<ISteppable>();
            StatusSteps  = GetComponents<IStatusStep>();
            MoveSteps    = GetComponents<IMoveStep>();
            EatSteps     = GetComponents<IEatStep>();

            movementInfluencers = GetComponents<IMoveInfluencer>();
            cohesion            = GetComponent<MoveInfluencer_Cohesion>();
            alignment           = GetComponent<MoveInfluencer_Alignment>();

            eats = GetComponent<Eats>();
            Edibles = GetComponents<IEdible>();

            // Calculate the maximum neighbor radius from the largest influencing distance
            foreach(var moveInfluencer in movementInfluencers)
                maxNeighborRadius = Math.Max(maxNeighborRadius, moveInfluencer.MaxDistance);
        }
        public void MoveStep()
        {
            World.worldInstance.GatherEntities(currentTile, maxNeighborRadius, ref _neighbors);
            _neighbors.Remove(this);
            foreach(var moveInfluencer in movementInfluencers)
                moveInfluencer.Setup(_neighbors);
            
            Vector3 totalInfluence = Vector3.zero;
            foreach(var moveInfluencer in movementInfluencers)
                totalInfluence += moveInfluencer.GetInfluenceVector();

            velocity += totalInfluence * Time.deltaTime;
            velocity.y = 0;
            transform.position += velocity * Time.deltaTime;

            if (World.worldInstance != null)
            {
                var size = World.worldInstance.worldSize;
                var pos = position;
                if (pos.x < 0)    pos.x += size;
                if (pos.x > size) pos.x -= size;
                if (pos.z < 0)    pos.z += size;
                if (pos.z > size) pos.z -= size;
                transform.position = pos;
            }
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
            
            // Update our active neighbors
            // @Todo ask the world what our neighbors are

            // Apply changes to our position
            // Update what tile we are in
        }

        public virtual void Die(Entity killer = null)
        {
           isDead = true;
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