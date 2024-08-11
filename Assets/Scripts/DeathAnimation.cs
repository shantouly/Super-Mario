using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathAnimation : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Sprite deadSprite;

    private void Reset()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        UpdateSprite();
        DisablePhysics();
        StartCoroutine(Animate());
    }

    private void UpdateSprite()
    {
        spriteRenderer.enabled = true;
        spriteRenderer.sortingOrder = 10;           // ȷ������������/���˵����ʱ���ܹ������ڵذ��ǰ�棬���ᱻ����

        if(deadSprite != null)
        {
            spriteRenderer.sprite = deadSprite;
        }
    }

    private void DisablePhysics()
    {
        Collider2D[] colliders = GetComponents<Collider2D>();

        foreach(Collider2D collider in colliders)
        {
            collider.enabled = false;
        }

        GetComponent<Rigidbody2D>().isKinematic = true;

        PlayerMovement playerMovement = GetComponent<PlayerMovement>();
        EnemyMovement enemyMovement = GetComponent<EnemyMovement>();

        if (playerMovement != null)
        {
            playerMovement.enabled = false;
        }
        if(enemyMovement != null)
        {
            enemyMovement.enabled = false;
        }
    }

    private IEnumerator Animate()
    {
        float elapsed = 0f;
        float druation = 3f;

        float jumpVelocity = 10f;
        float gravity = -36f;

        Vector3 velocity = Vector3.up * jumpVelocity;

        while(elapsed < druation)
        {
            // �൱���Ǹ���һ�����ٶ�
            transform.position += velocity * Time.deltaTime;
            velocity.y += gravity * Time.deltaTime;
            elapsed += Time.deltaTime;
            yield return null;
        }
    }
}
