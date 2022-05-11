using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class OnStart : MonoBehaviour
{
    
    public GameObject player1Prefab;
    public GameObject player2Prefab;
    public Vector3[] validSpawns;

    // List of material list.
    [System.Serializable]
    public class MaterialList
    {
        public List<Material> materials;
    }
    public List<MaterialList> playerMaterials = new List<MaterialList>();

    // Start is called before the first frame update
    void Start()
    {
        PlayerInput.Instantiate(player2Prefab, controlScheme: "Keyboard", pairWithDevice: Keyboard.current);
        PlayerInput.Instantiate(player2Prefab, controlScheme: "Player 2", pairWithDevice: Keyboard.current);
        GameObject[] foundObjects = GameObject.FindGameObjectsWithTag("Player");
        for (int i = 0; i < foundObjects.Length; i ++)
        {
            foundObjects[i].transform.position = validSpawns[i];
            GameObject child = foundObjects[i].transform.GetChild(0).gameObject;
            child.GetComponent<Renderer>().materials = playerMaterials[i].materials.ToArray();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
