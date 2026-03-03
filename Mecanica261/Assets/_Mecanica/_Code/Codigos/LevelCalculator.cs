using UnityEngine;
using UnityEngine.UI;
using TMPro;
using TMPro.Examples;

public class LevelCalculator : MonoBehaviour
{
    [SerializeField] private TMP_InputField _knomWeigthOneInputField;
    [SerializeField] private TMP_InputField _knomDistanceOneInputField;
    [SerializeField] private TMP_InputField _knomWeigthTwoInputField;
    [SerializeField] private TMP_InputField _knomDistanceTwoInputField;
    [SerializeField] private TextMeshProUGUI _distanceResult_Text;
    [SerializeField] private TextMeshProUGUI _weigthResultText;

    public void CalculateSolution()
    {
        if(ValiteInput(_knomWeigthTwoInputField))
        {
            _distanceResult_Text.text = CalculateDistancie().ToString("F2") + "m";
        }else if(ValiteInput(_knomDistanceTwoInputField))
        {
            _weigthResultText.text = CalculateWeigtn().ToString("F2") + "kg";
        }
    }
    public void ClearAll()
    {
        _knomDistanceOneInputField.text = "";
        _knomDistanceOneInputField.text = "";
        _knomDistanceTwoInputField.text = "";
        _knomWeigthTwoInputField.text = "";
        _weigthResultText.text = "0 m";
        _distanceResult_Text.text = "0 kg";
    }

    private float CalculateDistancie()
    {
        return float.Parse(_knomDistanceOneInputField.text) * float.Parse(_knomWeigthOneInputField.text) / float.Parse(_knomWeigthTwoInputField.text);
    }

    private float CalculateWeigtn()
    {
        return float.Parse(_knomWeigthOneInputField.text) * float.Parse(_knomDistanceOneInputField.text) / float.Parse(_knomDistanceTwoInputField.text);
    }
    private bool ValiteInput(TMP_InputField InputToValide)
    {
        return InputToValide.text != "";
    }
}
