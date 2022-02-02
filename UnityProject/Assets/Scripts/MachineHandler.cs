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

    public void RegisterObject(GameObject Object, string interactionType, Vector3 placementPosition, int interactionTime, movableObject outputObject)
    {
        //Machine machine = gameState.AddComponent<Machine>();
        //machine = new Machine(Object, interactionType, placementPosition, interactionTime, this);
        machineList.Add(new Machine(Object, interactionType, placementPosition, interactionTime, this, outputObject)); 
        //machineList.Add(machine);
    }

    public List<Machine> MachinesWithinGrabRadius(float grabRadius, Vector3 Position)
    {
        float Distance = 0;
        float minDistance = 100000;

        List<Machine> nearestMachines = new List<Machine>(); ;

        foreach (Machine x in machineList)
        {
            Distance = (x.gameObject.transform.position - Position).magnitude;

            if (Distance < grabRadius)
                nearestMachines.Add(x);
        }

        if (nearestMachines.Count > 0)
            return nearestMachines;
        else
            return null;
    }

    public void StartRoutine(Machine machine)
    {
        StartCoroutine(StartMachineCoroutine(machine));
    }

    IEnumerator StartMachineCoroutine(Machine machine)
    {
        yield return new WaitForSecondsRealtime(machine.interactionTime);

        //machine.CompleteMachineTaks();

        Destroy(machine.inputObject.item);
        gameState.GetComponent<ObjectHandler>().RemoveObject(machine.inputObject);

        Instantiate(machine.outputObject.item);
        gameState.GetComponent<ObjectHandler>().RegisterObject(machine.outputObject);

        machine.outputObject.item.transform.position = machine.gameObject.transform.position + machine.localItemPlacement;

        print(machine.gameObject.transform.position);
        print(machine.localItemPlacement);

        //machine.outputObject.item.transform.GetChild(0).localPosition = new Vector3(0f, 0f, 0f);

        print("Task Completed");
    }
}

public class Machine //: MonoBehaviour
{
    MachineHandler machineHandler;

    public movableObject inputObject;
    public movableObject outputObject;

    public GameObject gameObject;

    public bool machineFilled;

    public string interactionType;
    public int interactionTime;
    //public bool interactionCompleted;

    public Vector3 localItemPlacement;

    //public Transform finishedObject;

    public Machine(GameObject Object, string interactionType, Vector3 localItemPlacement, int interactionTime, MachineHandler machineHandler, movableObject outputObject)
    {
        this.gameObject = Object;

        this.interactionType = interactionType;
        this.localItemPlacement = localItemPlacement;
        this.interactionTime = interactionTime;
        this.machineHandler = machineHandler;
        this.outputObject = outputObject;

        machineFilled = false;
    }

    public void PlaceObject(movableObject inputObject)
    {
        inputObject.objectTransform.position = this.gameObject.transform.position + localItemPlacement;

        inputObject.objectTransform.GetChild(0).localPosition = new Vector3(0f, 0f, 0f);
        inputObject.objectTransform.GetChild(2).localPosition = new Vector3(0f, 0f, 0f);
        inputObject.Hands.gameObject.SetActive(false);
        inputObject.objectTransform.localEulerAngles = new Vector3(0f,0f,0f);

        this.inputObject = inputObject;

        //interactionCompleted = false;
        machineFilled = true;
        inputObject.freeToGrab = false;

        machineHandler.StartRoutine(this);
    }

    /*
    public void CompleteMachineTaks()
    {
        //inputObject.freeToGrab = true;
        //inputObject.CompleteTask();

        Destroy(inputObject.item);
        Instantiate(machine.finishedObject);

        machine.finishedObject.position = machine.objectTransform.position + machine.placementPosition;

        machine.finishedObject.GetChild(0).localPosition = new Vector3(0f, 0f, 0f);

        print("Task Completed");
    }
    */


}
