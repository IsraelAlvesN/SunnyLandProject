using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float offSetX = 3f;
    public float smooth = 0.1f;

    public float limitedUp = 2f;
    public float limitedDown = 0f;
    public float limitedLeft = 0f;
    public float limitedRight = 100f;

    private Transform player;
    private float playerX;
    private float playerY;

    void Start()
    {
        player = FindObjectOfType<PlayerController>().transform;
    }

    void FixedUpdate()
    {
        if(player != null)
        {
            playerX = Mathf.Clamp(player.position.x + offSetX, limitedLeft, limitedRight);
            playerY = Mathf.Clamp(player.position.y, limitedDown, limitedUp);

            //follow player
            transform.position = Vector3.Lerp(transform.position, new Vector3(playerX, playerY, transform.position.z), smooth);
        }
    }

    
}
