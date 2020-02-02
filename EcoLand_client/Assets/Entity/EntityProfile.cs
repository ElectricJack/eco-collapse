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

        public int        stomachSize;
        public int        hungryThreshold;
                          
        public int        minLife;
	    public int        maxLife;
        
        [MinMax(0.1f, 10f)] public Vector2 speedRange;
        
        [Range(0.1f, 4f)]
        public float size;
    }

}

