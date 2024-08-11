using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockCoin : MonoBehaviour
{
    private void Start()
    {
        GameManager.Instance.AddCoins();
        StartCoroutine(Animate());
    }

    private IEnumerator Animate()
    {
        Vector3 restingPosition = transform.localPosition;
        Vector3 animatePosition = transform.localPosition + Vector3.up * 2f;

        yield return Move(restingPosition, animatePosition);
        yield return Move(animatePosition, restingPosition);
    }

    private IEnumerator Move(Vector3 from,Vector3 to)
    {
        float elapsed = 0f;
        float druation = 0.25f;

        while(elapsed < druation)
        {
            float t = elapsed / druation;
            transform.localPosition = Vector3.Lerp(from, to, t);
            elapsed += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = to;
    }
}
