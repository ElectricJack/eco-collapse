
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Josh;
using System;
using System.Numerics;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;


namespace EntitySystem
{

    public interface IMoveInfluencer
    {
        float MaxDistance {get;}
        void Setup(List<Entity> neighbors);
        Vector3 GetInfluenceVector();
    }

    public class Entity : MonoBehaviour, IMoveStep, IStatusStep
    {
        public GameObject    instance;
        public Vector3       position => transform.position;
        public Vector3       velocity = Vector3.zero;
        public EntityProfile typeInfo;

	    public int           currentAge;
	    public int           deathAge;
        public float         stomachFullness;
        public float         energyDecay;

        public float         fertilityReservoir = 0f;
        public float         wasteReservoir = 0f;

        public AlertBehavior deathAlert;
        
        [HideInInspector]
        public bool          isDead = false;
        
        //public ISteppable[]  stepables;
        public IStatusStep[] StatusSteps;
        public IMoveStep[]   MoveSteps;
        public IEatStep[]    EatSteps;
        
        [HideInInspector]
        IMoveInfluencer[]               movementInfluencers;

        [HideInInspector] public MoveInfluencer_Cohesion  cohesion;
        [HideInInspector] public MoveInfluencer_Alignment alignment;
        [HideInInspector] public FloraReproduction        floraReproduction;

        [HideInInspector] public Eats      eats;
        [HideInInspector] public IEdible[] Edibles;

        private Animator animator;
        
        List<Entity> _neighbors = new List<Entity>();

        private bool initialized = false;

        //public float size => transform.localScale.z;
        
        public WorldTile     currentTile;
        public float         maxNeighborRadius = 0;

        void Awake() {
            currentTile = Josh.World.worldInstance
                .GetCellFromPosition(new Vector2(transform.position.x, transform.position.z))
                .GetWorldTile();
            currentTile?.RegisterEntity(this);
        
            //stepables    = GetComponents<ISteppable>();
            StatusSteps  = GetComponents<IStatusStep>();
            MoveSteps    = GetComponents<IMoveStep>();
            EatSteps     = GetComponents<IEatStep>();

            movementInfluencers = GetComponents<IMoveInfluencer>();
            cohesion            = GetComponent<MoveInfluencer_Cohesion>();
            alignment           = GetComponent<MoveInfluencer_Alignment>();

            floraReproduction   = GetComponent<FloraReproduction>();

            eats    = GetComponent<Eats>();
            Edibles = GetComponents<IEdible>();

            animator = GetComponentInChildren<Animator>();

            // Calculate the maximum neighbor radius from the largest influencing distance
            foreach(var moveInfluencer in movementInfluencers)
                maxNeighborRadius = Math.Max(maxNeighborRadius, moveInfluencer.MaxDistance);

            initialized = true;
        }
        
        public void MoveStep()
        {
            if (!initialized)
                return;

            World.worldInstance.GatherEntities(currentTile, maxNeighborRadius, ref _neighbors);
            _neighbors.Remove(this);
            foreach(var moveInfluencer in movementInfluencers)
                moveInfluencer.Setup(_neighbors);
            
            Vector3 totalInfluence = Vector3.zero;
            foreach(var moveInfluencer in movementInfluencers)
                totalInfluence += moveInfluencer.GetInfluenceVector();

            velocity += totalInfluence * Time.deltaTime;
            velocity.y = 0;

            if (velocity.sqrMagnitude > typeInfo.speedRange.y * typeInfo.speedRange.y)
            {
                velocity = velocity.normalized * typeInfo.speedRange.y;
            }
            transform.position += velocity * Time.deltaTime;
            if (velocity.sqrMagnitude > 0.001f)
                transform.rotation = Quaternion.FromToRotation(Vector3.forward, velocity);

            if (animator != null)
            {
                animator.SetFloat("speed", velocity.magnitude);
            }

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

            var cell = World.worldInstance.GetCellFromPosition(transform.position);
            if (cell != currentTile.myCell)
                UpdateTile();
        }
        public void StatusStep()
        {
            // First check if we have died of old age
            ++currentAge;
            if(currentAge >= deathAge)
            {
                Die();
            }
            else if (currentAge > deathAge)
                return;

            float stomachDelta = Time.deltaTime * velocity.magnitude * typeInfo.energyDecay;
            stomachFullness -= stomachDelta;
            wasteReservoir += stomachDelta;

            if(UnityEngine.Random.Range(0f, 1f) > 0.999f) {
                float wasteDeposit = Mathf.Clamp(stomachDelta / 10, 0f, fertilityReservoir / 10f);
                currentTile.fertility += wasteDeposit;
                fertilityReservoir -= wasteDeposit;
            }
        }

        public virtual void Die(Entity killer = null)
        {
            isDead = true;
            if(deathAlert != null)
                AlertManager.instance.SpawnAlertForEntity(this, deathAlert);
        }

        private void UpdateTile() {
            if(Josh.World.worldInstance != null) {
                //Debug.Log($"Updating Tile for {name}");
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
            DepositFertility();
        }

        private void DepositFertility() {
            currentTile.fertility += fertilityReservoir;
            fertilityReservoir = 0f;
        }
    }
}