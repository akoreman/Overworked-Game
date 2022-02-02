using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectHandler : MonoBehaviour
{
    public List<movableObject> objectList = new List<movableObject>();

    public void RegisterObject(GameObject Object, string interactionType)
    {
        objectList.Add(new movableObject(Object, interactionType));//, this) );
    }

    public void RegisterObject(movableObject inputObject)
    {
        objectList.Add(inputObject);
    }

    public void RemoveObject(movableObject inputObject)
    {
        objectList.Remove(inputObject);
    }



    public movableObject NearestObjectWithinGrabRadius(float grabRadius, Vector3 Position)
    {
        float Distance = 0;
        float minDistance = 100000;
        bool objectPresent = false;

        movableObject nearestObject = null;

        foreach (movableObject x in objectList)
        {
            Distance = (x.item.transform.position - Position).magnitude;

            if (Distance < minDistance && x.freeToGrab)
            {
                minDistance = Distance;
                nearestObject = x;
            }
        }

        if (minDistance < grabRadius)
            return nearestObject;  
        else
            return null;
    }

}

public class movableObject  //: MonoBehaviour
{
    //ObjectHandler objectHandler;

    public Transform objectTransform;
    public Transform Hands;

    public GameObject item;
    public string interactionType;
    public bool freeToGrab;

    public bool taskCompleted;

    public movableObject(GameObject Object, string interactionType)//, ObjectHandler objectHandler)
    {
        this.item = Object;
        this.objectTransform = item.transform;
        this.Hands = item.transform.GetChild(1);
        this.interactionType = interactionType;
        this.freeToGrab = true;

        //this.objectHandler = objectHandler;
    }
    /*
    public void CompleteTask()
    {
        this.objectTransform.GetChild(2).gameObject.SetActive(true);
    }
    */
}
