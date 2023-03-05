using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveFoot : MonoBehaviour
{
    private bool canMove = true;

    private void OnTriggerStay(Collider other)
    {
        canMove = true;
    }

    private void OnTriggerExit(Collider other)
    {
        canMove = false;
    }

    public bool isAbleToMove()
    {
        return canMove;
    }
}
