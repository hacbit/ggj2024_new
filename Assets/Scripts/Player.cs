using UnityEngine.InputSystem;
using UnityEngine;
using System.Collections;
using Unity.Collections;

public class Player : MonoBehaviour
{
    [Tooltip("The layer of the ground.")]
    public LayerMask groundLayer;
    [Tooltip("The radius of the circle to check if the player is on the ground.")]
    private float checkRaduis;
    [Tooltip("The offset of the circle to check if the player is on the ground.")]
    public readonly Vector2 buttomOffset = new Vector2(0.2f, 0);

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

    [Tooltip("The Bubble Prefab. It will play effect in player's position when player is invisible.")]
    public GameObject dropBubble;
    [Tooltip("The Bubble Prefab. It will play when player win")]
    public GameObject winBubble;
    

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        checkRaduis = this.GetComponent<CapsuleCollider2D>().size.y / 2 + 0.2f;
        controller = new Controller();
        controller.Enable();
    }

    public void Start()
    {
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Bomb"));
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 movement = controller.ActionMap.Movement.ReadValue<Vector2>();
        bool isJump = controller.ActionMap.Jump.triggered;
        bool isPlaceBomb = controller.ActionMap.PlaceBomb.triggered;

        Movement(movement);
        Jump(isJump);
        PlaceBomb(isPlaceBomb);
        AutoPlaceBomb();
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

    private void AutoPlaceBomb()
    {
        if (AutoPlaceBombTimer > 0f)
        {
            AutoPlaceBombTimer -= Time.deltaTime;
            return;
        }
        // 独立于玩家放置炸弹的计数
        GameObject bomb = Instantiate(bombPrefab, transform.position, Quaternion.identity);
        bomb.GetComponent<Bomb>().player = this.gameObject;
        AutoPlaceBombTimer = AutoPlaceBombDelay;
    }

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
        if (gameObject.activeInHierarchy)
            StartCoroutine(RespawnAfterdropBubble());
    }

    private IEnumerator RespawnAfterdropBubble()
    {
        GameObject camera = GameObject.Find("Main Camera");

        var position = new Vector3(
            transform.position.x,
            camera.transform.position.y - 5,
            transform.position.z
        );
        // 播放dropBubble特效
        GameObject dropBubbleObj = Instantiate(dropBubble, position, Quaternion.identity);
        // 固定player
        var old = rb.constraints;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;

        Animator animator = dropBubbleObj.GetComponent<Animator>();
        while (animator.GetCurrentAnimatorStateInfo(0).IsName("DropBubble") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
        {
            yield return null;
        }
        Destroy(dropBubbleObj);

        // 重置player
        rb.constraints = old;

        transform.position = respawnPoint;
    }

    [Tooltip("Play winBubble animation when receive message.")]
    public IEnumerator PlayWinBubble()
    {
        var position = this.transform.position + new Vector3(2f, 2f, 0f);
        GameObject winBubbleObj = Instantiate(winBubble, position, Quaternion.identity);

        var old = rb.constraints;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;

        Animator animator = winBubbleObj.GetComponent<Animator>();

        while (animator.GetCurrentAnimatorStateInfo(0).IsName("WinBubble") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
        {
            yield return new WaitForSeconds(0.5f);
        }
        Destroy(winBubbleObj);

        rb.constraints = old;
    }

    [Tooltip("Reduce the bomb count when receive message.")]
    public void OnReduceBombCount()
    {
        bombCount -= 1;
    }
}
