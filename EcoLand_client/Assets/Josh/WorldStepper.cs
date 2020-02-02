using System;
using System.Collections.Generic;
using System.Linq;
using EntitySystem;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;
using System.Threading;
using Josh;

public class WorldStepper : MonoBehaviour
{
    public EntityManager EntityManager;
    

    Unity.Profiling.ProfilerMarker moveStep = new Unity.Profiling.ProfilerMarker("MoveStep");
    Unity.Profiling.ProfilerMarker statusStep = new Unity.Profiling.ProfilerMarker("StatusStep");
    Unity.Profiling.ProfilerMarker entityStep = new Unity.Profiling.ProfilerMarker("EntityStep");
    public void Update()
    {
        if (Time.frameCount % 5 == 0)
        {
            foreach (var cell in World.worldInstance.cells)
            {
                cell.Value.GetWorldTile().StatusStep();
            }
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