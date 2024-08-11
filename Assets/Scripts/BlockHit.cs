using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockHit : MonoBehaviour
{
    public GameObject item;
    public Sprite emptySprite;
    public int maxHits = -1;
    private bool animating;         // �������ڶ����ڼ��ֹ����������Hit����
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // ����������HitΪ0ʱ����ֹ�ٲ��Ŷ�����
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
            // ��һ�����Եģ���һ��t�������������ش�a��b��������ϵ��Ǹ�t���ֵ
            transform.localPosition = Vector3.Lerp(from, to, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = to;
    }
}

