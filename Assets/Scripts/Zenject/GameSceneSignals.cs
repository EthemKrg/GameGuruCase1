using UnityEngine;

namespace Injection
{
    public class GridRebuildedSignal
    {
        public Vector2 GridSize { get; }
        public float CellSpacing { get; }
        public Vector3 GridStartPos { get; }

        public GridRebuildedSignal(Vector2 gridSize, float cellSpacing, Vector3 gridStartPos)
        {
            GridSize = gridSize;
            CellSpacing = cellSpacing;
            GridStartPos = gridStartPos;
        }
    }

    public class GridMarkedSignal
    {
        public GridCell GridCell { get; }
        public GridMarkedSignal(GridCell gridCell)
        {
            GridCell = gridCell;
        }
    }
}


