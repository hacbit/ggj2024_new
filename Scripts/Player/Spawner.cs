using UnityEngine; 

public class Spawner : MonoBehaviour
{
    public GameObject playerPrefab;

    void Start()
    {
        var player = Instantiate(playerPrefab, this.transform.position, Quaternion.identity);
        player.GetComponent<Player>().respawnPoint = this.transform.position;
    }
}
