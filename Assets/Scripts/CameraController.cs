using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public GameObject cam;
    public float zoomLevel;

    // Update is called once per frame
    void Update()
    {
        float minX = 0.0f;
        float minY = 0.0f;
        float maxX = 0.0f;
        float maxY = 0.0f;
        float zoom = 0.0f;
        GameObject[] foundObjects = GameObject.FindGameObjectsWithTag("Player");
        for (int i = 0; i < foundObjects.Length; i ++)
        {
            Vector3 t = foundObjects[i].transform.position;
            if (t.x < minX) minX = t.x;
            if (t.y < minY) minY = t.y;
            if (t.x > maxX) maxX = t.x;
            if (t.y > maxY) maxY = t.y;
        }
        float centerX = (minX + maxX) / 2;
        float ydiff = (maxY - minY) / 2;
        float a = Math.Max(maxX - minX - 10, 0);
        float b = Math.Max(maxY - minY, 0);
        zoom = (float) Math.Sqrt(a*a + b*b);
        cam.transform.position = new Vector3(centerX, 3.0f + ydiff, -7.0f);
        Vector3 FORWARD = cam.transform.TransformDirection(Vector3.forward);
        cam.transform.localPosition -= FORWARD * zoom * zoomLevel;
    }
}
