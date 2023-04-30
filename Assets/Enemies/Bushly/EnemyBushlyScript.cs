using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBushlyScript : EnemyScript
{
    void Update()
    {
        rb.velocity = new Vector2(xMovement * 2f, rb.velocity.y);
        HandleEnemyCollisions();
    }
}
