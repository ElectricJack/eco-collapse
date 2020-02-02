using Josh;

namespace EntitySystem
{
    public class GroundBased : BaseComponent, IMoveStep
    {
        public void MoveStep()
        {
            if (World.worldInstance != null)
            {
                var pos = transform.position;
                
                pos.y = World.worldInstance.GetCellFromPosition(pos).cellObject.transform.position.y;

                transform.position = pos;
            }
        }

        public void Step()
        {
        }
    }
}