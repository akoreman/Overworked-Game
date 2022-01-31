using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class AddMachineToHandler : MonoBehaviour
{
    GameObject gameState;


    public Vector3 placementPosition;
    public string interactionType;
    public int interactionTime;
    public Transform finishedObject;
    //public GameObject Object;

    void Awake()
    {
        gameState = GameObject.Find("Game State");
 
        //Machine machine = new Machine(this.gameObject, interactionType, placementPosition, interactionTime, MachineHandler currentHandler)

        gameState.GetComponent<MachineHandler>().RegisterObject(this.gameObject, interactionType, placementPosition, interactionTime, finishedObject);
    }
}
