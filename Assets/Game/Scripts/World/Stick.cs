using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class Stick : MonoBehaviour
{
    [SerializeField] private Transform _hookPoint;
    [SerializeField] private RopeSpawner _spawner;
    [Space]
    [SerializeField] private Material _notUsed;
    [SerializeField] private Material _inUse;
    [SerializeField] private Material _used;

    private bool _isUsed = false;

    private Renderer[] _renderers;
    private Rope _current;

    public Transform HookPoint => _hookPoint;

    private void Awake()
    {
        _renderers = GetComponentsInChildren<Renderer>();
        SetColor(_notUsed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_isUsed == false && other.TryGetComponent(out Hook hook) && hook.Hooked == false)
        {
            Attach(hook);
            StartCoroutine(Rotate());
        }
    }

    private void Attach(Hook hook)
    {
        _current = _spawner.Spawn(hook, _hookPoint.transform);
        _isUsed = true;
        SetColor(_inUse);
        _current.Tore += Unattach;
    }

    private void Unattach()
    {
        _current.Tore -= Unattach;
        SetColor(_used);
    }

    private void SetColor(Material material)
    {
        foreach (var renderer in _renderers)
            renderer.material = material;
    }

    private IEnumerator Rotate()
    {
        while(true)
        {
            _hookPoint.Rotate(0, 0, 0);
            yield return null;
        }
    }
}
