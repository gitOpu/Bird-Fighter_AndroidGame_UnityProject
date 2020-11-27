using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    private Rigidbody2D bulletRigidbody;
    [Range(1, 50)]
    public float bulletSpeed;
    void Start()
    {
        bulletRigidbody = gameObject.GetComponent<Rigidbody2D>();
        //Debug.Log("Bullet transform" + transform.rotation);
    }

    void Update()
    { 
        bulletRigidbody.velocity = transform.up * bulletSpeed;
        StartCoroutine(DestroyBullet(this.gameObject));
    }
    IEnumerator DestroyBullet(GameObject go)
    {
        yield return new WaitForSeconds(2.0f);
        Destroy(go);
    }
}
