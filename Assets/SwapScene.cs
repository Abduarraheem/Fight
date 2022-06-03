using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SwapScene : MonoBehaviour
{
    public Slider slider;
    public float volume = 1.0f;

    Scene scene;
    // Start is called before the first frame update
    void Start()
    {}

    // Update is called once per frame
    void Update() {
        if (slider) volume = slider.value;
        if (SceneManager.GetActiveScene().name == "Main") {
            Debug.Log("Enter main");
        }
    }

    void onClick() {
        DontDestroyOnLoad(this);
        SceneManager.LoadScene("Main", LoadSceneMode.Single);
    }
}
