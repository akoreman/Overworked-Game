using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class AddMachineToHandler : MonoBehaviour
{
    GameObject gameState;


    public Vector3 placementPosition;
    public string interactionType;
    public string outputType;
    public int interactionTime;

    public Transform outputTransform;
    //public GameObject Object;

    void Awake()
    {
        gameState = GameObject.Find("Game State");

        movableObject outputObject = new movableObject(outputTransform.gameObject, outputType);

 
        //Machine machine = new Machine(this.gameObject, interactionType, placementPosition, interactionTime, MachineHandler currentHandler)

        gameState.GetComponent<MachineHandler>().RegisterObject(this.gameObject, interactionType, placementPosition, interactionTime, outputObject);
    }
}
