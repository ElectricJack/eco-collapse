using UnityEngine;

public class Grader : MonoBehaviour
{
    public Grade GetFinalGrade()
    {
        float score = 0f;

        foreach(EntitySystem.Entity entity in EntitySystem.EntityManager.instance.entities) {
            switch(entity.typeInfo.name) {
                case "Bunny":
                    score += 1.5f;
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
                    score += 30f;
                    break;
                case "BirchTree":
                    score += 2f;
                    break;
                case "flower":
                    score += 3f;
                    break;
                case "Mushroom":
                    score += 0.5f;
                    break;
                case "PineTree":
                    score += 3f;
                    break;
                case "Grass":
                    score += 0.1f;
                    break;
            }
        }

        Grade grade;

        if(score > 800) {
            grade = Grade.A;
        } else if(score > 650) {
            grade = Grade.B;
        } else if (score > 450) {
            grade = Grade.C;
        } else if (score > 200) {
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