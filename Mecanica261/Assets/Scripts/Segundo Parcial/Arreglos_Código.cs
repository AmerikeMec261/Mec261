using TMPro;
using UnityEngine;

public class Arreglos_Código : MonoBehaviour
{

    // public class Contador : MonoBehaviour
    [Header("Settings")]
    [SerializeField] private TextMeshProUGUI _chronometerTimeLable;
    [SerializeField] private float _time = 0f;
    [SerializeField] private bool _counting = true;

    void Update()
    {
        if (_counting)
        {
            _time += Time.deltaTime;
            _chronometerTimeLable.text = "Tiempo: " + _time.ToString("F2");
        }
    }

    public void DetenerCronometro()
    {
        _counting = false;
    }

    public void ReiniciarCronometro()
    {
        _time = 0f;
        _counting = true;
    }
}
