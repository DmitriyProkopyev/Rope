using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Finish : MonoBehaviour
{
    [SerializeField] private ParticleSystem[] _effects;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Player _))
            StartCoroutine(Play());
    }

    private IEnumerator Play()
    {
        var wait = new WaitForSeconds(0.4f);
        foreach (var effect in _effects)
        {
            effect.Play();
            yield return wait;
        }
    }
}
