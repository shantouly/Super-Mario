using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockHit : MonoBehaviour
{
    public GameObject item;
    public Sprite emptySprite;
    public int maxHits = -1;
    private bool animating;         // 当方块在动画期间禁止对其再运行Hit方法
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 当方块最大的Hit为0时，禁止再播放动画了
        if (!animating &&  maxHits != 0 && collision.gameObject.CompareTag("Player"))
        {
            if (collision.transform.DotTest(transform, Vector2.up))
            {
                Hit();
            }
        }
    }

    private void Hit()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = true;
        maxHits--;

        if(maxHits == 0)
        {
            spriteRenderer.sprite = emptySprite;
        }

        if(item != null)
        {
            Instantiate(item, transform.position, Quaternion.identity);
        }

        StartCoroutine(Animate());
    }

    private IEnumerator Animate()
    {
        animating = true;
        Vector3 restingPosition = transform.localPosition;
        Vector3 animatedPosition = restingPosition + Vector3.up * 0.5f;

        yield return Move(restingPosition, animatedPosition);
        yield return Move(animatedPosition, restingPosition);

        animating = false;
    }

    private IEnumerator Move(Vector3 from, Vector3 to)
    {
        float elapsed = 0f;
        float druation = 0.125f;

        while(elapsed < druation)
        {
            float t = elapsed / druation;
            // 是一个线性的，给一个t（比例），返回从a到b这个线性上的那个t点的值
            transform.localPosition = Vector3.Lerp(from, to, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = to;
    }
}

