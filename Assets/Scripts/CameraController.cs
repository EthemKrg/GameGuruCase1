using DG.Tweening;
using Injection;
using UnityEngine;
using Zenject;

public class CameraController : MonoBehaviour
{
    [Inject] private SignalBus signalBus;

    // Multiplier to calculate the camera's distance based on the grid size
    [SerializeField] private float cameraDistanceMultiplier = 1.5f;

    private void OnEnable()
    {
        signalBus.Subscribe<GridRebuildedSignal>(UpdateCameraAfterRebuild);
    }

    private void OnDisable()
    {
        signalBus.Unsubscribe<GridRebuildedSignal>(UpdateCameraAfterRebuild);
    }

    /// <summary>
    /// Updates the camera's position after the grid is rebuilt.
    /// </summary>
    /// <param name="gridRebuildedSignal">Signal containing grid details such as size, spacing, and start position.</param>
    private void UpdateCameraAfterRebuild(GridRebuildedSignal gridRebuildedSignal)
    {
        // Get grid dimensions from the signal
        int gridSizeX = (int)gridRebuildedSignal.GridSize.x; // Number of columns
        int gridSizeY = (int)gridRebuildedSignal.GridSize.y; // Number of rows

        // Get cell spacing and grid start position from the signal
        float cellSpacing = gridRebuildedSignal.CellSpacing;
        Vector3 gridStartPosition = gridRebuildedSignal.GridStartPos;

        // Calculate the center of the grid
        Vector3 gridCenter = gridStartPosition + new Vector3((gridSizeX - 1) * cellSpacing / 2, 0,
            (gridSizeY - 1) * cellSpacing / 2);

        // Initialize the camera's position at the grid center
        Vector3 camPos = gridCenter;

        // Calculate the camera's distance based on the grid size and multiplier
        float cameraDistance = Mathf.Max(gridSizeX, gridSizeY) * cellSpacing * cameraDistanceMultiplier;

        // Adjust the camera's position to move it back along the z-axis
        camPos.z -= cameraDistance;

        // Set the camera's new position
        transform.DOMove(camPos, 0.5f).SetEase(Ease.OutBack);
    }
}
