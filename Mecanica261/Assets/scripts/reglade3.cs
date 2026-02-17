using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class reglade3 : MonoBehaviour
{
    [Header("Dependencies")]

    [SerializeField] private TMP_InputField _knoweightOneInputfield;
    [SerializeField] private TMP_InputField _knowDistanceOneInputfield;
    [SerializeField] private TMP_InputField _knoweightTwoInputfield;
    [SerializeField] private TMP_InputField _knowDistanceTwoInputfield;
    [SerializeField] private TextMeshProUGUI _distanceResultText;
    [SerializeField] private TextMeshProUGUI _weightResultText;

    public void CalculateSolution()
    {
        Debug.Log("ButtonPress");
        if (ValiteInput(_knoweightTwoInputfield))
        {
            _distanceResultText.text = CalculateDistance().ToString("F2") + "m"; Debug.Log(CalculateDistance().ToString());
            
        }
        else if (ValiteInput(_knowDistanceTwoInputfield))
        {
            _weightResultText.text = CalculateWeight().ToString("F2") + "kg"; Debug.Log(CalculateWeight().ToString());
        }
    
    }

    public float CalculateDistance()
    {
        return float.Parse(_knowDistanceOneInputfield.text) * float.Parse(_knoweightOneInputfield.text)/ float.Parse(_knoweightTwoInputfield.text);
    }

    public float CalculateWeight()
    {
        return float.Parse(_knoweightOneInputfield.text) * float.Parse(_knowDistanceOneInputfield.text)/ float.Parse(_knowDistanceTwoInputfield.text);
    }

    private bool ValiteInput(TMP_InputField inputToValidate)
    {
        return inputToValidate.text != "";
    }





}
