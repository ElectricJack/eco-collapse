using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertBehavior : MonoBehaviour
{
    public float timeDisplayed = 1f;

    public PlayerCamera playerCam;
    

    // Update is called once per frame
    void Update()
    {
        if (timeDisplayed <= 0f)
            Destroy(gameObject);

        transform.position = new Vector3(transform.position.x, transform.position.y + (timeDisplayed / 20), transform.position.z);
        timeDisplayed -= Time.deltaTime;

        transform.LookAt(playerCam.transform.position);
    }
}
