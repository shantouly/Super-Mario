using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerSpriteRenderer smallRenderer;
    public PlayerSpriteRenderer bigRenderer;
    private PlayerSpriteRenderer activeRenderer;
    private CapsuleCollider2D capusleCollider;

    private DeathAnimation deathAnimation;

    public bool big => bigRenderer.enabled;
    public bool small => smallRenderer.enabled;
    public bool dead => deathAnimation.enabled;
    public bool starPower { get; private set; }

    private void Awake()
    {
        deathAnimation = GetComponent<DeathAnimation>();
        capusleCollider = GetComponent<CapsuleCollider2D>();
        activeRenderer = smallRenderer;
    }

    public void Hit()
    {
        // ����starPower״̬�µ���������޵�Ч���ģ����ԣ���ʱ��ʹ�������ˣ������������������������
        if (!starPower)
        {
            if (big)
            {
                Shrink();
            }
            else
            {
                Death();
            }
        }
    }

    private void Death()
    {
        smallRenderer.enabled = false;
        /* ��������Ļ������һֱ�ƶ�Ȼ�����������˵Ļ��������Ķ���ֻ�����һ�£���Ϊrun.enabled = movement.running;����PlayerSpriteRenderer
         ʱ����ʹAnimatedSpriteһֱΪtrue��״̬*/
        //smallAnimated.enabled = false;
        bigRenderer.enabled = false;
        deathAnimation.enabled = true;

        // ����֮����ùؿ����÷���
        GameManager.Instance.ResetLevel(3f);
    }

    public void Grow()
    {
        smallRenderer.enabled = false;
        bigRenderer.enabled = true;
        activeRenderer = bigRenderer;

        capusleCollider.size = new Vector2(1f, 2f);
        capusleCollider.offset = new Vector2(0, 0.5f);

        StartCoroutine(ScaleAnimation());
    }

    private void Shrink()
    {
        // ��bigת���small
        smallRenderer.enabled = true;
        bigRenderer.enabled = false;
        activeRenderer = smallRenderer;

        capusleCollider.size = new Vector2(1f, 1f);
        capusleCollider.offset = new Vector2(0, 0);

        StartCoroutine(ScaleAnimation());
    }

    private IEnumerator ScaleAnimation()
    {
        float elapsed = 0f;
        float druation = 0.5f;

        while(elapsed < druation)
        {
            elapsed += Time.deltaTime;
            if(Time.frameCount % 4 == 0)
            {
                smallRenderer.enabled = !smallRenderer.enabled;
                bigRenderer.enabled = !smallRenderer.enabled;
            }
            yield return null;
        }

        smallRenderer.enabled = false;
        bigRenderer.enabled = false;

        activeRenderer.enabled = true;
    }

    public void StarPower(float druation = 10f)
    {
        StartCoroutine(StarpowerAnimatation(druation));
    }

    private IEnumerator StarpowerAnimatation(float druation)
    {
        starPower = true;
        float elapsed = 0f;

        while(elapsed < druation)
        {
            elapsed += Time.deltaTime;

            if(Time.frameCount % 4 == 0)
            {
                activeRenderer.spriteRenderer.color = Random.ColorHSV(0f, 1f, 1f, 1f, 1f, 1f);
            }
            yield return null;
        }

        activeRenderer.spriteRenderer.color = Color.white;

        starPower = false;
    }
}
