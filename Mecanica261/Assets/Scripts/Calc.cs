using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Calc : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private TMP_InputField _knowWeightOneInputField;
    [SerializeField] private TMP_InputField _knowDistanceOneInputField;
    [SerializeField] private TMP_InputField _knowWeightTwoInputField;
    [SerializeField] private TMP_InputField _knowDistanceTwoInputField;
    [SerializeField] private TextMeshProUGUI _distanceResultText;
    [SerializeField] private TextMeshProUGUI _weightResultText;
}
