using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(RaycastTarget))]
public class MovementPanel : MonoBehaviour
{
    [SerializeField] private UnityEvent _clicked;

    public void Click() => _clicked?.Invoke();
}
