using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DispenserHandler : MonoBehaviour
{
    public List<Dispenser> dispenserList = new List<Dispenser>();
    GameObject gameState;
    /*
    public void RegisterDispenser(GameObjGameObject Object)
    {
        objectList.Add(new movableObject(Object, interactionType));//, this) );
    }
    */
    void Awake()
    {
        gameState = GameObject.Find("Game State");
    }

    public void RegisterDispenser(GameObject gameObject, GameObject outputObject)
    {
        

        dispenserList.Add(new Dispenser(gameObject, outputObject, this));
    }

    public Dispenser NearestDispenserWithinGrabRadius(float grabRadius, Vector3 Position)
    {
        float Distance = 0;
        float minDistance = 100000;
        //bool objectPresent = false;

        Dispenser nearestDispenser = null;

        print("yaaay");

        foreach (Dispenser x in dispenserList)
        {
            print("yaay");

            Distance = (x.gameObject.transform.position - Position).magnitude;

            print(Distance);

            if (Distance < minDistance)
            {
                print("yay");
                minDistance = Distance;
                nearestDispenser = x;
            }
        }

        if (minDistance < grabRadius)
        {
            print("one");
            return nearestDispenser;
        }
        else
        {
            print("two");
            return null;
        }
    }

    public movableObject DispenseObject(Dispenser dispenser)
    {
        //Instantiate(dispenser.outputObject.gameObject);
        return gameState.GetComponent<ObjectHandler>().CreateAndRegisterObject(dispenser.outputObject);
    }
}

public class Dispenser
{
    DispenserHandler dispenserHandler;

    public GameObject gameObject;
    public GameObject outputObject;

    public Dispenser(GameObject gameObject, GameObject outputObject, DispenserHandler dispenserHandler)
    {
        this.gameObject = gameObject;
        this.outputObject = outputObject;
        this.dispenserHandler = dispenserHandler;
    }

    public movableObject DispenseObject()
    {
        return dispenserHandler.DispenseObject(this);
    }
}