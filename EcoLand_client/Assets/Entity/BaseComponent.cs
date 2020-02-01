using UnityEngine;

namespace EntitySystem
{
    class BaseComponent : MonoBehaviour
    {
        protected Entity  entity;
        private void Awake()
        {
            entity = this.gameObject.GetComponent<Entity>();
        }
    }
}

