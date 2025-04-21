using DG.Tweening;
using Injection;
using UnityEngine;
using Zenject;

public class CameraController : MonoBehaviour
{
    [Inject] private SignalBus signalBus;

    // Multiplier to calculate the camera's orthographic size based on the grid size
    [SerializeField] private float sizePadding = 1.5f; // Extra padding to ensure the grid fits in view
    [SerializeField] private float cameraYOffset = 0.5f;

    private Camera _camera;

    private void Awake()
    {
        // Cache the Camera component
        _camera = GetComponent<Camera>();

        // Ensure the camera is in orthographic mode
        if (!_camera.orthographic)
        {
            Debug.LogWarning("Camera is not in orthographic mode. Switching to orthographic.");
            _camera.orthographic = true;
        }
    }

    private void OnEnable()
    {
        signalBus.Subscribe<GridRebuildedSignal>(UpdateCameraAfterRebuild);
    }

    private void OnDisable()
    {
        signalBus.Unsubscribe<GridRebuildedSignal>(UpdateCameraAfterRebuild);
    }

    /// <summary>
    /// Updates the camera's position and orthographic size after the grid is rebuilt.
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
        Vector3 gridCenter = gridStartPosition + new Vector3((gridSizeX - 1) * cellSpacing / 2,
            (gridSizeY - 1) * cellSpacing / 2, 0);

        // Set the camera's position
        Vector3 camPos = gridCenter;
        camPos.z = transform.position.z; // Keep the camera's z position
        camPos.y += cameraYOffset; // Apply vertical offset
        transform.DOMove(camPos, 0.5f).SetEase(Ease.OutBack);

        // Calculate the orthographic size
        float gridHeight = gridSizeY * cellSpacing;
        float gridWidth = gridSizeX * cellSpacing;

        // Orthographic size is based on the height, but we also need to account for the aspect ratio
        float aspectRatio = _camera.aspect;
        float orthographicSize = Mathf.Max(gridHeight / 2, (gridWidth / 2) / aspectRatio) * sizePadding;

        // Set the camera's orthographic size
        _camera.DOOrthoSize(orthographicSize, 0.5f).SetEase(Ease.OutBack);
    }
}
