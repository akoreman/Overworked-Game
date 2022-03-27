using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddObjectToHandler : MonoBehaviour
{
    GameObject gameState;
    Transform Hands;

    public string interactionType;

    void Awake()
    {
        gameState = GameObject.Find("Game State");
        Hands = this.gameObject.transform.GetChild(1);

        //this.gameObject.transform.GetChild(2).gameObject.SetActive(false);
        Hands.gameObject.SetActive(false);

        gameState.GetComponent<ObjectHandler>().RegisterObject(this.gameObject, interactionType);
    }
}

