using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddObjectToHandler : MonoBehaviour
{
    GameObject gameState;
    Transform Hands;

    public string interactionType;

    public bool isMachine;
    public string machineInteractionType;
    public GameObject outputObject;

    void Awake()
    {
        gameState = GameObject.Find("Game State");
        Hands = this.gameObject.transform.GetChild(1);

        //this.gameObject.transform.GetChild(2).gameObject.SetActive(false);
        Hands.gameObject.SetActive(false);

        movableObject movableobject = new movableObject(this.gameObject, interactionType);
        Machine machine = new Machine(this.gameObject, machineInteractionType, gameState.GetComponent<MachineHandler>(),  movableobject, outputObject);

        gameState.GetComponent<ObjectHandler>().RegisterObject(movableobject);
        //gameState.GetComponent<ObjectHandler>().RegisterObject(this.gameObject, interactionType);

        if (isMachine)
            gameState.GetComponent<MachineHandler>().RegisterObject(machine);
    }
}

