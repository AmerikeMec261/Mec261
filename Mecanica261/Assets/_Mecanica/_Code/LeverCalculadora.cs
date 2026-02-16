using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LeverCalculadora : MonoBehaviour
{
    [SerializeField] private TMP_InputField _knownWeightOneInputField;
    [SerializeField] private TMP_InputField _knownDistanceOneInputField;
    [SerializeField] private TMP_InputField _knownWeightTwoInputField;
    [SerializeField] private TMP_InputField _knownDistanceTwoInputField;
    [SerializeField] private TextMeshProUGUI _distanceResultText;
    [SerializeField] private TextMeshProUGUI _weightResultText;
}
