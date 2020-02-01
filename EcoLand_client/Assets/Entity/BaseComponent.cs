using UnityEngine;
using Josh;

namespace EntitySystem
{
    class BaseComponent : MonoBehaviour
    {
        protected Entity    entity;
        protected WorldTile tile;

        private void Awake()
        {
            entity = this.gameObject.GetComponent<Entity>();
        }
    }
}

