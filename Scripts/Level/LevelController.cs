using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelController : MonoBehaviour
{
    public List<Button> buttons;
    [SerializeField] private int level;
    // Start is called before the first frame update
    void Start()
    {
        level = PlayerPrefs.GetInt("level");
        for(int i = 0; i < buttons.Count; i++)
        {
             if(i > level)
             {
                 buttons[i].interactable = false;
             }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
