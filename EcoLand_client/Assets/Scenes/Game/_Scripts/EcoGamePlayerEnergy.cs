using UnityEngine;

public class EcoGamePlayerEnergy : MonoBehaviour
{
    public float current = 0f;
    public float max = 100f;
    public float regen = 20f;

    public WorldStepper Stepper;

    public void Update()
    {
        if (!Stepper.isReady || !Stepper.enabled)
        {
            return;
        }

        current += regen * Time.deltaTime;
        current = Mathf.Clamp(current, 0f, max);
    }
}