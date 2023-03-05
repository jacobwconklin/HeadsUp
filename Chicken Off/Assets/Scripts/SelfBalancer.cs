using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfBalancer : MonoBehaviour
{
    // My attemtp to have a script automatically balancing the character to assist players in staying upright
    // Somewhat works while player is mostly vertical but glitches on jump and when close to horizontal
    // and does now allow for tilting left or right only forward and back idk why. 

    private Rigidbody rb;
    public float balanceSpeed = 1.0f;
    [SerializeField] private Transform footTransform;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

        // Determine which direction to rotate towards
        Vector3 targetDirection = footTransform.position; // target.position - transform.position;
        //targetDirection.y = transform.position.y;

        // The step size is equal to speed times frame time.
        float singleStep = balanceSpeed * Time.deltaTime;

        // Rotate the forward vector towards the target direction by one step
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);

        // Draw a ray pointing at our target in
        Debug.DrawRay(transform.forward, newDirection, Color.green);
        Debug.DrawRay(transform.position, newDirection, Color.red);
        Debug.DrawRay(targetDirection, newDirection, Color.blue);

        // Calculate a rotation a step closer to the target and applies rotation to this object
        transform.rotation = Quaternion.LookRotation(newDirection, Vector3.up);
    }
}
