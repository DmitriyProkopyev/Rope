using System.Collections;
using UnityEngine;

public class RopeSpawner : MonoBehaviour
{
    [SerializeField] private Rope _prefab;
    [SerializeField] private ParticleSystem _effect;
    [SerializeField] private float _normalThickness;
    [SerializeField] private float _normalScale;

    private const float ScaleModifier = 2;

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
        float depth = position1.z - position2.z;
        float length = Vector3.Distance(position1, position2);

        float xzLength = Mathf.Sqrt(width * width + depth * depth);
        float zdegree = Mathf.Asin(height / length) * Mathf.Rad2Deg * Mathf.Sign(width);
        float yDegree = Mathf.Asin(depth / xzLength) * Mathf.Rad2Deg;

        float scale = length * _normalScale;
        var center = (position1 + position2) / 2;

        var rope = Instantiate(_prefab, center, Quaternion.Euler(new Vector3(0, yDegree, zdegree)));
        rope.Initialize(scale, _normalThickness / (scale * scale));
        StartCoroutine(ModifyScale(rope));

        return rope;
    }

    private IEnumerator ModifyScale(Rope rope)
    {
        const float speed = 0.25f;
        const float delay = 0.01f;

        var wait = new WaitForSeconds(delay);
        float target = rope.ObiRope.stretchingScale * ScaleModifier;

        while (rope.ObiRope.stretchingScale < target)
        {
            rope.ObiRope.stretchingScale =
                Mathf.MoveTowards(rope.ObiRope.stretchingScale, target, speed);
            yield return wait;
        }

        target = rope.ObiRope.stretchingScale / ScaleModifier;

        while (rope.ObiRope.stretchingScale > target)
        {
            rope.ObiRope.stretchingScale = 
                Mathf.MoveTowards(rope.ObiRope.stretchingScale, target, speed);
            yield return wait;
        }
    }
}
