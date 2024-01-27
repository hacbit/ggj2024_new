using UnityEngine.InputSystem;
using UnityEngine;
using System.Collections;
using Unity.Collections;

public class Player : MonoBehaviour
{
    [Tooltip("The layer of the ground.")]
    public LayerMask groundLayer;
    [Tooltip("The radius of the circle to check if the player is on the ground.")]
    public float checkRaduis = 0.8f;
    [Tooltip("The offset of the circle to check if the player is on the ground.")]
    public Vector2 buttomOffset;

    [Tooltip("The prefab of the bomb.")]
    public GameObject bombPrefab;
    [Tooltip("The current count of your bomb.")]
    [Range(0, 1)]
    public int bombCount = 0;
    public const int MaxBombCount = 1;

    public Rigidbody2D rb;
    public readonly float speed = 5f;

    [Tooltip("The force of the player's jump.")]
    public readonly float jumpForce = 6f;
    [Tooltip("The count of your jump.")]
    [Range(0, 1)]
    public int jumpCount = 0;
    public const int MaxJumpCount = 1;

    [Tooltip("The respawn point of the player.")]
    public Vector3 respawnPoint;

    [Tooltip("Player Input System Controller.")]
    private Controller controller;

    [Tooltip("The timer to place bomb automatically.")]
    private float AutoPlaceBombTimer = 1f;
    [Tooltip("The delay to place bomb automatically.")]
    private readonly float AutoPlaceBombDelay = 1f;

    public Animator animator;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        controller = new Controller();
        controller.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 movement = controller.ActionMap.Movement.ReadValue<Vector2>();
        if (movement != null)
        {
            //animator.SetFloat("Speed", Mathf.Abs(movement.x));
            if (movement.x > 0)
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else if (movement.x < 0)
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }
        }
        bool isJump = controller.ActionMap.Jump.triggered;
        bool isPlaceBomb = controller.ActionMap.PlaceBomb.triggered;

        Movement(movement);
        Jump(isJump);

        PlaceBomb(isPlaceBomb);
        //AutoPlaceBomb();
    }

    private void Movement(Vector2 movement)
    {
        rb.velocity = new Vector2(movement.x * speed, rb.velocity.y);
    }

    private void Jump(bool isJump)
    {
        if (isJump && OnGround())
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        } else {
            //Debug.Log("Not on ground.");
        }
    }

    private void PlaceBomb(bool isPlaceBomb)
    {
        if (isPlaceBomb && bombCount < MaxBombCount)
        {
            GameObject bomb = Instantiate(bombPrefab, transform.position, Quaternion.identity);
            bomb.GetComponent<Bomb>().player = this.gameObject;

            bombCount += 1;
        }
    }

    //private void AutoPlaceBomb()
    //{
    //    if (AutoPlaceBombTimer > 0f)
    //    {
    //        AutoPlaceBombTimer -= Time.deltaTime;
    //        return;
    //    }
    //    // 独立于玩家放置炸弹的计数
    //    GameObject bomb = Instantiate(bombPrefab, transform.position, Quaternion.identity);
    //    bomb.GetComponent<Bomb>().player = this.gameObject;
    //    AutoPlaceBombTimer = AutoPlaceBombDelay;
    //}

    private bool OnGround()
    {
        var hit1 = Physics2D.Raycast(transform.position + (Vector3)buttomOffset, Vector2.down, checkRaduis, groundLayer);
        var hit2 = Physics2D.Raycast(transform.position - (Vector3)buttomOffset, Vector2.down, checkRaduis, groundLayer);

        if (hit1.collider != null)
        {
            float angle1 = Vector2.Angle(hit1.normal, Vector2.up);
            if (angle1 <= 30f)
            {
                return true;
            }
        }

        if (hit2.collider != null)
        {
            float angle2 = Vector2.Angle(hit2.normal, Vector2.up);
            if (angle2 <= 30f)
            {
                return true;
            }
        }

        return false;
    }

    public void OnBecameInvisible()
    {
        StartCoroutine(RespawnAfterDelay(1f));
    }

    private IEnumerator RespawnAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        transform.position = respawnPoint;
    }

    [Tooltip("Reduce the bomb count when receive message.")]
    public void OnReduceBombCount()
    {
        bombCount -= 1;
    }
}
