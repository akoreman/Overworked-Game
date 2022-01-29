using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineHandler : MonoBehaviour
{
    public List<Machine> machineList = new List<Machine>();

    public void RegisterObject(GameObject Object, string interactionType, Vector3 placementPosition, int interactionTime)
    {
        machineList.Add(new Machine(Object, interactionType, placementPosition, interactionTime));
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
}

public class Machine : MonoBehaviour
{
    public Transform objectTransform;

    public GameObject item;

    public string interactionType;
    int interactionTime;
    public bool interactionCompleted;

    bool machineFilled;

    Vector3 placementPosition;

    public Machine(GameObject Object, string interactionType, Vector3 placementPosition, int interactionTime)
    {
        item = Object;
        objectTransform = item.transform;
        this.interactionType = interactionType;
        this.placementPosition = placementPosition;
        this.interactionTime = interactionTime;
        interactionCompleted = false;
        machineFilled = false;
    }

    public void PlaceObject(movableObject Object)
    {
        print(this.objectTransform.position);
        Object.objectTransform.position = this.objectTransform.position + placementPosition;
        //Object.objectTransform.localPosition = new Vector3(3f, 2f, 1f);
        print(Object.objectTransform.position);
        //Object.objectTransform.localPosition = placementPosition;
        Object.objectTransform.GetChild(0).localPosition = new Vector3(0f, 0f, 0f);
        Object.Hands.gameObject.SetActive(false);
        Object.objectTransform.localEulerAngles = new Vector3(0f,0f,0f);

        interactionCompleted = false;
        machineFilled = true;
        Object.freeToGrab = false;

        StartCoroutine(StartMachineCoroutine( Object));
    }

    private IEnumerator StartMachineCoroutine(movableObject Object)
    {
        yield return new WaitForSeconds(interactionTime);
        interactionCompleted = true;
        Object.freeToGrab = true;
    }

}
