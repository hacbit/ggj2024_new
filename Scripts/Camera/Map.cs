using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    public GameObject mainCamera;
    public float mapWidth;
    public float mapNums;

    private float totalWidth;
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        mapWidth = GetComponent<SpriteRenderer>().bounds.size.x;
        totalWidth = mapWidth * mapNums;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 tempPosition = transform.position;
        if(mainCamera.transform.position.x > transform.position.x + mapWidth / 2)
        {
            tempPosition.x += totalWidth;
            transform.position = tempPosition;
        }
        else if(mainCamera.transform.position.x < transform.position.x - mapWidth / 2)
        {
            tempPosition.x -= totalWidth;
            transform.position = tempPosition;
        }
    }
}
