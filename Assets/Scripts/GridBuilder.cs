using Injection;
using UnityEngine;
using Zenject;

public class GridBuilder : MonoBehaviour
{
    [Inject] private SignalBus signalBus;

    // Reference to the prefab used for each cell in the grid
    [SerializeField] private GameObject cellPrefab;

    // The starting position of the grid
    [SerializeField] private Vector3 gridStartPosition = Vector3.zero;

    // The spacing between cells in the grid
    [SerializeField] private float cellSpacing = 1.1f;

    // The size of the grid for debugging purposes
    [SerializeField] private int gridDebugSizeX = 9;
    [SerializeField] private int gridDebugSizeY = 9;

    /// <summary>
    /// Creates a grid of cells based on the given size.
    /// </summary>
    /// <param name="gridSizeX">The number of columns in the grid.</param>
    /// <param name="gridSizeY">The number of rows in the grid.</param>
    public void CreateGrid(int gridSizeX, int gridSizeY)
    {
        // Clear any existing grid before creating a new one
        while (transform.childCount > 0)
        {
            DestroyImmediate(transform.GetChild(0).gameObject);
        }

        // Loop through rows (y-axis) and columns (x-axis) to instantiate cells
        for (int row = 0; row < gridSizeY; row++)
        {
            for (int col = 0; col < gridSizeX; col++)
            {
                // Calculate the position for the current cell
                Vector3 cellPosition = gridStartPosition + new Vector3(col * cellSpacing, row * cellSpacing, 0);

                // Instantiate the cellPrefab at the calculated position
                GameObject cell = Instantiate(cellPrefab, cellPosition, Quaternion.identity, transform);

                // Optionally, name the cell for easier debugging
                cell.name = $"Cell ({row}, {col})";
            }
        }

        // Fire a signal to notify that the grid has been rebuilt
        if (signalBus != null)
        {
            signalBus.Fire(new GridRebuildedSignal(
                new Vector2(gridSizeX, gridSizeY),
                cellSpacing,
                gridStartPosition
            ));
        }
    }

    // Debug function to create the grid from the inspector
    [ContextMenu("Create Grid Debug")]
    private void CreateGridDebug()
    {
        // Use the debug sizes to create the grid
        CreateGrid(gridDebugSizeX, gridDebugSizeY);
    }
}
