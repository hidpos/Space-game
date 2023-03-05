using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceshipController : MonoBehaviour
{
    public float smooth = 70;
    private float tiltAngle = 35.0f;

    // Update is called once per frame
    void Update()
    {
        // spaceship movement
        if (Input.GetKey(KeyCode.A) && transform.localPosition.x - 0.005f > -1.1f)
                transform.position += new Vector3(-0.005f, 0, 0);
        if (Input.GetKey(KeyCode.D) && transform.localPosition.x + 0.005f < 1.1f)
                transform.position += new Vector3(0.005f, 0, 0);
        
        // spaceship tilt
        var tiltAroundZ = Input.GetAxis("Horizontal") * tiltAngle;
        var target = Quaternion.Euler (0, 0, -tiltAroundZ);
        transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * smooth);
    }
}