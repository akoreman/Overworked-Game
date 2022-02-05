using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class AddMachineToHandler : MonoBehaviour
{
    GameObject gameState;

    public Vector3 placementPosition;
    public string interactionType;
    public int interactionTime;

    public bool hasOutput;
    public GameObject outputObject;

    public bool destroyMachineOnCompletion;


    void Awake()
    {
        gameState = GameObject.Find("Game State");

        if (hasOutput)
        {
            //movableObject outputObject = new movableObject(outputTransform.gameObject, outputType);
            gameState.GetComponent<MachineHandler>().RegisterObject(this.gameObject, interactionType, placementPosition, interactionTime, outputObject, destroyMachineOnCompletion);

        }
        else
        {
            gameState.GetComponent<MachineHandler>().RegisterObject(this.gameObject, interactionType, placementPosition, interactionTime, destroyMachineOnCompletion);
        }


    }
}
