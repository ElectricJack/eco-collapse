using UnityEngine;


namespace EntitySystem
{
    public class ModelHealthVariation : BaseComponent, IStatusStep
    {
        int            healthyState = 0;
        MeshRenderer[] healthy      = null;
        MeshRenderer[] unhealthy    = null;
        MeshRenderer[] dead         = null;
        MeshRenderer   active       = null;

        protected override void Awake()
        {
            base.Awake();
            GatherAnyModels(transform.Find("dead"),      out dead);
            GatherAnyModels(transform.Find("unhealthy"), out unhealthy);
            GatherAnyModels(transform.Find("healthy"),   out healthy);
            ActivateModel(2);// Start fully healthy
        }

        private void GatherAnyModels(Transform parentNode, out MeshRenderer[] list)
        {
            list = null;
            if (parentNode != null)
            {
                list = parentNode.GetComponentsInChildren<MeshRenderer>(true);
                if(list != null)
                    foreach(var mesh in list)
                        mesh.enabled = false;
            }
        }

        public void StatusStep()
        {
            if (Time.frameCount % 30 == 0)
            {
                UpdateModel();
            }
        }
        private int CalculateHealthValue()
        {
            bool isDead        = entity.currentAge > entity.deathAge;
            bool isCloseToDead = entity.currentAge > entity.deathAge * 0.95f;
            bool unfirtile     = entity.fertilityReservoir < 0.01f || (entity.floraReproduction != null && entity.fertilityReservoir*0.5f < entity.floraReproduction.fertilityToChildren);

            if (isDead) return 0;
            if (isCloseToDead || unfirtile) return 1;
            return 2;
        }
        private void UpdateModel()
        {
            var newHealthyState = CalculateHealthValue();
            if(newHealthyState != healthyState)
            {
                healthyState = newHealthyState;
                ActivateModel(healthyState);
            }
        }
        private void ActivateModel(int healthyState)
        {
            // Find the best available model and change what is active
            if (healthyState == 0)
            {
                if (dead != null && dead.Length > 0)           
                    ActivateModel( dead[UnityEngine.Random.Range(0,Mathf.Max(dead.Length-1, 0))] );
                if (unhealthy != null && unhealthy.Length > 0) 
                    ActivateModel( unhealthy[UnityEngine.Random.Range(0,Mathf.Max(unhealthy.Length-1, 0))] );
            }
            else if(healthyState == 1)
            {
                if (unhealthy != null && unhealthy.Length > 0) 
                    ActivateModel( unhealthy[UnityEngine.Random.Range(0,Mathf.Max(unhealthy.Length-1, 0))] );
            }
            else
            {
                if (healthy != null && healthy.Length > 0) 
                    ActivateModel( healthy[UnityEngine.Random.Range(0,Mathf.Max(healthy.Length-1, 0))] );
                else
                    Debug.LogError("No healthy models hooked up!");
            }
        }
        private void ActivateModel(MeshRenderer mesh)
        {
            if (active != null)
                active.enabled = false;

            active = mesh;
            active.enabled = true;
        }
    }
}