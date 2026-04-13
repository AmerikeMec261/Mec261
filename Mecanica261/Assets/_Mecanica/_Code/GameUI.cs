using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour //El codigo no está estandarizado.
{
    public Button buyTowerButton;
    public GameObject towerPrefab;
    public Transform towerParent;
    public int towerCost = 50;

    void Start()
    {
        if (buyTowerButton == null)
        {
            Debug.LogError("buyTowerButton no esta asignado en el Inspector");
            buyTowerButton.onClick.AddListener(OnBuyTowerClicked);
        }
    }

    void OnBuyTowerClicked()
    {
        if(GameManager.Instance==null)
        {
            Debug.LogError("GameManager.Instance is null.");
            return;
        }

        if(towerPrefab == null)
        {
            Debug.LogError("towerPrefab no esta asignado en el Inspector.");
            return;
        }

        if (GameManager.Instance.SpendMoney(towerCost))
        {
            Vector3 zero = Vector3.zero;
            GameObject t= Instantiate(towerPrefab,zero,Quaternion.identity);
            t.transform.SetParent(towerParent); //No uses abreviaciones
            t.transform.localPosition = Vector3.zero;
        }
        else
        {
            Debug.Log("No hay suficiente dinero para comprar la torre.");
        }
    }
}
