using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsCheck : MonoBehaviour
{
    public bool isGround;
    public float checkRaduis;
    public LayerMask groundLayer;
    public CapsuleCollider2D coll;

    public Vector2 buttomOffset;
    // Start is called before the first frame update

    private void Awake()
    {
        coll = GetComponent<CapsuleCollider2D>();
    }
    private void Update()
    {
        Check();
    }
    public void Check()
    {
        //µÿ√ÊºÏ≤È
        isGround = Physics2D.OverlapCircle( (Vector2)transform.position + buttomOffset * transform.localScale, checkRaduis, groundLayer);
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere( (Vector2)transform.position + buttomOffset * transform.localScale, checkRaduis);
    }
}
