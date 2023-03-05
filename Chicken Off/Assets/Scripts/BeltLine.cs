using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeltLine : MonoBehaviour
{
    // Moves individual belt in a rectangle.
    [Tooltip("0 for TopRight, 1 for BottomRight, 2 for BottomLeft, and 3 for TopLeft")]
    [SerializeField] private List<GameObject> locations = new List<GameObject>();
    [SerializeField] private GameObject parentToBe = null;
    [SerializeField] private float beltSpeed = 20.0f;
    [SerializeField] private float pullSpeed = 0.10f;
    [SerializeField] private int goToNext = 0;
    [SerializeField] private bool moveX = true;

    // Start is called before the first frame update
    void Start()
    {

        StartCoroutine(MoveDirectly(gameObject, locations[goToNext].transform.position));
    }

    // Update is called once per frame
    void Update()
    {
        // Wait until successfully moved to locations[goToNext]
        if (transform.position == locations[goToNext].transform.position)
        {
            Debug.Log("inside update if");
            goToNext = ++goToNext % locations.Count;
            StartCoroutine(MoveDirectly(gameObject, locations[goToNext].transform.position));
        }
    }

    // Causes players touching platform to be moved with it
    private void OnCollisionStay(Collision collision)
    {
        // only move players
        if (moveX && collision.collider.gameObject.layer == LayerMask.NameToLayer("PLAYER") && collision.collider.gameObject.transform.parent != null)
        {
            // manual set players x and z in parent transform
            // Vector3 newSetPosition = transform.position;
            // newSetPosition.y = collision.collider.gameObject.transform.parent.transform.position.y;
            // newSetPosition.z = collision.collider.gameObject.transform.parent.transform.position.z;
            // TODO instead of just SETTING X apply a force to the rigid body in the direction of newSetPosition maybe?
            // Or in that case just apply a force in the Z direction
            // collision.collider.gameObject.transform.parent.transform.position = newSetPosition;
            // collision.collider.gameObject.transform.parent.GetComponent<Rigidbody>().AddForce(new Vector3(1, 0, 0) * pullSpeed, ForceMode.VelocityChange);

            Vector3 newSetPosition = collision.collider.gameObject.transform.parent.transform.position;
            newSetPosition.x += 1 * Time.deltaTime;
            collision.collider.gameObject.transform.parent.transform.position = newSetPosition;


        }
        /*
         * if (parentToBe != null)
        {
            // Temp parenting is better but it is effecting the scale of them for some reason
            // So I require an unscaled object to set as the parent as an intermediary
            collision.gameObject.transform.SetParent(parentToBe.transform, true);
        }
        */
    }

    private void OnCollisionExit(Collision collision)
    {
        /* Did not work
        if (parentToBe != null)
        {
            collision.gameObject.transform.parent = null;
            DontDestroyOnLoad(collision.gameObject);
        }
        */
    }

    // This actually moves the belt and any children
    IEnumerator MoveDirectly(GameObject obj, Vector3 target)
    {
        Vector3 startPosition = obj.transform.position;
        float time = 0f;

        while (obj.transform.position != target)
        {
            obj.transform.position = Vector3.Lerp(startPosition, target, (time / Vector3.Distance(startPosition, target)) * beltSpeed);
            time += Time.deltaTime;
            yield return null;
        }
    }
}
