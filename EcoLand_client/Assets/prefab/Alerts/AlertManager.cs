using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AlertManager : MonoBehaviour
{
    public static AlertManager instance;

    public PlayerCamera playerCam;

    public static float alertNoise = 0f;

    private Dictionary<EntitySystem.Entity, float> individualChannelNoise = new Dictionary<EntitySystem.Entity, float>();

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    private List<NotifyData>          toAdd    = new List<NotifyData>();
    private List<EntitySystem.Entity> toRemove = new List<EntitySystem.Entity>();

    private void Update() {
        alertNoise -= Time.deltaTime;
        toRemove.Clear();
        foreach(var entity in individualChannelNoise.Keys.ToList()) {
            individualChannelNoise[entity] -= Time.deltaTime;
            if(individualChannelNoise[entity] < 0) {
                toRemove.Add(entity);
            }
        }
        foreach(var entity in toRemove)
            individualChannelNoise.Remove(entity);

        foreach(var data in toAdd)
        {
            individualChannelNoise[data.entity] = 1f;
            FinallySpawn(data.entity, data.behavior);
        }
        toAdd.Clear();
            
    }

    class NotifyData
    {
        public EntitySystem.Entity entity;
        public AlertBehavior       behavior;
    }
    public void SpawnAlertForEntity(EntitySystem.Entity entity, AlertBehavior behavior, float priority = 1f) {
        if (behavior == null)
            return;

        // Limit messages if screen is getting too noisy, but still pass along high priorty messages.
        if (Random.Range(0f, alertNoise) < 1f || priority > 3f) {
            if(individualChannelNoise.ContainsKey(entity)) {
                if(individualChannelNoise[entity] > priority) {
                    return; // channel noisier than priority
                }
                FinallySpawn( entity, behavior);
            } else {
                toAdd.Add(new NotifyData() {entity = entity, behavior = behavior });
            }
        }
        
    }

    private void FinallySpawn(EntitySystem.Entity entity, AlertBehavior behavior)
    {
        if (behavior == null || behavior.gameObject == null)
            return;

        GameObject clone = Instantiate(behavior.gameObject, entity.transform.position, Quaternion.identity);
        clone.GetComponent<AlertBehavior>().playerCam = playerCam;
        alertNoise += 0.1f;
        individualChannelNoise[entity] += 1;
    }
}
