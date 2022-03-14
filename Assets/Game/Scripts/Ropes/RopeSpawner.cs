using System.Collections;
using UnityEngine;

public class RopeSpawner : MonoBehaviour
{
    [SerializeField] private Rope _prefab;
    [SerializeField] private ParticleSystem _effect;
    [SerializeField] private float _normalThickness;
    [SerializeField] private float _normalScale;

    public Rope Spawn(Hook hook, Transform point)
    {
        var rope = Spawn(hook.transform, point);

        if (hook.transform.position.x < point.position.x)
            rope.SetAttachments(hook, point);
        else
            rope.SetAttachments(point, hook);

        _effect.transform.position = point.transform.position;
        _effect.Play();

        hook.GetHooked(rope);
        return rope;
    }

    private Rope Spawn(Transform point1, Transform point2)
    {
        Vector3 position1 = point1.position;
        Vector3 position2 = point2.position;

        float width = position1.x - position2.x;
        float height = position1.y - position2.y;
        float length = Vector3.Distance(position1, position2);
        float degree = Mathf.Asin(height / length) * Mathf.Rad2Deg * Mathf.Sign(width);
        float scale = length * _normalScale;
        var center = (position1 + position2) / 2;

        var rope = Instantiate(_prefab, center, Quaternion.Euler(new Vector3(0, 0, degree)));
        rope.Initialize(scale, _normalThickness / (scale * scale));

        return rope;
    }
}
