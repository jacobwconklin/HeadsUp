using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Node class
public class AvailableNode
{
    public bool isAvailable = true;
    public GameObject value = null;
    public AvailableNode next = null;
    public AvailableNode prev = null;
}
public class AvailableLinkedList
{
    private AvailableNode head = null;

    /*
     * Linked list but nodes can be set to unavailable so iterating
     * through the list always gives the next available node.
     * List instantiated by providing a List of objects
     */
    public AvailableLinkedList(List<GameObject> values)
    {
        if (values == null) return;

        foreach (GameObject value in values)
        {
            AvailableNode newNode = new AvailableNode();
            newNode.value = value;
            addNode(newNode);
        }
    }

    // Insert new node at end of linked list
    private void addNode(AvailableNode newNode)
    {
        if (head == null)
        {
            head = newNode;
            head.prev = head;
            head.next = head;
        } else
        {
            head.prev.next = newNode;
            newNode.prev = head.prev.next;
            head.prev = newNode;
            newNode.next = head;
        }
    }

    public AvailableNode getFirstAvailable()
    {
        AvailableNode nextNode = head;
        while (!nextNode.isAvailable)
        {
            nextNode = nextNode.next;
            if (nextNode == head) return nextNode;
        }
        nextNode.isAvailable = false;
        return nextNode;
    }

    public AvailableNode nextAvailable(AvailableNode node)
    {
        AvailableNode nextNode = node.next;
        while (!nextNode.isAvailable && nextNode != node)
        {
            nextNode = nextNode.next;
        }
        node.isAvailable = true;
        nextNode.isAvailable = false;
        return nextNode;
    }

    public void setAllAvailable()
    {
        head.isAvailable = true;
        AvailableNode nextNode = head.next;
        while (nextNode != head)
        {
            nextNode.isAvailable = true;
            nextNode = nextNode.next;
        }
    }
}
