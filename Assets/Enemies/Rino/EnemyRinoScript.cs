using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState
{
    IDLE,
    ATTACKING,
    HIT_WALL,
    DEAD
}

public class EnemyRinoScript : EnemyScript
{
    private RaycastHit2D view;
    private RaycastHit2D hitBounds;
    private EnemyState state = EnemyState.IDLE;
    private Animator anim;
    private float viewDistance = 7f;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        HandleEnemyCollisions();
        switch (state)
        {
            case EnemyState.IDLE:
                WaitingForPlayer();
                break;
            case EnemyState.ATTACKING:
                rb.velocity = new Vector2(xMovement * 4f, rb.velocity.y);
                break;
            default:
                break;
        }

    }

    private void SetEnemyState(EnemyState newState)
    {
        anim.SetInteger("EnemyState", (int)newState);
        state = newState;
    }

    private void WaitingForPlayer()
    {
        view = Physics2D.Raycast(front.transform.position, -front.transform.right, viewDistance, LayerMask.GetMask("Player"));
        if (view.collider != null && state == EnemyState.IDLE)
            SetEnemyState(EnemyState.ATTACKING);
        // else if (view.collider == null && state == EnemyState.ATTACKING)
        // {
        //     state = EnemyState.IDLE;
        //     Debug.Log("Player lost!");
        // }
    }
}
