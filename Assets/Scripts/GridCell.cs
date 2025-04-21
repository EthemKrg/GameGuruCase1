using UnityEngine;

public class GridCell : MonoBehaviour
{
    // Position variable for the cell
    private Vector2 position;
    public Vector2 Position => position;

    /// <summary>
    /// Initializes the GridCell with a given position.
    /// </summary>
    /// <param name="position">The position to assign to the cell.</param>
    public void Initialize(Vector2 position)
    {
        this.position = position;
    }
}
