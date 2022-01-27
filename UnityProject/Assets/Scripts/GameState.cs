using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour
{
    public List<movableObject> objectList = new List<movableObject>();

    public void RegisterObject(GameObject Object)
    {
        objectList.Add(new movableObject(Object) );
    }

    public class movableObject
    {
        public Transform objectTransform;
        public Transform Hands;

        public GameObject item;

        public movableObject(GameObject Object)
        {
            item = Object;
            objectTransform = item.transform;
            Hands = item.transform.GetChild(1);
        }
    }

    public movableObject NearestObjectWithinGrabRadius(float grabRadius, Vector3 Position)
    {
        float Distance = 0;
        float minDistance = 100000;

        movableObject nearestObject = null;

        foreach (movableObject x in objectList)
        {
            Distance = (x.item.transform.position - Position).magnitude;

            if (Distance < minDistance)
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
