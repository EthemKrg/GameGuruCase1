using UnityEngine;
using UnityEngine.UI;

public class GridMenuController : MonoBehaviour
{
    [SerializeField] private InputField inputField;
    [SerializeField] private Button submitButton;

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
        // Perform any necessary operations with the received value
        Debug.Log($"Received value: {value}");
    }
}
