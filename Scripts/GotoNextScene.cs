using System.Collections;
using UnityEngine;

public class GotoNextScene : MonoBehaviour
{

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") || other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            other.gameObject.SendMessage("OnWin", SendMessageOptions.DontRequireReceiver);

            RandomPlayAudio.isStageDown = true;
            RandomPlayAudio.isWin = true;

            StartCoroutine(LoadNextSceneAfterDelay());
        }
    }

    private IEnumerator LoadNextSceneAfterDelay()
    {
        yield return new WaitForSeconds(0.5f);

        string thisSceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        string[] names = thisSceneName.Split(' ');
        int nextSceneIndex = int.Parse(names[1]) + 1;
        string nextSceneName = names[0] + " " + nextSceneIndex;
        UnityEngine.SceneManagement.SceneManager.LoadScene(nextSceneName);
    }
}
