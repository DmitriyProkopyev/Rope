using UnityEngine;
using UnityEngine.Events;
using Obi;
using System.Collections;

public class Rope : MonoBehaviour
{
    [SerializeField] private float _dissapearingSpeed;
    [SerializeField] private Material _template;

    private ObiRope _obiRope;

    public event UnityAction Tore;

    public void Initialize(float scale, float thickness)
    {
        _obiRope = GetComponentInChildren<ObiRope>();
        transform.localScale = new Vector3(scale, scale, scale);
        var renderer = GetComponentInChildren<ObiRopeExtrudedRenderer>();
        renderer.thicknessScale = thickness;
    }

    public void SetAttachments(Hook dynamicAttachment, Transform staticAttachment)
        => SetAttachments(dynamicAttachment.transform, staticAttachment, 0);

    public void SetAttachments(Transform staticAttachment, Hook dynamicAttachment)
        => SetAttachments(dynamicAttachment.transform, staticAttachment, 1);

    private void SetAttachments(Transform dynamicAttachment, Transform staticAttachment, int modifier)
    {
        modifier = Mathf.FloorToInt(Mathf.Clamp01(modifier));

        var attachments = GetComponentsInChildren<ObiParticleAttachment>();
        attachments[modifier].attachmentType = ObiParticleAttachment.AttachmentType.Dynamic;
        attachments[modifier].target = dynamicAttachment.transform;
        attachments[1 - modifier].attachmentType = ObiParticleAttachment.AttachmentType.Static;
        attachments[1 - modifier].target = staticAttachment;
    }

    public bool HasIntersection(Vector2 point, out int index)
    {
        const float epsilon = 80f;
        var first = _obiRope.GetElementPosition(0);
        var last = _obiRope.GetElementPosition(_obiRope.elements.Count - 1);

        bool horizontal = Mathf.Abs(first.x - last.x) > Mathf.Abs(first.y - last.y);
        bool inverted = horizontal ? first.x < last.x : first.y < last.y;
        bool movingForward = (first - point).sqrMagnitude < (last - point).sqrMagnitude;

        var searcher = new BinarySearch(_obiRope.elements.Count);
        index = -1;

        var distance = 0f;
        var oldDistance = 0f;

        while (true)
        {
            var element = _obiRope.GetElementPosition(searcher.Index);
            oldDistance = distance;
            distance = (element - point).sqrMagnitude;

            if (distance >= oldDistance)
                movingForward = !movingForward;

            if (distance <= epsilon)
            {
                index = searcher.Index;
                return true;
            }

            if (movingForward)
                if (searcher.TryMoveForward() == false)
                    return false;

            if (movingForward == false)
                if (searcher.TryMoveBackward() == false)
                    return false;
        }
    }

    public bool Tear(int index)
    {
        Tore?.Invoke();
        StartCoroutine(Die());
        return _obiRope.Tear(index);
    }

    private IEnumerator Die()
    {
        var material = new Material(_template.shader);
        transform.GetChild(0).GetComponent<MeshRenderer>().material = material;
        var color = material.color;
        var wait = new WaitForSeconds(0.005f);

        while (color.a > 0f)
        {
            color.a -= _dissapearingSpeed;
            material.color = color;
            yield return wait;
        }

        Destroy(gameObject);
    }
}
