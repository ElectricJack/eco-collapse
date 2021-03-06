﻿using UnityEngine;
using Josh;

namespace EntitySystem
{
    public class BaseComponent : MonoBehaviour
    {
        protected Entity    entity;
        protected WorldTile tile;

        protected virtual void Awake()
        {
            entity = this.gameObject.GetComponent<Entity>();
        }
    }
}

