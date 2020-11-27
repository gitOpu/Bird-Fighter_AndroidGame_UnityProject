using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Boundary : MonoBehaviour
{
    public enum BoundaryLocation
    {
        LEFT, TOP, RIGHT, BOTTOM
    };
    public BoundaryLocation direction;
    private BoxCollider2D boxCollider;

    public float boundaryWidth = 0.5f;
    public float overHang = 0.5f;
    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        Vector3 topLeft = Camera.main.ScreenToWorldPoint(new Vector3(0, Camera.main.pixelHeight, 0));
        Vector3 topRight = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth, Camera.main.pixelHeight, 0));
        Vector3 lowerLeft = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0));
        Vector3 lowerRight = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth, 0, 0));

        if (direction == BoundaryLocation.TOP)
        {
            boxCollider.size = new Vector2(Mathf.Abs(topLeft.x) + Mathf.Abs(topRight.x) + overHang, boundaryWidth);
            boxCollider.offset = new Vector2(0, boundaryWidth / 2);
            transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth / 2, Camera.main.pixelHeight, 1));
        }
        if (direction == BoundaryLocation.BOTTOM)
        {
            boxCollider.size = new Vector2(Mathf.Abs(lowerLeft.x) + Mathf.Abs(lowerRight.x) + overHang, boundaryWidth);
            boxCollider.offset = new Vector2(0, -boundaryWidth / 2);
            transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth / 2, 0, 1));
        }
        if (direction == BoundaryLocation.LEFT)
        {
            boxCollider.size = new Vector2(boundaryWidth, Mathf.Abs(lowerLeft.y) + Mathf.Abs(topLeft.y) + overHang);
            boxCollider.offset = new Vector2(-boundaryWidth / 2, 0);
            transform.position = Camera.main.ScreenToWorldPoint(new Vector3(0, Camera.main.pixelHeight / 2, 1));
        }
        if (direction == BoundaryLocation.RIGHT)
        {
            boxCollider.size = new Vector2(boundaryWidth, Mathf.Abs(lowerRight.y) + Mathf.Abs(topRight.y) + overHang);
            boxCollider.offset = new Vector2(boundaryWidth / 2, 0);
            transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth, Camera.main.pixelHeight / 2, 1));
        }
    }

    // Draw Gizmos, To show gizmos, active it from game panel and select boundary object
    void Update()
    {
        Debug.DrawRay(transform.position, Vector3.up, Color.red);
        Debug.DrawRay(transform.position, Vector3.left, Color.red);
        Debug.DrawRay(transform.position, Vector3.right, Color.red);
        Debug.DrawRay(transform.position, Vector3.down, Color.red);
    }
}