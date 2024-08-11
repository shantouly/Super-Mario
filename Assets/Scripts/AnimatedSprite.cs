using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatedSprite : MonoBehaviour
{
    public Sprite[] sprites;
    public float frameRate = 1f / 6;

    private SpriteRenderer spriteRenderer;
    private int frame = 0;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        InvokeRepeating("Animate", frameRate, frameRate);
    }

    private void OnDisable()
    {
        CancelInvoke();
    }

    private void Animate()
    {
        if(frame >=0 && frame < sprites.Length)
        {
            spriteRenderer.sprite = sprites[frame];
        }else if(frame >= sprites.Length)
        {
            frame = -1;
        }

        frame++;
    }
}
