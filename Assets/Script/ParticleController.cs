using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleController : MonoBehaviour
{
    
    void Start()
    {
        StartCoroutine(DestroyParticleGameObject());
    }

    private IEnumerator DestroyParticleGameObject()
    {
        yield return new WaitForSeconds(GetComponent<ParticleSystem>().main.duration);
        Destroy(this.gameObject);
    }
}
