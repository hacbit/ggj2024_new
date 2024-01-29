using UnityEngine.InputSystem;
using UnityEngine;
using System.Collections;
using Unity.Collections;
using Unity.Entities.UniversalDelegates;

public class Player : MonoBehaviour
{
    [Tooltip("The layer of the ground.")]
    public LayerMask groundLayer;
    [Tooltip("The radius of the circle to check if the player is on the ground.")]
    public float checkRaduis;
    [Tooltip("The offset of the circle to check if the player is on the ground.")]
    public readonly Vector2 buttomOffset = new Vector2(0.2f, 0);

    [Tooltip("The prefab of the bomb.")]
    public GameObject bombPrefab;
    [Tooltip("The current count of your bomb.")]
    [Range(0, 1)]
    public static int bombCount = 0;
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

    [Tooltip("Offset from player root to player foot.")]
    public static Vector3 offset = new Vector3(1.5f, 2.5f, 0f);

    [Tooltip("Whether the player is off screen.")]
    private bool isOffScreen = false;
    

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

        Vector3 viewportPosition = Camera.main.WorldToViewportPoint(transform.position + offset);

        if (!isOffScreen && (viewportPosition.x < 0 || viewportPosition.x > 1 || viewportPosition.y < 0 || viewportPosition.y > 1))
        {
            isOffScreen = true;
            OnBecameInvisible();
        }
        else if (isOffScreen && viewportPosition.x >= 0 && viewportPosition.x <= 1 && viewportPosition.y >= 0 && viewportPosition.y <= 1)
        {
            isOffScreen = false;
        }
    }

    private void FixedUpdate()
    {
        AnimationController();
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
            Debug.Log("Jump");
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        } else {
            Debug.Log("Not on ground");
        }
    }

    private void PlaceBomb(bool isPlaceBomb)
    {
        if (isPlaceBomb && bombCount < MaxBombCount)
        {
            Debug.Log("Position: " + transform.position);
            GameObject bomb = Instantiate(bombPrefab, transform.position + offset, Quaternion.identity);
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
        GameObject bomb = Instantiate(bombPrefab, transform.position + offset, Quaternion.identity);
        bomb.GetComponent<Bomb>().player = this.gameObject;
        AutoPlaceBombTimer = AutoPlaceBombDelay;
    }

    private bool OnGround()
    {
        var hit1 = Physics2D.OverlapCircle(transform.position + offset, checkRaduis, groundLayer);

        if (hit1 != null)
        {
            return true;
        }
        return false;
    }

    public void OnBecameInvisible()
    {
        Debug.Log("OnBecameInvisible");
        if (gameObject.activeInHierarchy)
            StartCoroutine(RespawnAfterdropBubble());
    }

    private IEnumerator RespawnAfterdropBubble()
    {
        GameObject camera = GameObject.Find("Main Camera");

        RandomPlayAudio.isFall = true;

        var position = new Vector3(
            transform.position.x + offset.x,
            camera.transform.position.y - 5f,
            transform.position.z
        );
        Debug.Log("position: " + position);
        // 播放dropBubble特效
        GameObject dropBubbleObj = Instantiate(dropBubble, position, Quaternion.identity);

        Animator animator = dropBubbleObj.GetComponent<Animator>();
        while (animator.GetCurrentAnimatorStateInfo(0).IsName("DropBubble") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
        {
            yield return null;
        }
        Destroy(dropBubbleObj);

        transform.position = respawnPoint - offset;
    }

    [Tooltip("Play winBubble animation when receive message.")]
    public void OnWin()
    {
        var position = this.transform.position + offset + new Vector3(2f, 3f, 0f);
        GameObject winBubbleObj = Instantiate(winBubble, position, Quaternion.identity);

        Animator animator = winBubbleObj.GetComponent<Animator>();

        while (animator.GetCurrentAnimatorStateInfo(0).IsName("WinBubble") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
        {
            Debug.Log("WinBubble");
            return;
        }
        Destroy(winBubbleObj);
    }

    [Tooltip("Reduce the bomb count when receive message.")]
    public static void OnReduceBombCount()
    {
        if (bombCount > 0)
        {
            bombCount -= 1;
        }
    }


    [Tooltip("The animation controller of the player.")]
    public void AnimationController()
    {
        if (rb.velocity.x != 0)
        {
            GetComponent<Animator>().SetBool("isRun", true);
        }
        else
        {
            GetComponent<Animator>().SetBool("isRun", false);
        }
    }

}
