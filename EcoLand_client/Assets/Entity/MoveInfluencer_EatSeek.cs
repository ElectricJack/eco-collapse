using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace EntitySystem
{
    
    // Seek things that you can eat
    // Find all things in range
    // Filter out things you can eat
    
    public class MoveInfluencer_EatSeek : BaseComponent, IMoveInfluencer
    {
        public float MinDistance;
        [SerializeField]
        private float maxDistance;
        public float MaxDistance  { get {return maxDistance;} }

        public AnimationCurve strengthForHunger = new AnimationCurve();
        
        List<Vector3> _toInfluencer = new List<Vector3>();
        List<Entity>  _influencers = new List<Entity>();
        Vector3       _influence = new Vector3();

        public void Setup(List<Entity> neighbors)
        {
            _influencers.Clear();
            _toInfluencer.Clear();
            if (strengthForHunger.Evaluate(entity.stomachFullness) <= 0) 
                return; // Don't add any work if strength is 0;

            
            // Filter based on radius
            float sqrMax = MaxDistance * MaxDistance;
            foreach(var neighbor in neighbors)
            {
                var toNeighbor = neighbor.position - entity.position;
                if (toNeighbor.sqrMagnitude < sqrMax
                    && entity.eats != null
                    && entity.eats.WillEat(neighbor) != null
                    && entity.eats.EdibleSizeRange.x < neighbor.typeInfo.edibleSize                                 
                    && entity.eats.EdibleSizeRange.y > neighbor.typeInfo.edibleSize)
                {
                    //Filter based on type
                    _influencers.Add(neighbor);
                }
            }
            
            // Filter based on Vore and Size
            var closestFloat = Mathf.Infinity;
            Entity closest = null; 
            _influencers.ForEach(x =>
            {
                var dist = x.position - entity.position;
                if (dist.sqrMagnitude < closestFloat)
                {
                    closest = x;
                }
            });
            
            if(closest == null)
                return;
            
            _toInfluencer.Add((closest.position - entity.position).normalized);
        }
        
        public Vector3 GetInfluenceVector()
        {
            _influence.Set(0,0,0);
            for(int i=0; i<_toInfluencer.Count; ++i)
            {
                _influence += _toInfluencer[i];
            }
            if(_toInfluencer.Count > 0)
                _influence /= _toInfluencer.Count;

            return _influence * strengthForHunger.Evaluate(entity.stomachFullness);
        }
    }
}