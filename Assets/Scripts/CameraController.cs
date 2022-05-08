using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public Transform cam;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float minX = 0.0f;
        float minZ = 0.0f;
        float maxX = 0.0f;
        float maxZ = 0.0f;
        GameObject[] foundObjects = GameObject.FindGameObjectsWithTag("Player");
        for (int i = 0; i < foundObjects.Length; i ++)
        {
            Vector3 t = foundObjects[i].transform.position;
            if (t.x < minX) minX = t.x;
            if (t.z < minZ) minZ = t.z;
            if (t.x > maxX) maxX = t.x;
            if (t.z > maxZ) maxZ = t.z;
        }
        float centerX = (minX + maxX) / 2;
        float centerZ = (minZ + maxZ) / 2;
        cam.position = new Vector3(centerX, 6.0f + centerZ, -7.0f);
    }
}
