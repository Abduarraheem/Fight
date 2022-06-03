using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Swapper : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
		Button btn = gameObject.GetComponent<Button>();
		btn.onClick.AddListener(TaskOnClick);
    }

	void TaskOnClick(){
        SceneManager.LoadScene("Menu", LoadSceneMode.Single);
        Time.timeScale = 1.0f;
	}

}

