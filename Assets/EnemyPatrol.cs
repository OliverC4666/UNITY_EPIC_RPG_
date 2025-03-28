using UnityEngine;
using System.Collections;

public class EnemyAI_WithColliders : EnemyAI
{
    public BoxCollider2D FeetRunWalk;
    public CapsuleCollider2D FeetIDLE;



    private new void Start()
    {
        base.Start();
        StartCoroutine(ColliderCheckRoutine());
    }

    private IEnumerator ColliderCheckRoutine()
    {
        while (true) // Keep running indefinitely
        {
            bool isMoving = (GetIsChasing() || anim.GetBool("IsWalking")) || GetIsAttacking() ;

            // Switch colliders based on movement state
            FeetIDLE.enabled = !isMoving;
            FeetRunWalk.enabled = isMoving;

            yield return new WaitForSeconds(0.1f); // Adjust collider check interval
        }
    }
}
