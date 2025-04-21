using Injection;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GridBuilder : MonoBehaviour
{
    [Inject] private SignalBus signalBus;

    // Reference to the prefab used for each cell in the grid
    [SerializeField] private GridCell cellPrefab;

    // The starting position of the grid
    [SerializeField] private Vector3 gridStartPosition = Vector3.zero;

    // The spacing between cells in the grid
    [SerializeField] private float cellSpacing = 1.1f;

    // Builded Grid size
    private Vector2 gridSize;
    public Vector2 GridSize => gridSize;

    [Space(20)]
    [Header("DEBUGGING \nUse ContextMenu for Rebuild.")]
    // The size of the grid for debugging purposes
    [SerializeField] private int gridDebugSizeX = 9;
    [SerializeField] private int gridDebugSizeY = 9;

    // Private variables
    private List<GridCell> gridCells = new List<GridCell>();
    public List<GridCell> GridCells => gridCells;

    private void Start()
    {
        CreateGridDebug();
    }

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

        // Clear the list of grid cells
        gridCells.Clear();

        int index = 0; // Initialize index for cell naming

        // Loop through rows (y-axis) and columns (x-axis) to instantiate cells
        for (int row = 0; row < gridSizeY; row++)
        {
            for (int col = 0; col < gridSizeX; col++)
            {
                // Calculate the position for the current cell
                Vector3 cellPosition = gridStartPosition + new Vector3(col * cellSpacing, row * cellSpacing, 0);

                // Instantiate the cellPrefab at the calculated position
                GridCell cell = Instantiate(cellPrefab, cellPosition, Quaternion.identity, transform);

                cell.Initialize(index, new Vector2(col, row), signalBus); // Initialize the cell with its position
                
                gridCells.Add(cell);

                // Optionally, name the cell for easier debugging
                cell.name = $"Cell ({row}, {col})";

                // Increment the index for the next cell
                index++;
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

        // Set the grid size
        gridSize = new Vector2(gridSizeX, gridSizeY);
    }

    // Debug function to create the grid from the inspector
    [ContextMenu("Create Grid Debug")]
    private void CreateGridDebug()
    {
        // Use the debug sizes to create the grid
        CreateGrid(gridDebugSizeX, gridDebugSizeY);
    }
}
