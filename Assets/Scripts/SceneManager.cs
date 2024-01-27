using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    // Start is called before the first frame update
    public void ChangeScene(string sceneName)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }
    public void GoToMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void SetActive(GameObject obj)
    {
        if (obj.activeSelf)
            obj.SetActive(false);
        else
            obj.SetActive(true);
    }
    //public void KeyControllActive(GameObject obj)
    //{
    //    if(Input.GetKeyDown(KeyCode.Escape))
    //        SetActive(obj);
    //}
}
