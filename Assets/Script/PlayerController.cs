using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using System.Reflection;

public class PlayerController : MonoBehaviour
{
    
    public GameObject bullet;
    public GameObject[] playerPrefab;
    public int currentPlayerIndex = 0;
    private Transform turret;

    GameObject currentPlayer;
    private Vector3 userActionInPlace;
    private void Start()
    {
      InstantiatePlayer();
    }
    void Update()
    {


        //if (Input.GetButtonDown("Fire1") || Input.touchCount > 0)
        //{
        //    if (Input.touchCount > 0)
        //    {
        //        userActionInPlace = Camera.main.WorldToScreenPoint(Input.GetTouch(0).position);
        //    }
        //    else
        //    { 
        //        userActionInPlace = Camera.main.WorldToScreenPoint(Input.mousePosition);                
        //    }
        //    RotaedSprite(userActionInPlace); 
        //}
        //}

        if (Input.GetMouseButtonDown(0))
        {
            userActionInPlace = Camera.main.WorldToScreenPoint(Input.mousePosition);
            RotaedSprite(userActionInPlace);
        }

        //if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began)
        //{
        //    userActionInPlace = Camera.main.WorldToScreenPoint(Input.GetTouch(0).position);
        //    RotaedSprite(userActionInPlace);
        //}
    }

    void InstantiatePlayer()
    {
        if (currentPlayer != null) { Destroy(currentPlayer); }
        currentPlayer = Instantiate(playerPrefab[currentPlayerIndex], gameObject.transform.position, gameObject.transform.rotation) as GameObject;
        currentPlayer.transform.parent = gameObject.transform;

        if (userActionInPlace.magnitude > 0)
        {
          RotaedSprite(userActionInPlace);
        }

    }
    void RotaedSprite(Vector3 userActionInPlace)
    {
        turret = currentPlayer.transform.Find("Turret");
        Vector3 turretPosition = Camera.main.WorldToScreenPoint(turret.position);
        Vector3 direction = userActionInPlace - turretPosition;
        float angle = (Mathf.Atan2(direction.y, direction.x) ) * Mathf.Rad2Deg;
         currentPlayer.transform.rotation = Quaternion.AngleAxis(angle % 15, Vector3.forward);
        turret.rotation = Quaternion.AngleAxis(angle - Mathf.PI / 2 * Mathf.Rad2Deg, Vector3.forward);
        int temPlayerIndex = SelectTurret(angle);
        if (temPlayerIndex != currentPlayerIndex)
        {
            currentPlayerIndex = temPlayerIndex;
            InstantiatePlayer();
        }
        else
        {
            Fire();
           
        }
       

    }
    
    

    void Fire(){
        if (bullet != null)
            Instantiate(bullet, turret.position, turret.rotation);
            GameManager.shared.PlayFireParticle(turret.transform);
         GameManager.shared.bulletCount++;
         GameManager.shared.ScoreUpdate();
       
    }
    int SelectTurret(float angle)
    {
        int temPlayerIndex = 0;
        if (angle >= 0 && angle < 15)
        {
            temPlayerIndex = 0;
        }
        else if (angle >= 15 && angle < 30)
        {
            temPlayerIndex = 1;
        }
        else if (angle >= 30 && angle < 45)
        {
            temPlayerIndex = 2;
        }
        else if (angle >= 45 && angle < 60)
        {
            temPlayerIndex = 3;
        }
        else if (angle >= 60 && angle < 75)
        {
            temPlayerIndex = 4;
        }
        else if (angle >= 75 && angle < 90)
        {
            temPlayerIndex = 5;
        }
        return temPlayerIndex;


    }
    //public void ClearLog() 
    //{
    //    var assembly = Assembly.GetAssembly(typeof(UnityEditor.Editor));
    //    var type = assembly.GetType("UnityEditor.LogEntries");
    //    var method = type.GetMethod("Clear");
    //    method.Invoke(new object(), null);
    //}
}
