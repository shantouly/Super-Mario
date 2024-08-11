using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Koopa : MonoBehaviour
{
    public Sprite ShellSprite;

    private float shellSpeed = 12f;
    private bool shelled;
    private bool pushed;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!shelled && collision.gameObject.CompareTag("Player"))
        {
            Player player = collision.gameObject.GetComponent<Player>();
            if (collision.transform.DotTest(transform, Vector2.down))
            {
                EnterShell();
            }else if (player.starPower)
            {
                Hit();
            }else
            {
                player.Hit();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(shelled && collision.gameObject.CompareTag("Player"))
        {
            Player player = collision.gameObject.GetComponent<Player>();
            if (!pushed)
            {
                Vector2 direction = new Vector2(transform.position.x - collision.transform.position.x, 0);
                PushShell(direction);
            }else if (player.starPower)
            {
                Hit();
            } else 
            {
                player.Hit();
            }
        }else if(!shelled && collision.gameObject.layer == LayerMask.NameToLayer("Shell"))
        {
            Hit();
        }
    }

    private void EnterShell()
    {
        shelled = true;
        GetComponent<EnemyMovement>().enabled = false;
        GetComponent<AnimatedSprite>().enabled = false;
        GetComponent<SpriteRenderer>().sprite = ShellSprite;
    }

    private void PushShell(Vector2 direction)
    {
        pushed = true;

        EnemyMovement movement = GetComponent<EnemyMovement>();
        movement.direction = direction.normalized;
        movement.speed = shellSpeed;
        movement.enabled = true;

        gameObject.layer = LayerMask.NameToLayer("Shell");
    }

    private void Hit()
    {
        GetComponent<AnimatedSprite>().enabled = false;
        GetComponent<DeathAnimation>().enabled = true;
        Destroy(gameObject, 3f);
    }

    private void OnBecameInvisible()
    {
        if (pushed)
        {
            Destroy(gameObject);
        }
    }
}
