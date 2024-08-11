using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpriteRenderer : MonoBehaviour
{
    public SpriteRenderer spriteRenderer { get; private set; }
    public FlagPole flagPole;
    private PlayerMovement movement;

    public Sprite idle;
    public Sprite jump;
    public Sprite slide;
    public AnimatedSprite run;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        movement = GetComponentInParent<PlayerMovement>();
    }

    private void OnEnable()
    {
        spriteRenderer.enabled = true;
    }

    private void OnDisable()
    {
        run.enabled = false;
        spriteRenderer.enabled = false;
    }

    private void LateUpdate()
    {
        run.enabled = movement.running || flagPole.isFinish;
        if (movement.jumping)
        {
            spriteRenderer.sprite = jump;
        }else if (movement.sliding)
        {
            spriteRenderer.sprite = slide;
        }else if(!movement.running && !flagPole.isFinish)
        {
            spriteRenderer.sprite = idle;
        }
    }
}
