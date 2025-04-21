using DG.Tweening;
using UnityEngine;

public class GridCell : MonoBehaviour
{
    [SerializeField] private GameObject marker;

    // Position variable for the cell
    private Vector2 position;
    public Vector2 Position => position;

    private bool hasMarker = false;
    public bool HasMarker => hasMarker;

    private float scaleUpDuration = 0.6f;
    private float scaleDownDuration = 0.5f;

    private void OnEnable()
    {
        // Set the marker to inactive when the cell is enabled
        marker.SetActive(false);
    }

    /// <summary>
    /// Initializes the GridCell with a given position.
    /// </summary>
    /// <param name="position">The position to assign to the cell.</param>
    public void Initialize(Vector2 position)
    {
        this.position = position;
    }

    public void CellClicked()
    {
        ToggleMarker();
    }

    private void ToggleMarker()
    {
        // Toggle the marker state
        hasMarker = !hasMarker;

        if (hasMarker)
        {
            marker.transform.localScale = Vector3.zero; // Reset scale to zero before showing
            marker.SetActive(true);
            marker.transform.DOScale(Vector3.one, scaleUpDuration).SetEase(Ease.OutBack);
        }
        else
        {
            marker.transform.DOScale(Vector3.zero, scaleDownDuration).SetEase(Ease.InBack)
                .OnComplete(() =>
                {
                    marker.SetActive(false);
                });
        }
    }
}
