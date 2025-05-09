using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Zenject;
using Injection;

public class GridMenuController : MonoBehaviour
{
    [Inject] private SignalBus signalBus;
    [Inject] private GridBuilder gridBuilder;

    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private Button submitButton;
    [SerializeField] private TextMeshProUGUI infoText;

    private void OnEnable()
    {
        signalBus.Subscribe<GridRebuildedSignal>(OnGridRebuilded);
    }

    private void OnDisable()
    {
        signalBus.Unsubscribe<GridRebuildedSignal>(OnGridRebuilded);
    }

    void Start()
    {
        // Add a listener to the button to call OnSubmitButtonClicked when clicked
        submitButton.onClick.AddListener(OnSubmitButtonClicked);

    }

    // Function triggered when the button is clicked
    private void OnSubmitButtonClicked()
    {
        // Retrieve the value from the InputField and attempt to parse it as an integer
        if (int.TryParse(inputField.text, out int inputValue))
        {
            // Call a function to process the retrieved integer value
            ProcessInputValue(inputValue);
        }
        else
        {
            // Log an error message if the input is not a valid integer
            Debug.LogError("Invalid number entered!");
        }
    }

    // Function to process the integer value retrieved from the InputField
    private void ProcessInputValue(int value)
    {
        gridBuilder.CreateGrid(value, value);

        // Perform any necessary operations with the received value
        Debug.Log($"Received value: {value}");
    }

    // Function to handle the signal when the grid is rebuilt
    private void OnGridRebuilded(GridRebuildedSignal signal)
    {
        int size = (int)signal.GridSize.x;
        infoText.text = $"Grid Builded : {size}x{size}";
    }
}
