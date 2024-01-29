using System.Collections;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [Tooltip("The reference to the player which created this bomb.")]
    public GameObject player;

    [Tooltip("The time before the bomb explodes.")]
    public readonly float explodeTime = 1.5f;
    [Tooltip("The radius of the explosion.")]
    public readonly float explodeRadius = 2f;
    [Tooltip("The force of the explosion.")]
    public readonly float maxExplodeForce = 0.5f;

    [Tooltip("Already used time.")]
    private float usedTime = 0f;

    public void Update()
    {
        usedTime += Time.deltaTime;
        if (usedTime >= explodeTime)
        {
            Explode();
        }
    }

    private void Explode()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explodeRadius);
        // 仅对Player施加力
        var playerCollider = player.GetComponent<Collider2D>();

        foreach (var collider in colliders)
        {
            if (collider == playerCollider) {
                Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
                Vector2 direction = rb.position - (Vector2)(transform.position - Player.offset);
                Debug.Log("direction: " + direction);
                direction = direction.normalized;
                rb.AddForce(direction * maxExplodeForce, ForceMode2D.Impulse);
            }
        }

        // 启用Animator组件播放爆炸动画
        Animator animator = GetComponent<Animator>();
        
        player.SendMessage("OnReduceBombCount", SendMessageOptions.DontRequireReceiver);

        StartCoroutine(PlayExplosionAnimation());

        Destroy(gameObject, 0.5f);

        Debug.Log("Explode");
    }

    private IEnumerator PlayExplosionAnimation()
    {
        Animator animator = GetComponent<Animator>();
        animator.enabled = true;
        yield return new WaitForSeconds(0.5f);
    }
}