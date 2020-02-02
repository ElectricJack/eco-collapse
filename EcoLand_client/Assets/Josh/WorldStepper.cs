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