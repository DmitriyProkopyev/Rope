using UnityEngine;
using UnityEngine.Events;
using Obi;
using System.Collections;

public class Rope : MonoBehaviour
{
    [SerializeField] private float _dissapearingSpeed;
    [SerializeField] private Material _template;

    public ObiRope ObiRope { get; private set; }

    public event UnityAction Tore;

    public void Initialize(float scale, float thickness)
    {
        ObiRope = GetComponentInChildren<ObiRope>();
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
        var first = ObiRope.GetElementPosition(0);
        var last = ObiRope.GetElementPosition(ObiRope.elements.Count - 1);

        bool horizontal = Mathf.Abs(first.x - last.x) > Mathf.Abs(first.y - last.y);
        bool movingForward = (first - point).sqrMagnitude < (last - point).sqrMagnitude;

        var searcher = new BinarySearch(ObiRope.elements.Count);
        index = -1;

        float distance = 0f;
        float oldDistance = 0f;

        while (true)
        {
            var element = ObiRope.GetElementPosition(searcher.Index);
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
            {
                if (searcher.TryMoveForward() == false)
                    return false;
            }

            else
            {
                if (searcher.TryMoveBackward() == false)
                    return false;
            }
        }
    }

    public bool Tear(int index)
    {
        Tore?.Invoke();
        StartCoroutine(Die());
        return ObiRope.Tear(index);
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
