using System.Collections.Generic;
using UnityEngine;

namespace EntitySystem
{
    public class MoveInfluencer_Alignment : BaseComponent, IMoveInfluencer
    {
        [SerializeField]
        private float maxDistance;
        public float MaxDistance => maxDistance;

        public int   Type        = 0;
        public float Strength    = 0;

        List<Entity> _influencers = new List<Entity>();
        Vector3      _influence   = new Vector3();

        public void Setup(List<Entity> neighbors)
        {
            _influencers.Clear();
            if (Strength == 0) return; // Don't add any work if strength is 0;

            // Filter based on radius
            float sqrMax = MaxDistance*MaxDistance;
            foreach(var neighbor in neighbors)
            {
                var toNeighbor = neighbor.position - entity.position;
                if (toNeighbor.sqrMagnitude < sqrMax && 
                    neighbor.alignment != null && 
                    neighbor.alignment.Type == Type)
                {
                    _influencers.Add(neighbor);
                }
            }
            
        }

        public Vector3 GetInfluenceVector()
        {
            _influence.Set(0,0,0);
            for(int i=0; i<_influencers.Count; ++i)
            {
                _influence += _influencers[i].velocity.normalized;
            }
            if(_influencers.Count > 0)
                _influence /= _influencers.Count;

            return _influence * Strength;
        }
    }
}