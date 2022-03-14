using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Mover : MonoBehaviour
{
    [SerializeField] private float _force;
    [SerializeField] private ParticleSystem _effect;

    private Rigidbody _rigidbody;

    private void Awake() => _rigidbody = GetComponent<Rigidbody>();

    public void PushLeft() => Push(-Vector3.right);

    public void PushRight() => Push(Vector3.right);

    private void Push(Vector3 direction)
    {
        _rigidbody.AddForce(direction.normalized * _force);
        _effect.Play();
    }
}
