using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectHandler : MonoBehaviour
{
    public List<movableObject> objectList = new List<movableObject>();

   
    public void RegisterObject(GameObject Object, string interactionType)
    {
        objectList.Add(new movableObject(Object, interactionType));
    }

    public void RegisterObject(movableObject inputObject)
    {
        objectList.Add(inputObject);
    }

    public void RemoveObject(movableObject inputObject)
    {
        objectList.Remove(inputObject);
        print("remove object");
    }

    public movableObject CreateAndRegisterObject(GameObject inputObject)
    {
        Instantiate(inputObject);

        return objectList[objectList.Count - 1];
    }



    public movableObject NearestObjectWithinGrabRadius(float grabRadius, Vector3 Position)
    {
        float Distance = 0;
        float minDistance = 100000;

        movableObject nearestObject = null;

        foreach (movableObject x in objectList)
        {
            Distance = (x.gameObject.transform.position - Position).magnitude;

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

public class movableObject 
{
    public GameObject hands;
    public GameObject gameObject;

    public string interactionType;
    public bool freeToGrab;
    public bool taskCompleted;

    public Machine machine;

    public movableObject(GameObject gameObject, string interactionType)//, ObjectHandler objectHandler)
    {
        this.gameObject = gameObject;
        this.hands = gameObject.transform.GetChild(1).gameObject;
        this.interactionType = interactionType;
        this.freeToGrab = true;
        this.machine = null;
    }

}
