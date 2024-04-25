using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class PlayerBoundary : MonoBehaviour
{
    private Vector2 screenBounds;
    private float objectWidth;
    private float objectHeight;

    public GameObject pathFinding;

    void Start()
    {
        // Get the screen bounds based on camera
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        objectWidth = transform.GetComponent<SpriteRenderer>().bounds.extents.x;  // half width of the object
        objectHeight = transform.GetComponent<SpriteRenderer>().bounds.extents.y;  // half height of the object
    }

    void LateUpdate()
    {
        Vector3 viewPos = transform.position;
        // Clamp position to stay within screen bounds
        viewPos.x = Mathf.Clamp(viewPos.x, screenBounds.x * -1 + objectWidth, screenBounds.x - objectWidth);
        viewPos.y = Mathf.Clamp(viewPos.y, screenBounds.y * -1 + objectHeight, screenBounds.y - objectHeight);
        transform.position = viewPos;
    }
}
