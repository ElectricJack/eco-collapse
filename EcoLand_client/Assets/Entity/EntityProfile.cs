using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
                          
        public float      minSpeed;
        public float      maxSpeed;
    }

}

