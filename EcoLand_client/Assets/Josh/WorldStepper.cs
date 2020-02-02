using System;
using System.Collections.Generic;
using System.Linq;
using EntitySystem;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;
using System.Threading;
using Josh;
using System.Collections;


public class WorldStepper : MonoBehaviour
{
    public EntityManager EntityManager;
    
    [HideInInspector]
    public bool isReady = false;

    Unity.Profiling.ProfilerMarker moveStep = new Unity.Profiling.ProfilerMarker("MoveStep");
    Unity.Profiling.ProfilerMarker statusStep = new Unity.Profiling.ProfilerMarker("StatusStep");
    Unity.Profiling.ProfilerMarker entityStep = new Unity.Profiling.ProfilerMarker("EntityStep");

    
    public void Awake()
    {
        //StartCoroutine(InitWorld(750));
        isReady = true;
    }
    public void Update()
    {
        if (!isReady) return;
        Step();
    }
    private IEnumerator InitWorld(int steps)
    {
        for(int i=0; i<steps; ++i)
        {
            StepWorld();
            if (i % 50 == 0)
                yield return null;
        }

        isReady = true;
    }

    int stepCount = 0;
    private void Step()
    {
        ++stepCount;
        if (stepCount % 5 == 0)
        {
            StepWorld();
        }
        else
        {
            using(statusStep.Auto())
            {
                foreach (var statusStep in EntityManager.entities.SelectMany(x => x.StatusSteps))
                {
                    statusStep.StatusStep();
                }
            }
            using(moveStep.Auto())
            {
                foreach (var move in EntityManager.entities.SelectMany(x => x.MoveSteps))
                {
                    move.MoveStep();
                }
            }
            using(entityStep.Auto())
            {
                foreach (var eat in EntityManager.entities.SelectMany(x => x.EatSteps))
                {
                    eat.EatStep();
                }
            }

            var deads = EntityManager.entities
                .Where(x => x.isDead)
                .ToList();

            foreach (var dead in deads)
            {
                EntityManager.entities.Remove(dead);
                Destroy(dead.gameObject);
            }
        }
    }

    private void StepWorld()
    {
        if (World.worldInstance == null) 
            return;

        foreach (var cell in World.worldInstance.cells)
        {
            cell.Value.GetWorldTile().StatusStep();
        }
    }
}



public interface IMoveStep
{
    void MoveStep();
}

public interface IEatStep
{
    void EatStep();
}

public interface IStatusStep
{
    void StatusStep();
}