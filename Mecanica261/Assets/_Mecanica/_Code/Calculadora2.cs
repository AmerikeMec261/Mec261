using TMPro;
using UnityEngine;

public class Calculadora2 : MonoBehaviour
{
   
        [Header("Dependencies")]
        [SerializeField] private TMP_InputField _weightInputField;
        [SerializeField] private TMP_InputField _angleInputField;
        [SerializeField] private TMP_InputField _FrictionInputField;
        [SerializeField] private TMP_InputField _gravityInputField;
        [SerializeField] private TextMeshProUGUI _answerLabel;

    public void Calculate()
    {
        _answerLabel.text = (CalculateParaleWeight() + CalculateNormalForce()).ToString("F2") + "N";

    }
    private float CalculateWeight()
    {
        return float.Parse(_weightInputField.text) * float.Parse(_gravityInputField.text);
    }
    private float CalculateParaleWeight()
    {
        return CalculateWeight() * Mathf.Sin(float.Parse(_angleInputField.text) * Mathf.Deg2Rad);
    }
    private float CalculateNormalForce()
    {
        return CalculateWeight() * Mathf.Cos(float.Parse(_angleInputField.text) * Mathf.Deg2Rad);
    }
    private float CalculateFrictionForce()
    {
        return CalculateNormalForce() * float.Parse(_FrictionInputField.text);
    }
}

  
