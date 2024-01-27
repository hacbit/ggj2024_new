using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform traget;
    public Transform farBackground, middleBackground;
    private Vector2 lastPosition;
    // Start is called before the first frame update
    void Start()
    {
        lastPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (TryGetPlayer(out GameObject player))
        {
            traget = player.transform;
        }
        transform.position = new Vector3(traget.position.x, traget.position.y, transform.position.z);
        Vector2 amountToMove = new Vector2(transform.position.x - lastPosition.x, transform.position.y - lastPosition.y);

        farBackground.position += new Vector3(amountToMove.x, 0f, 0f) * 0.05f;
        middleBackground.position += new Vector3(amountToMove.x, 0f, 0f) * 0.1f;

        lastPosition = transform.position;
    }

    public bool TryGetPlayer(out GameObject player)
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
