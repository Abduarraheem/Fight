using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class OnStart : MonoBehaviour
{
    
    public GameObject player1Prefab;
    public GameObject player2Prefab;
    public Vector3[] validSpawns;
    // Start is called before the first frame update
    void Start()
    {
        PlayerInput.Instantiate(player2Prefab, controlScheme: "Keyboard", pairWithDevice: Keyboard.current);
        PlayerInput.Instantiate(player2Prefab, controlScheme: "Player 2", pairWithDevice: Keyboard.current);
        GameObject[] foundObjects = GameObject.FindGameObjectsWithTag("Player");
        for (int i = 0; i < foundObjects.Length; i ++)
        {
            foundObjects[i].transform.position = validSpawns[i];
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
