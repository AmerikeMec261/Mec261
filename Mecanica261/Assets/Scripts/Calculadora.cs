using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelCalculator : MonoBehaviour
{
    [Header("Dependencias")]
    [SerializeField] private TMP_InputField _knowWeightOneInputField;
    [SerializeField] private TMP_InputField _knowDistanceOneInputField;
    [SerializeField] private TMP_InputField _knowWeightTwoInputField;
    [SerializeField] private TMP_InputField _knowDistanceTwoInputField;
    [SerializeField] private TextMeshProUGUI _distanceResultText;
    [SerializeField] private TextMeshProUGUI _weightResultText;


    public void CalculateSolution()
    {
        if(ValidateInput(_knowWeightTwoInputField))
        {
            _distanceResultText.text = CalculateDistance().ToString("F2") + " m";
        }
        else if (ValidateInput(_knowDistanceTwoInputField))
        {
            _weightResultText.text = CalculateWeight().ToString("F2") + " N";
        }
    }

    public void ClearAll()
    {
        _knowWeightOneInputField.text = "";
        _knowDistanceOneInputField.text = "";
        _knowWeightTwoInputField.text = "";
        _knowWeightTwoInputField.text = "";
        _distanceResultText.text = "";
        _weightResultText.text = "";
    }

    private float CalculateDistance()
    {
        return float.Parse(_knowDistanceOneInputField.text) * float.Parse(_knowWeightOneInputField.text) / float.Parse(_knowWeightTwoInputField.text);
    }

    private float CalculateWeight()
    {
        return float.Parse(_knowWeightOneInputField.text) * float.Parse(_knowDistanceOneInputField.text) / float.Parse(_knowWeightTwoInputField.text);
    }

    private bool ValidateInput(TMP_InputField inputToValidate)
    {
        return inputToValidate.text != "";
    }


}