using UnityEngine;

public class NormalBullet : MonoBehaviour
{
    #region Variables

    [Header("Settings")]
    [SerializeField] private float _lifeTime = 5f;

    #endregion Variables

    #region Unity Methods

    private void Start()
    {
        Destroy(gameObject, _lifeTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }

    #endregion Unity Methods
}
