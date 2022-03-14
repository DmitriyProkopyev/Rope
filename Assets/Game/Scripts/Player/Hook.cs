using UnityEngine;

public class Hook : MonoBehaviour
{
    [SerializeField] private GameObject _grip;

    public Rope Rope { get; private set; }

    public void GetHooked(Rope rope)
    {
        Rope = rope;
        _grip.SetActive(true);
        Rope.Tore += GetUnhooked;
    }

    private void GetUnhooked()
    {
        if (Rope != null)
            Rope.Tore -= GetUnhooked;

        Rope = null;
        _grip.SetActive(false);
    }
}
