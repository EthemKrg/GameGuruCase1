using DG.Tweening;
using Injection;
using System;
using UnityEngine;
using Zenject;

public class GridCell : MonoBehaviour
{
    [SerializeField] private GameObject marker;

    // Cell index
    private int cellIndex;
    public int CellIndex => cellIndex;

    // Position variable for the cell
    private Vector2 position;
    public Vector2 Position => position;

    private bool hasMarker = false;
    public bool HasMarker => hasMarker;

    // Duration for scaling up and down the marker
    private float scaleUpDuration = 0.4f;
    private float scaleDownDuration = 0.25f;

    private SignalBus signalBus;
    private Tween scaleTween;

    private void OnEnable()
    {
        // Set the marker to inactive when the cell is enabled
        marker.SetActive(false);
    }

    /// <summary>
    /// Initializes the GridCell with a given position.
    /// </summary>
    /// <param name="position">The position to assign to the cell.</param>
    public void Initialize(int _index, Vector2 position, SignalBus _signalBus)
    {
        signalBus = _signalBus;
        cellIndex = _index;
        this.position = position;
    }

    /// <summary>
    /// Handles the click event on the cell.
    /// </summary>
    public void CellClicked()
    {
        ToggleMarker();
    }

    /// <summary>
    /// Handles the event when the cell is matched.
    /// </summary>
    public void CellMatched()
    {
        if(hasMarker)
            ToggleMarker();
    }

    /// <summary>
    /// Toggles the marker state and updates the cell's appearance.
    /// </summary>
    private void ToggleMarker()
    {
        // Toggle the marker state
        hasMarker = !hasMarker;

        if (hasMarker)
        {
            marker.SetActive(true); // Activate the marker
            ScaleTweenMarker(Vector3.zero, Vector3.one, scaleUpDuration, () =>
            {
                signalBus.Fire(new GridMarkedSignal(this));
            });

        }
        else
        {
            ScaleTweenMarker(Vector3.one, Vector3.zero, scaleDownDuration, () =>
            {
                marker.SetActive(false); // Deactivate the marker
            });
        }
    }

    /// <summary>
    /// Scales the marker using DOTween.
    /// </summary>
    private void ScaleTweenMarker(Vector3 startScale, Vector3 targetScale, float duration, Action action)
    {
        if (scaleTween != null && scaleTween.IsActive())
        {
            scaleTween.Kill();
        }

        // Set the initial scale
        marker.transform.localScale = startScale; 

        // Set the easing type
        Ease easeType = targetScale.x == 1? Ease.OutBack: Ease.InBack; 

        scaleTween = marker.transform.DOScale(targetScale, duration).SetEase(easeType)
            .OnComplete(() =>
            {
                action?.Invoke(); // Invoke the action when the tween is complete
            });
    }
}
