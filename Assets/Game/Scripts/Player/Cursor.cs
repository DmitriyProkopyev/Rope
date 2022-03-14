using UnityEngine;
using UnityEngine.Events;

public class Cursor : MonoBehaviour
{
    [SerializeField] private ParticleSystem _effect;
    [SerializeField] private float _distanceModifier;

    private Camera _camera;

    public RaycastHit Hit { get; private set; }

    public event UnityAction<Vector2> Clicking;
    
    private void Start() => _camera = Camera.main;

    private void Update()
    {
        Hit = ThrowRaycast();
        transform.position = CalculatePosition();

        if (Input.GetMouseButton(0))
            Clicking?.Invoke(Input.mousePosition);

        if (Input.GetMouseButtonDown(0))
        {
            _effect.Play();

            if (Hit.collider.TryGetComponent(out MovementPanel panel))
                panel.Click();
        }
        else if (Input.GetMouseButtonUp(0))
            _effect.Stop();
    }

    private RaycastHit ThrowRaycast()
    {
        var ray = _camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
            return hit;

        return new RaycastHit();
    }

    private Vector3 CalculatePosition()
    {
        var distanceCorrection = (Hit.point - _camera.transform.position) / _distanceModifier;
        return Hit.point - distanceCorrection;
    }
}
