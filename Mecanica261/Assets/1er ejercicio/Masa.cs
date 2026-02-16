using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Masa : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private InputField _knowWeightOneInputField_;
    [SerializeField] private InputField _knowDistanceOneInputField_;
    [SerializeField] private InputField _knowWeightTwoInputField_;
    [SerializeField] private InputField _knowDistanceTwoInputField_;
    [SerializeField] private TextMeshProUGUI _distanceResultado_;
    [SerializeField] private TextMeshProUGUI _weightResultado_;
}
