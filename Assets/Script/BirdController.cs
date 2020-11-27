using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdController : MonoBehaviour
{
   
    public float birdSpeedMin = 5.0f; 
    public float birdSpeedMax = 10.0f; 
    Vector3 offset;
   
    void Start()
    {
       
        offset = new Vector3(Vector3.left.x * Random.Range(birdSpeedMin, birdSpeedMax) * Time.deltaTime, 0, 0);
       
    }

  
    void Update()
    {
        
        transform.position += offset;
      
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Bullet"))
        {
            Destroy(this.gameObject);
            Destroy(other.gameObject);
            GameManager.shared.PlayShootParticle(transform.position);
            GameManager.shared.ScoreUp();
        }
        //Debug.Log("Bird Controller: OnTriggerEnter2D");
    }


    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Boundary") && other.gameObject.name != "Right Boundary")
        {
            Destroy(this.gameObject);
        }
        //Debug.Log("Bird Controller: OnTriggerExit2D");
    }

}
