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
        machineList.Add(new Machine(Object, interactionType, placementPosition, interactionTime, this, outputObject)); 
    }

    public List<Machine> MachinesWithinGrabRadius(float grabRadius, Vector3 Position)
    {
        float Distance = 0;
  
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
        yield return new WaitForSeconds(machine.interactionTime);

        Destroy(machine.inputObject.gameObject);
        gameState.GetComponent<ObjectHandler>().RemoveObject(machine.inputObject);

        Instantiate(machine.outputObject.gameObject);

        machine.outputObject.gameObject.transform.position = machine.gameObject.transform.position + machine.localObjectPlacement;
        machine.outputObject.gameObject.GetComponent<Rigidbody>().freezeRotation = true;
        machine.outputObject.gameObject.GetComponent<Rigidbody>().useGravity = false;
        machine.outputObject.gameObject.GetComponent<Collider>().enabled = false;
    }
}

public class Machine
{
    MachineHandler machineHandler;
    public GameObject gameObject;

    public movableObject inputObject;
    public movableObject outputObject;

    public bool machineFilled;

    public string interactionType;
    public int interactionTime;

    public Vector3 localObjectPlacement;

    public Machine(GameObject Object, string interactionType, Vector3 localObjectPlacement, int interactionTime, MachineHandler machineHandler, movableObject outputObject)
    {
        this.gameObject = Object;

        this.interactionType = interactionType;
        this.localObjectPlacement = localObjectPlacement;
        this.interactionTime = interactionTime;
        this.machineHandler = machineHandler;
        this.outputObject = outputObject;

        machineFilled = false;
    }

    public void PlaceObject(movableObject inputObject)
    {
        inputObject.gameObject.transform.position = this.gameObject.transform.position + localObjectPlacement;

        inputObject.gameObject.transform.GetChild(0).localPosition = new Vector3(0f, 0f, 0f);
        inputObject.gameObject.transform.GetChild(2).localPosition = new Vector3(0f, 0f, 0f);
        inputObject.hands.gameObject.SetActive(false);
        inputObject.gameObject.transform.localEulerAngles = new Vector3(0f,0f,0f);

        this.inputObject = inputObject;

        machineFilled = true;
        inputObject.freeToGrab = false;

        machineHandler.StartRoutine(this);
    }



}
