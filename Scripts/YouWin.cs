using UnityEngine;

public class YouWin : MonoBehaviour
{


    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") || other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            

            Debug.Log("You Win!");

            UnityEngine.SceneManagement.SceneManager.LoadScene("Last");
        }
    }
}
