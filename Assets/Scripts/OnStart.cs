using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class OnStart : MonoBehaviour
{
    
    public GameObject playerPrefab;
    // Start is called before the first frame update
    void Start()
    {
        PlayerInput.Instantiate(playerPrefab, controlScheme: "Keyboard", pairWithDevice: Keyboard.current);
        PlayerInput.Instantiate(playerPrefab, controlScheme: "Player 2", pairWithDevice: Keyboard.current);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
