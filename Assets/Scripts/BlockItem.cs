using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockItem : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(Animate());
    }

    private IEnumerator Animate()
    {
        Rigidbody2D rigidbody2D = GetComponent<Rigidbody2D>();
        CircleCollider2D physicsCollider = GetComponent<CircleCollider2D>();
        BoxCollider2D triggerCollider = GetComponent<BoxCollider2D>();
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        EnemyMovement movement = GetComponent<EnemyMovement>();

        rigidbody2D.isKinematic = true;
        physicsCollider.enabled = false;
        triggerCollider.enabled = false;
        spriteRenderer.enabled = false;
        movement.enabled = false;

        yield return new WaitForSeconds(0.25f);

        spriteRenderer.enabled = true;

        float elapsed = 0f;
        float druation = 0.5f;

        Vector3 startPosition = transform.localPosition;
        Vector3 endPosition = transform.localPosition + Vector3.up;

        while(elapsed < druation)
        {
            float t = elapsed / druation;
            transform.localPosition = Vector3.Lerp(startPosition, endPosition, t);
            elapsed += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = endPosition;

        rigidbody2D.isKinematic = false;
        physicsCollider.enabled = true;
        triggerCollider.enabled = true;
        movement.enabled = true;
    }
}
