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
        // 处于starPower状态下的马里奥是无敌效果的，所以，此时即使碰到敌人，敌人死亡而不是马里奥死亡
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
        /* 不加这个的话，如果一直移动然后再碰到敌人的话，死亡的动画只会出现一下，因为run.enabled = movement.running;禁用PlayerSpriteRenderer
         时，会使AnimatedSprite一直为true的状态*/
        //smallAnimated.enabled = false;
        bigRenderer.enabled = false;
        deathAnimation.enabled = true;

        // 死亡之后调用关卡重置方法
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
        // 将big转变成small
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
