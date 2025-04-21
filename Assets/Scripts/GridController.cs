using Injection;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GridController : MonoBehaviour
{
    [Inject] private SignalBus signalBus;
    [Inject] private GridBuilder gridBuilder;

    private void OnEnable()
    {
        // Subscribe to the signal when the object is enabled
        signalBus.Subscribe<GridMarkedSignal>(CheckIfAnyMatch);
    }
    private void OnDisable()
    {
        // Unsubscribe from the signal when the object is disabled
        signalBus.Unsubscribe<GridMarkedSignal>(CheckIfAnyMatch);
    }

    /// <summary>
    /// Checks if any cells in the grid match the marked cells.
    /// </summary>
    private void CheckIfAnyMatch(GridMarkedSignal gridMarkedSignal)
    {
        // Get all connected matching cells
        List<GridCell> matchedCells = CheckAllDirectionsForMatch(gridMarkedSignal.GridCell);

        // If there are matched cells, log them
        if (matchedCells.Count > 0)
        {
            Debug.Log($"Matched {matchedCells.Count} cells:");
            foreach (var cell in matchedCells)
            {
                Debug.Log($"Matched cell: {cell.name}");
            }
        }
        else
        {
            Debug.Log("No matches found.");
        }
    }

    /// <summary>
    /// Checks all four directions (up, down, left, right) for matches. Recursively checks connected cells.
    /// </summary>
    private List<GridCell> CheckAllDirectionsForMatch(GridCell markedCell, HashSet<GridCell> visitedCells = null)
    {
        // Initialize the visited cells set if it's null (first call)
        if (visitedCells == null)
        {
            visitedCells = new HashSet<GridCell>();
        }

        int gridSizeX = (int)gridBuilder.GridSize.x; // Number of columns
        int gridSizeY = (int)gridBuilder.GridSize.y; // Number of rows

        // If the cell is already visited or doesn't have a marker, return an empty list
        if (visitedCells.Contains(markedCell) || !markedCell.HasMarker)
        {
            return new List<GridCell>();
        }

        // Mark the cell as visited
        visitedCells.Add(markedCell);

        // Initialize the list of matched cells with the current cell
        List<GridCell> matchedCells = new List<GridCell> { markedCell };

        // Get the index of the marked cell
        int markedCellIndex = markedCell.CellIndex;

        // Check up
        if (markedCellIndex - gridSizeX >= 0) // Ensure we are not at the top edge
        {
            GridCell upCell = gridBuilder.GridCells[markedCellIndex - gridSizeX];
            matchedCells.AddRange(CheckAllDirectionsForMatch(upCell, visitedCells));
        }

        // Check down
        if (markedCellIndex + gridSizeX < gridBuilder.GridCells.Count) // Ensure we are not at the bottom edge
        {
            GridCell downCell = gridBuilder.GridCells[markedCellIndex + gridSizeX];
            matchedCells.AddRange(CheckAllDirectionsForMatch(downCell, visitedCells));
        }

        // Check left
        if (markedCellIndex % gridSizeX != 0) // Ensure we are not at the left edge
        {
            GridCell leftCell = gridBuilder.GridCells[markedCellIndex - 1];
            matchedCells.AddRange(CheckAllDirectionsForMatch(leftCell, visitedCells));
        }

        // Check right
        if ((markedCellIndex + 1) % gridSizeX != 0) // Ensure we are not at the right edge
        {
            GridCell rightCell = gridBuilder.GridCells[markedCellIndex + 1];
            matchedCells.AddRange(CheckAllDirectionsForMatch(rightCell, visitedCells));
        }

        // Return the matched cells
        return matchedCells;
    }

}
