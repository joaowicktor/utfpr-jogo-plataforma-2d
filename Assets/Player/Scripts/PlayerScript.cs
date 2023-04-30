using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Timers;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerScript : MonoBehaviour
{
    [Header("Player Movement Settings")]
    [SerializeField] private float speed = 5;
    [SerializeField] private float jumpForce = 12;

    [Header("Raycast Origins")]
    [SerializeField] private GameObject head;
    [SerializeField] private GameObject foot;

    private Rigidbody2D rb;
    private Animator anim;
    private ViewDirection viewDirection = ViewDirection.Right;
    private RaycastHit2D hitGround;
    private RaycastHit2D hitSecondGroundWithHead;
    private RaycastHit2D hitSecondGroundWithFoot;
    private RaycastHit2D hitEnemy;
    private AudioManager audioManager;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    void Update()
    {
        if (GameManager.state != GameState.InGame)
            return;

        ControlPlayerOnInput();
        OnGroundCollision();
        OnEnemyCollision();
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Resource")
            RemoveCoinTilemap(collision);
    }

    private void RemoveCoinTilemap(Collider2D collision)
    {
        Tilemap map = collision.GetComponentInParent<Tilemap>();
        Vector3Int removePos = map.WorldToCell(transform.position);
        map.SetTile(removePos, null);
    }

    private void ControlPlayerOnInput()
    {
        float x = Input.GetAxis("Horizontal");
        HandlePlayerMovement(x);
        HandlePlayerHorizontalFlip(x);

        if (Input.GetKeyDown(KeyCode.Space))
            Jump();
    }

    private void SetPlayerState(PlayerState state)
    {
        anim.SetInteger("PlayerState", (int)state);
    }

    private void OnGroundCollision()
    {
        hitGround = Physics2D.Raycast(
                    foot.transform.position,
                    -foot.transform.up,
                    0.1f,
                    LayerMask.GetMask("Ground"));

        bool isOnCollidableObject = hitGround.collider != null;
        transform.parent = isOnCollidableObject ? hitGround.collider.transform : null;

        if (!isOnCollidableObject)
        {
            hitSecondGroundWithHead = Physics2D.Raycast(
                        head.transform.position,
                        head.transform.up,
                        0.1f,
                        LayerMask.GetMask("Second Ground"));


            bool isOnCollidableObjectWithHead = hitSecondGroundWithHead.collider != null;
            if (isOnCollidableObjectWithHead)
                StartCoroutine(IgnoreSecondGroundCollisionTemporarily(hitSecondGroundWithHead.collider));

            hitSecondGroundWithFoot = Physics2D.Raycast(
                        foot.transform.position,
                        -foot.transform.up,
                        0.1f,
                        LayerMask.GetMask("Second Ground"));

            bool isOnSecondCollidableObject = hitSecondGroundWithFoot.collider != null;
            transform.parent = isOnSecondCollidableObject ? hitSecondGroundWithFoot.collider.transform : null;
        }

    }

    private IEnumerator IgnoreSecondGroundCollisionTemporarily(Collider2D collider)
    {
        Physics2D.IgnoreCollision(collider, GetComponent<Collider2D>());

        yield return new WaitForSeconds(0.5f);

        Physics2D.IgnoreCollision(collider, GetComponent<Collider2D>(), false);
    }

    private void OnEnemyCollision()
    {
        hitEnemy = Physics2D.Raycast(
                    foot.transform.position,
                    -foot.transform.up,
                    0.1f,
                    LayerMask.GetMask("Enemy"));

        bool hasCollidedWithEnemy = hitEnemy.collider != null;

        if (hasCollidedWithEnemy)
        {
            Destroy(hitEnemy.collider.gameObject);
            rb.velocity = new Vector2(rb.velocity.x, 8f);
            audioManager.PlaySound(audioManager.hit);
        }
    }

    private void HandlePlayerMovement(float horizontalInput)
    {
        rb.velocity = new Vector2(horizontalInput * speed, rb.velocity.y);

        bool isRunning = horizontalInput != 0;
        bool isFallingByGravity = isRunning && rb.velocity.y == 0 && !hitGround.collider && !hitSecondGroundWithFoot.collider;
        bool isFalling = rb.velocity.y < 0 && !hitGround.collider && !hitSecondGroundWithFoot.collider;

        switch (true)
        {
            case var _ when isFallingByGravity:
                SetPlayerState(PlayerState.FALLING_BY_GRAVITY);
                break;
            case var _ when isRunning && !isFalling:
                SetPlayerState(PlayerState.RUNNING);
                break;
            case var _ when isFalling:
                SetPlayerState(PlayerState.FALLING);
                break;
            default:
                SetPlayerState(PlayerState.IDLE);
                break;
        }
    }

    private void HandlePlayerHorizontalFlip(float horizontalInput)
    {
        bool isViewDirectionRight = viewDirection == ViewDirection.Right;
        bool shouldFlipToLeft = horizontalInput < 0 && isViewDirectionRight;
        bool shouldFlipToRight = horizontalInput > 0 && !isViewDirectionRight;

        if (shouldFlipToLeft || shouldFlipToRight)
        {
            viewDirection = isViewDirectionRight ? ViewDirection.Left : ViewDirection.Right;
            transform.Rotate(new Vector2(0, 180));
        }
    }
    private void Jump()
    {
        bool isJumping = !hitGround.collider && !hitSecondGroundWithFoot.collider;
        SetPlayerState(PlayerState.JUMPING);

        if (!isJumping)
        {
            audioManager.PlaySound(audioManager.jump);
            rb.velocity = new Vector2(0, jumpForce);
        }
    }
}
