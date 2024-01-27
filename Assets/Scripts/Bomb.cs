using UnityEngine;

public class Bomb : MonoBehaviour
{
    [Tooltip("The reference to the player which created this bomb.")]
    public GameObject player;

    [Tooltip("The time before the bomb explodes.")]
    public readonly float explodeTime = 1.2f;
    [Tooltip("The radius of the explosion.")]
    public readonly float explodeRadius = 10f;
    [Tooltip("The force of the explosion.")]
    public readonly float maxExplodeForce = 9f;

    [Tooltip("Already used time.")]
    private float usedTime = 0f;

    public void Update()
    {
        usedTime += Time.deltaTime;
        if (usedTime >= explodeTime)
        {
            Explode();
            Destroy(gameObject);

            player.SendMessage("OnReduceBombCount", SendMessageOptions.DontRequireReceiver);
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
                Vector2 direction = (rb.position - (Vector2)transform.position).normalized;
                // 越远Force越小
                var force = maxExplodeForce * (1 - Vector2.Distance(rb.position, transform.position) / explodeRadius);
                rb.AddForce(direction * force, ForceMode2D.Impulse);
            }
        }
    }
}