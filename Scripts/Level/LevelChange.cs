using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelChange : MonoBehaviour
{
    private int level;
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            level = PlayerPrefs.GetInt("level");
            level++;
            Debug.Log(level);
            PlayerPrefs.SetInt("level", level);
            Debug.Log("Level" + (level + 1).ToString());
            UnityEngine.SceneManagement.SceneManager.LoadScene("Level " + (level+1).ToString());
        }
    }
        
}
