using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Hook[] _hooks;
    [SerializeField] private Cursor _cursor;

    private void OnEnable()
    {
        _cursor.Clicked += OnMouseClicked;
    }

    private void OnDisable()
    {
        _cursor.Clicked -= OnMouseClicked;
    }

    private void OnMouseClicked(Vector2 point)
    {
        var ropes = new List<Rope>();

        foreach (Hook hook in _hooks)
            if (hook.Hooked)
                ropes.Add(hook.Rope);

        foreach (var rope in ropes)
        {
            if (rope == null)
                return;

            if (rope.HasIntersection(point, out int index))
                rope.Tear(index);
        }
    }
}
