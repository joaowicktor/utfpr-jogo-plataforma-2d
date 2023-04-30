using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    [Header("Raycast Origins")]
    [SerializeField] protected GameObject front;
    [SerializeField] protected GameObject back;
    protected float xMovement = -1;
    protected Rigidbody2D rb { get { return GetComponent<Rigidbody2D>(); } }
    private RaycastHit2D hitBounds;
    private RaycastHit2D hitPlayerByFront;
    private RaycastHit2D hitPlayerByBack;
    private float collisionDistance = 0.2f;
    private ViewDirection viewDirection = ViewDirection.Left;
    private AudioManager audioManager;

    void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    protected void HandleEnemyCollisions()
    {
        if (GameManager.state != GameState.InGame)
            return;

        OnPlayerCollision();
        OnEnvironmentBoundsCollision();
    }

    private void OnPlayerCollision()
    {
        hitPlayerByFront = Physics2D.Raycast(
                    front.transform.position,
                    viewDirection == ViewDirection.Left ? -front.transform.right : front.transform.right,
                    collisionDistance,
                    LayerMask.GetMask("Player"));

        hitPlayerByBack = Physics2D.Raycast(
                    back.transform.position,
                    viewDirection == ViewDirection.Left ? back.transform.right : -back.transform.right,
                    collisionDistance,
                    LayerMask.GetMask("Player"));


        bool hasCollidedWithPlayer = hitPlayerByFront.collider != null || hitPlayerByBack.collider != null;

        if (hasCollidedWithPlayer)
        {
            GameObject player = hitPlayerByFront.collider != null ? hitPlayerByFront.collider.gameObject : hitPlayerByBack.collider.gameObject;
            audioManager.PlaySound(audioManager.death);
            Destroy(player);
            GameManager.GameOver();
        }
    }

    private void OnEnvironmentBoundsCollision()
    {
        hitBounds = Physics2D.Raycast(
                    front.transform.position,
                    viewDirection == ViewDirection.Left ? -front.transform.right : front.transform.right,
                    collisionDistance,
                    LayerMask.GetMask("EnemyBoundLimits"));

        bool isOnCollidableObject = hitBounds.collider != null;

        if (isOnCollidableObject)
            HorizontalFlip();
    }

    private void HorizontalFlip()
    {
        viewDirection = viewDirection == ViewDirection.Right ? ViewDirection.Left : ViewDirection.Right;
        xMovement *= -1;
        transform.Rotate(new Vector2(0, 180));
    }
}
