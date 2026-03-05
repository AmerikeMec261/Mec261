using TMPro;
using UnityEngine;
using NaughtyAttributes;
using System.Runtime.CompilerServices;
using UnityEngine.UI;

public class Calc1 : MonoBehaviour
{

    [Header("Deoendencia")]
    [SerializeField] private TMP_InputField _knowWeightOneInputField;
    [SerializeField] private TMP_InputField _knowDistanceOneInputField;
    [SerializeField] private TMP_InputField _knowWeightTwoInputField;
    [SerializeField] private TMP_InputField _knowDistanceTwoInputField;
    [SerializeField] private TextMeshProUGUI _distanceResultText;
    [SerializeField] private TextMeshProUGUI _weightResultText;

    public void CalculateSolution()
    {
        if (ValidateInput(_knowWeightTwoInputField))
        {
            _distanceResultText.text = CalculatorDistance().ToString("F2") + " m";
        }
    }

    public void ClearAll()
    {
        _knowWeightOneInputField.text = "";
        _knowDistanceOneInputField.text = "";
        _knowWeightTwoInputField.text = "";
        _knowDistanceTwoInputField.text = "";
        _distanceResultText.text = "0 m";
        _weightResultText.text = "0 N";
    }

    private float CalculatorDistance()
    {
        return float.Parse(_knowDistanceOneInputField.text) * float.Parse(_knowWeightOneInputField.text) / float.Parse(_knowWeightTwoInputField.text);
    }
    private float CalculatorWeight()
    {
        return float.Parse(_knowWeightOneInputField.text) * float.Parse(_knowDistanceOneInputField.text) / float.Parse(_knowDistanceTwoInputField.text);
    }

    private bool ValidateInput(TMP_InputField inputToValidate)
    {
        return inputToValidate.text != "";
    }
}
