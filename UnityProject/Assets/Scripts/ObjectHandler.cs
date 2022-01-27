using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectHandler : MonoBehaviour
{
    GameObject gameState;
    Transform Hands;


    public GameObject Object;

    void Awake()
    {
        gameState = GameObject.Find("Game State");
        Hands = this.gameObject.transform.GetChild(1);
       
        Hands.gameObject.SetActive(false);

        gameState.GetComponent<GameState>().RegisterObject(this.gameObject);
    }
}
