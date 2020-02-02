using System.Collections;
using System.Collections.Generic;
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

    private void Update() {
        alertNoise -= Time.deltaTime;
        foreach(EntitySystem.Entity entity in individualChannelNoise.Keys) {
            individualChannelNoise[entity] -= Time.deltaTime;
            if(individualChannelNoise[entity] < 0) {
                individualChannelNoise.Remove(entity);
            }
        }
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
            } else {
                individualChannelNoise[entity] = 1f;
            }
            GameObject clone = Instantiate(behavior.gameObject, entity.transform.position, Quaternion.identity);
            clone.GetComponent<AlertBehavior>().playerCam = playerCam;
            alertNoise += 0.1f;
            individualChannelNoise[entity] += 1;
            
        }
        
    }
}
