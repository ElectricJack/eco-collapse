using Josh;
using UnityEngine;

namespace EntitySystem
{
    public class GroundBased : BaseComponent, IMoveStep
    {
        public void MoveStep()
        {
            if (World.worldInstance != null)
            {
                var pos = transform.position;
                var cell = World.worldInstance.GetCellFromPosition(pos);
                var nw = cell.GetNeighbor(Direction.NorthWest).cellObject.transform.position;
                var sw = cell.GetNeighbor(Direction.SouthWest).cellObject.transform.position;
                var ne = cell.GetNeighbor(Direction.NorthEast).cellObject.transform.position;
                var se = cell.GetNeighbor(Direction.SouthEast).cellObject.transform.position;
                
                float dx0 = (pos.x - nw.x) / (ne.x - nw.x); // Along top
                float dx1 = (pos.x - sw.x) / (se.x - sw.x); // Along bottom

                float yx0 = Mathf.Lerp(nw.y, ne.y, dx0);
                float yx1 = Mathf.Lerp(sw.y, se.y, dx1);

                float dz = (pos.z - sw.z) / (nw.z - sw.z);

                pos.y =  Mathf.Lerp(yx1, yx0, dz) + 0.5f;

                transform.position = pos;
            }
        }

        public void Step()
        {
        }
    }
}