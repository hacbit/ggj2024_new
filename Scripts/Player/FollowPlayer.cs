using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public float minX, maxX, minY, maxY;

    void Start()
    {
        transform.position = new Vector3(minX, minY, transform.position.z);
    }

    void Update()
    {
        if (TryGetPlayer(out GameObject player)) {
            float x = player.transform.position.x;
            float y = player.transform.position.y;

            transform.position = new Vector3(
                Mathf.Clamp(x, minX, maxX),
                Mathf.Clamp(y, minY, maxY),
                transform.position.z
            );
        }
    }

    public bool TryGetPlayer(out GameObject player) {
        player = GameObject.FindGameObjectWithTag("Player");
        if (player != null) {
            return true;
        } else {
            return false;
        }
    }
}
