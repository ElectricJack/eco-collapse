using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace EntitySystem
{

    public class MoveInfluencer_Avoid : BaseComponent, IMoveInfluencer
    {
        public float MinDistance;
        
        [SerializeField]
        private float maxDistance;
        public float MaxDistance  { get {return maxDistance;} }

        public AnimationCurve AvoidStrengthByDistance = new AnimationCurve();

        List<Vector3> _toInfluencer = new List<Vector3>();
        Vector3       _influence = new Vector3();

        public void Setup(List<Entity> neighbors)
        {
            _toInfluencer.Clear();

            // Filter based on radius
            float sqrMax = MaxDistance * MaxDistance;
            
            foreach(var neighbor in neighbors.Where(other => other.eats != null 
                                                             && other.eats.WillEat(entity, true) != null))
            {
                var toNeighbor = neighbor.position - entity.position;
                if (toNeighbor.sqrMagnitude < sqrMax && 
                    neighbor.cohesion != null)
                {
                    _toInfluencer.Add(toNeighbor);
                }
            }
        }
        
        public Vector3 GetInfluenceVector()
        {
            _influence.Set(0,0,0);
            for(int i=0; i<_toInfluencer.Count; ++i)
            {
                var dist = _toInfluencer[i].magnitude;
                var nrmDir = (-_toInfluencer[i] / dist);
                
                _influence += nrmDir * AvoidStrengthByDistance.Evaluate(dist);
                
            }
            
            if(_toInfluencer.Count > 0)
                _influence /= _toInfluencer.Count;

            return _influence;
        }
    }
}