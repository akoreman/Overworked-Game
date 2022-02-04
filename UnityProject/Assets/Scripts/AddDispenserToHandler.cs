using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddDispenserToHandler : MonoBehaviour
{
    GameObject gameState;

    public GameObject outputObject;
    //public string interactionType;

    // Start is called before the first frame update
    void Awake()
    {
        gameState = GameObject.Find("Game State");

        //movableObject outputObject = new movableObject(outputTransform.gameObject, interactionType);

        gameState.GetComponent<DispenserHandler>().RegisterDispenser(this.gameObject, outputObject);

    }

}
