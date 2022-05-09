using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class OnStart : MonoBehaviour
{
    
    public GameObject player1Prefab;
    public GameObject player2Prefab;
    // Start is called before the first frame update
    void Start()
    {
        PlayerInput.Instantiate(player2Prefab, controlScheme: "Keyboard", pairWithDevice: Keyboard.current);
        PlayerInput.Instantiate(player2Prefab, controlScheme: "Player 2", pairWithDevice: Keyboard.current);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
