using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Hook[] _hooks;
    [SerializeField] private Cursor _cursor;

    private void OnEnable()
    {
        _cursor.Clicking += OnMouseClicking;
    }

    private void OnDisable()
    {
        _cursor.Clicking -= OnMouseClicking;
    }

    private void OnMouseClicking(Vector2 point)
    {
        foreach (var rope in FindObjectsOfType<Rope>())
        {
            if (rope == null)
                return;

            if (rope.HasIntersection(point, out int index))
                rope.Tear(index);
        }
    }
}
