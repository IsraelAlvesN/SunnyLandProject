using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour
{
    public Transform platform, limitA, limitB;
    public float platformSpeed;
    public Vector3 destiny;
    public GameObject player;

    void Start()
    {
        platform.position = limitA.position;
        destiny = limitB.position;
    }

    void Update()
    {
        if (platform.position == limitA.position)
        {
            destiny = limitB.position;
        }
        if (platform.position == limitB.position)
        {
            destiny = limitA.position;
        }

        platform.position = Vector3.MoveTowards(platform.position, destiny, platformSpeed);
    }
}
