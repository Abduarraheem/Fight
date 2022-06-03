using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageSwag : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void onCityClick() {
        SceneManager.LoadScene("City", LoadSceneMode.Single);
    }

    void onLoungeClick() {
        SceneManager.LoadScene("Lounge", LoadSceneMode.Single);
    }

    void onPicnicClick() {
        SceneManager.LoadScene("PickNick", LoadSceneMode.Single);
    }
}
