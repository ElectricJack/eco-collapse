using UnityEngine;

public class Grader : MonoBehaviour
{
    public Grade GetFinalGrade()
    {
        float score = 0f;

        foreach(EntitySystem.Entity entity in EntitySystem.EntityManager.instance.entities) {
            switch(entity.typeInfo.name) {
                case "Bunny":
                    score += 1f;
                    break;
                case "Bug":
                    score += 0.5f;
                    break;
                case "Bird":
                    score += 1.5f;
                    break;
                case "Mouse":
                    score += 0.8f;
                    break;
                case "Fox":
                    score += 10f;
                    break;
                case "BirchTree":
                    score += 0.5f;
                    break;
                case "flower":
                    score += 3f;
                    break;
                case "Mushroom":
                    score += 0.2f;
                    break;
                case "PineTree":
                    score += 2f;
                    break;
                case "Grass":
                    score += 0.01f;
                    break;
            }
        }

        Debug.LogWarning("Final score:" + score);

        Grade grade;

        if(score > 900) {
            grade = Grade.A;
        } else if(score > 750) {
            grade = Grade.B;
        } else if (score > 600) {
            grade = Grade.C;
        } else if (score > 300) {
            grade = Grade.D;
        } else {
            grade = Grade.F;
        }

        return grade;
    }

    private void Update() {
        GetFinalGrade();
    }
}