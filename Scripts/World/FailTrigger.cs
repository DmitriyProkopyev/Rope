using UnityEngine;

public class FailTrigger : MonoBehaviour
{
    [SerializeField] private GameObject _menu;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Player _))
            _menu.SetActive(true);
    }
}
