using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertManager : MonoBehaviour
{
    public static AlertManager instance;

    public PlayerCamera playerCam;

    public static float alertNoise = 0f;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    public void SpawnAlertForEntity(EntitySystem.Entity entity, AlertBehavior behavior) {
        GameObject clone = Instantiate(behavior.gameObject, entity.transform.position, Quaternion.identity);
        clone.GetComponent<AlertBehavior>().playerCam = playerCam;
    }
}
