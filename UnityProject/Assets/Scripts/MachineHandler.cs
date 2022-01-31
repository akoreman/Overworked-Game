using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineHandler : MonoBehaviour
{
    public List<Machine> machineList = new List<Machine>();

    GameObject gameState;

    void Awake()
    {
        gameState = GameObject.Find("Game State");
    }

    public void RegisterObject(GameObject Object, string interactionType, Vector3 placementPosition, int interactionTime, Transform finishedObject)
    {
        //Machine machine = gameState.AddComponent<Machine>();
        //machine = new Machine(Object, interactionType, placementPosition, interactionTime, this);
        machineList.Add(new Machine(Object, interactionType, placementPosition, interactionTime, this, finishedObject)); 
        //machineList.Add(machine);
    }

    public List<Machine> MachinesWithinGrabRadius(float grabRadius, Vector3 Position)
    {
        float Distance = 0;
        float minDistance = 100000;

        List<Machine> nearestMachines = new List<Machine>(); ;

        foreach (Machine x in machineList)
        {
            Distance = (x.item.transform.position - Position).magnitude;

            if (Distance < grabRadius)
                nearestMachines.Add(x);
        }

        if (nearestMachines.Count > 0)
            return nearestMachines;
        else
            return null;
    }

    public void StartRoutine(Machine machine, int waitTime, movableObject Object)
    {
        StartCoroutine(StartMachineCoroutine(machine, waitTime, Object));
    }

    IEnumerator StartMachineCoroutine(Machine machine, int waitTime, movableObject Object)
    {
        yield return new WaitForSecondsRealtime(waitTime);
       
        machine.interactionCompleted = true;
        Object.freeToGrab = true;
        Object.CompleteTask();

  
        print("Task Completed");
    }
}

public class Machine //: MonoBehaviour
{
    MachineHandler machineHandler;

    public Transform objectTransform;

    Transform finishedObject;

    public GameObject item;

    public string interactionType;
    int interactionTime;
    public bool interactionCompleted;

    bool machineFilled;

    Vector3 placementPosition;

    public Machine(GameObject Object, string interactionType, Vector3 placementPosition, int interactionTime, MachineHandler machineHandler, Transform finishedObject)
    {
        item = Object;
        objectTransform = item.transform;
        this.interactionType = interactionType;
        this.placementPosition = placementPosition;
        this.interactionTime = interactionTime;
        interactionCompleted = true;
        machineFilled = false;
        this.machineHandler = machineHandler;
        this.finishedObject = finishedObject;

    }

    public void PlaceObject(movableObject Object)
    {
        Object.objectTransform.position = this.objectTransform.position + placementPosition;

        Object.objectTransform.GetChild(0).localPosition = new Vector3(0f, 0f, 0f);
        Object.objectTransform.GetChild(2).localPosition = new Vector3(0f, 0f, 0f);
        Object.Hands.gameObject.SetActive(false);
        Object.objectTransform.localEulerAngles = new Vector3(0f,0f,0f);

        interactionCompleted = false;
        machineFilled = true;
        Object.freeToGrab = false;

        machineHandler.StartRoutine(this, interactionTime, Object);
    }



}
