using UnityEngine;
using UnityEngine.UI;
using TMPro;
using TMPro.Examples;

public class LevelCalculator : MonoBehaviour
{
    [SerializeField] private TMP_InputField _knowWeigthOneInputField;
    [SerializeField] private TMP_InputField _knowDistanceOneInputField;
    [SerializeField] private TMP_InputField _knowWeigthTwoInputField;
    [SerializeField] private TMP_InputField _knowDistanceTwoInputField;
    [SerializeField] private TextMeshProUGUI _distanceResult_Text;
    [SerializeField] private TextMeshProUGUI _weigthResultText;

    public void CalculateSolution()
    {
        if (ValiteInput(_knowWeigthTwoInputField))
        {
            _distanceResult_Text.text = CalculateDistancie().ToString("F2") + "m";
        }
        else if (ValiteInput(_knowDistanceTwoInputField))
        {
            _weigthResultText.text = CalculateWeigtn().ToString("F2") + "kg";
        }
    }
    public void ClearAll()
    {
        _knowDistanceOneInputField.text = "";
        _knowDistanceOneInputField.text = "";
        _knowDistanceTwoInputField.text = "";
        _knowWeigthTwoInputField.text = "";
        _weigthResultText.text = "0 m";
        _distanceResult_Text.text = "0 kg";
    }

    private float CalculateDistancie()
    {
        return float.Parse(_knowDistanceOneInputField.text) * float.Parse(_knowWeigthOneInputField.text) / float.Parse(_knowWeigthTwoInputField.text);
    }

    private float CalculateWeigtn()
    {
        return float.Parse(_knowWeigthOneInputField.text) * float.Parse(_knowDistanceOneInputField.text) / float.Parse(_knowDistanceTwoInputField.text);
    }
    private bool ValiteInput(TMP_InputField InputToValide)
    {
        return InputToValide.text != "";
    }
}