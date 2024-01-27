using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuController : MonoBehaviour
{
    [SerializeField] private bool isPaused = false;
    public GameObject GameObject;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(!isPaused)
            {
                Debug.Log("Pause");
                isPaused = true;
                Time.timeScale = 0;
                GameObject.SetActive(true);
            }
            else
            {
                Debug.Log("Unpause");
                isPaused = false;
                Time.timeScale = 1;
                GameObject.SetActive(false);
            }
        }
    }
}
