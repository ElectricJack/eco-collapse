
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EntitySystem
{
    public class EntityManager : MonoBehaviour
    {
        public Camera          mainCamera;
        public EntityProfile[] entityTypes = new EntityProfile[10];
        public List<Entity>   entities    = new List<Entity>();

        public List<Entity> pendingDeath = new List<Entity>();

        // Update is called once per frame
        void Update()
        {
            AddEntitiesHACK();
        }

        void AddEntitiesHACK()
        {
            // Do something with the object that was hit by the raycast.
            if      (Input.GetKeyDown("1")) AddEntity(entityTypes[0]);
            else if (Input.GetKeyDown("2")) AddEntity(entityTypes[1]);
            else if (Input.GetKeyDown("3")) AddEntity(entityTypes[2]);
            else if (Input.GetKeyDown("4")) AddEntity(entityTypes[3]);
            else if (Input.GetKeyDown("5")) AddEntity(entityTypes[4]);
            else if (Input.GetKeyDown("6")) AddEntity(entityTypes[5]);
            else if (Input.GetKeyDown("7")) AddEntity(entityTypes[6]);
            else if (Input.GetKeyDown("8")) AddEntity(entityTypes[7]);
            else if (Input.GetKeyDown("9")) AddEntity(entityTypes[8]);
            else if (Input.GetKeyDown("0")) AddEntity(entityTypes[9]);
        }

        private void AddEntity(EntityProfile entityType)
        {
            if (entityType == null)
                return;

            //Debug.Log("ADD");
                
            RaycastHit hit;
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        
            if (Physics.Raycast(ray, out hit)) {
                var objectHit = hit.point;//hit.transform.position;
                
                var instance = Instantiate(entityType.prefab, objectHit, Quaternion.identity);
                instance.transform.parent = this.transform;
                var ent = instance.GetComponent<Entity>();
                ent.deathAge        = Random.Range(entityType.minLife, entityType.maxLife);
                ent.typeInfo        = entityType;
                ent.stomachFullness = entityType.stomachSize;

                entities.Add(ent);
            }
        }

    }
}

