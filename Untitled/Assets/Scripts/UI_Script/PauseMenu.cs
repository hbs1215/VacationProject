using UnityEngine;
using System.Collections;
//using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {

    public GameObject PauseUI;
    //private bool isPause;


	// Use this for initialization
	void Start () {
        //isPause = false;
        PauseUI.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void resume_game()
    {
        Time.timeScale = 1;
        //isPause = false;
        PauseUI.SetActive(false);
    }

    public void quit_game()
    {
        Time.timeScale = 1;
        //isPause = false;
        Application.LoadLevel(0);
       // SceneManager.LoadScene(0);
    }

    void LateUpdate()
    {
        if (Time.timeScale == 0)
        {
            if (Input.GetButtonDown("Cancel"))
            {
                Time.timeScale = 1;
                //isPause = false;
                PauseUI.SetActive(false);
            }
        }

        else if (Time.timeScale != 0)
        {
            if (Input.GetButtonDown("Cancel"))
            {
                Time.timeScale = 0;
                //isPause = true;
                PauseUI.SetActive(true);
            }
        }

    }

}
