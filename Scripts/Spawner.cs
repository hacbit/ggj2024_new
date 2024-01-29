using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject playerPrefab;

    void Start()
    {
        if (playerPrefab == null) {
            Debug.LogError("Player Prefab is null");
        }

        var player = Instantiate(playerPrefab, this.transform.position - Player.offset, Quaternion.identity);
        var playerComponent = player.GetComponent<Player>();

        if (playerComponent != null)
        {
            playerComponent.respawnPoint = this.transform.position - Player.offset;
        } else {
            Debug.LogError("Player Prefab does not have Player Component");
        }
    }
}
