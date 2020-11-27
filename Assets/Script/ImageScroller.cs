using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageScroller : MonoBehaviour
{

    private Renderer meshRenderer;
    public float scrollSpeed = 0.5f;
    //public float playerOffset;


    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.sortingLayerName = "Scenary";
        meshRenderer.sortingOrder = -10;
    }
    void FixedUpdate()
    {
        float offset = Time.time * scrollSpeed;
        meshRenderer.material.mainTextureOffset = new Vector2(offset, 0);
    }
}
