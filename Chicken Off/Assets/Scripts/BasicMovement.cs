using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicMovement : MonoBehaviour
{
    public float speed = 20f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    

    void Update()
    {
        Vector3 pos = transform.position;

        if (Input.GetKey("w"))
        {
            Debug.Log("Got W");
            pos.z += speed * Time.deltaTime;
        }
        if (Input.GetKey("s"))
        {
            pos.z -= speed * Time.deltaTime;
        }
        if (Input.GetKey("d"))
        {
            pos.x += speed * Time.deltaTime;
        }
        if (Input.GetKey("a"))
        {
            pos.x -= speed * Time.deltaTime;
        }

        // One set of inputs (like left joystick) for movement,
        // One set of inputs for tilting (like right joystick no camera control)
        // that should be pretty much it. 

        // transform.Translate(pos);
        transform.position = pos;
    }
}
