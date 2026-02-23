using TMPro;
using UnityEngine;

public class Triangulo : MonoBehaviour
{
    [Header("Dependencias")]
    [SerializeField] private TMP_InputField _weightInputField;
    [SerializeField] private TMP_InputField _angleInputField;
    [SerializeField] private TMP_InputField _FrictionInputField;
    [SerializeField] private TMP_InputField _gravityInputField;
    [SerializeField] private TextMeshProUGUI _answerLabel;

    public void ClearAll()
    {
        _weightInputField.text = "";
        _angleInputField.text = "";
        _FrictionInputField.text = "";
        _gravityInputField.text = "";
        _answerLabel.text = "";
    }

    public void Calculate()
    {
        _answerLabel.text = (CalculateParalWeight() + CalculateFrictionForce()).ToString("F2") + "N";
    }

    private float CalculateWeight()
    {
        return float.Parse(_weightInputField.text)*float.Parse(_gravityInputField.text);
    }

    private float CalculateParalWeight()
    {
        return CalculateWeight() * Mathf.Sin(float.Parse(_angleInputField.text) * Mathf.Deg2Rad);
    }

    private float CalculateNormalForce()
    {
        return CalculateWeight() * Mathf.Cos(float.Parse(_angleInputField.text) * Mathf.Deg2Rad);
    }

    private float CalculateFrictionForce()
    {
        return CalculateNormalForce()*float.Parse(_FrictionInputField.text);
    }
}
