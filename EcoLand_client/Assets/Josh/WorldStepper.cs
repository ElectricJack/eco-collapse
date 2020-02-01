using System;
using System.Collections.Generic;
using System.Linq;
using EntitySystem;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;
using System.Threading;

public class WorldStepper : MonoBehaviour
{
    public EntityManager EntityManager;
    
    public void Update()
    {
        foreach (var statusStep in EntityManager.entities.SelectMany(x => x.StatusSteps))
        {
            statusStep.StatusStep();;
        }
        
        foreach (var move in EntityManager.entities.SelectMany(x => x.MoveSteps))
        {
            move.MoveStep();
        }

        foreach (var eat in EntityManager.entities.SelectMany(x => x.EatSteps))
        {
            eat.EatStep();
        }

        var deads = EntityManager.entities
            .Where(x => x.isDead).AsParallel();

        foreach (var dead in deads)
        {
            EntityManager.entities.Remove(dead);
        }
    }
}

public interface IStepable
{
    
}

public interface IMoveStep : IStepable
{
    void MoveStep();
}

public interface IEatStep : IStepable
{
    void EatStep();
}

public interface IStatusStep : IStepable
{
    void StatusStep();
}