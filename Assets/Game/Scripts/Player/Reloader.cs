using UnityEngine;
using UnityEngine.SceneManagement;

public class Reloader : MonoBehaviour
{
    [SerializeField] private string _name;

    public void Reload()
    {
        SceneManager.LoadScene(_name);
    }
}
