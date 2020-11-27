using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelController : MonoBehaviour
{
    
    public bool isActive;
    void Start()
    {
        gameObject.SetActive(isActive);
    }

    
}
