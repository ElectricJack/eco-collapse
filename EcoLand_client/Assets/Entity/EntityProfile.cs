using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace EntitySystem
{
    [CreateAssetMenu(fileName = "EntityProfile", menuName = "ScriptableObjects/EntityProfile", order = 1)]
    public class EntityProfile : ScriptableObject
    {
        // Entities are defined by behaviors on the prefab
        public GameObject prefab;

        public float        stomachSize;
                          
        public int        minLife;
	    public int        maxLife;
        public int        matureAge;
        
        [MinMax(0.1f, 10f)] public Vector2 speedRange;
        
        [MinMax(0.1f, 10f)]
        public Vector2 finalGrowthScalar;
        public float   birthScale = 0.1f;

        [Range(0.1f, 10f)]
        public float   edibleSize;
        
        [Range(0.0001f, 0.05f)]
        public float         energyDecay;
    }

}

