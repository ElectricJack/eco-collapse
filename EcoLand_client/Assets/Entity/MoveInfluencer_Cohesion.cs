using System.Collections.Generic;
using UnityEngine;

namespace EntitySystem
{

    public class MoveInfluencer_Cohesion : BaseComponent, IMoveInfluencer
    {
        public float MinDistance;
        [SerializeField]
        private float maxDistance;
        public float MaxDistance  { get {return maxDistance;} }

        public int   Type          = 0;
        public float SeekStrength  = 0;
        public float RepelStrength = 0;

        List<Vector3> _toInfluencer = new List<Vector3>();
        Vector3       _influence = new Vector3();

        public void Setup(List<Entity> neighbors)
        {
            _toInfluencer.Clear();

            // Filter based on radius
            float sqrMax = MaxDistance*MaxDistance;
            foreach(var neighbor in neighbors)
            {
                var toNeighbor = neighbor.position - entity.position;
                if (toNeighbor.sqrMagnitude < sqrMax && 
                    neighbor.cohesion != null && 
                    neighbor.cohesion.Type == Type)
                {
                    //Filter based on type
                    //_influencers.Add(neighbor);
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
                var nrmDir = (_toInfluencer[i] / dist);
                if (dist < MinDistance)
                {
                    float tooClose = (MinDistance - dist) / MinDistance;
                    _influence += -nrmDir * tooClose * RepelStrength;
                }
                else
                {
                    _influence += nrmDir * (dist - MinDistance) * SeekStrength;
                }
            }
            if(_toInfluencer.Count > 0)
                _influence /= _toInfluencer.Count;

            return _influence;
        }
    }
}