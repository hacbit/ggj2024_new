using Unity.Entities;
using UnityEngine;

public class GotoNextScene : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") || other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            StartCoroutine(other.GetComponent<Player>().PlayWinBubble());

            string thisSceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
            string[] names = thisSceneName.Split(' ');
            int nextSceneIndex = int.Parse(names[1]) + 1;
            string nextSceneName = names[0] + " " + nextSceneIndex;
            UnityEngine.SceneManagement.SceneManager.LoadScene(nextSceneName);
        }
    }
}
