using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;

    public void setHealth(float healthPercent) {
        slider.value = healthPercent;
    }

    void LateUpdate() {
        transform.LookAt(Camera.main.transform);
    }
}
