using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBarrier : MonoBehaviour
{
    // 当马里奥或者是敌人掉到坑里的时候，死亡，并且如果是马里奥的话要重置游戏。
    // 当两个游戏物体都有collider的时候，有一方是trigger的话那么两者都能调用该方法来判断对方
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.SetActive(false);
            GameManager.Instance.ResetLevel(3f);
        }
        else
        {
            Destroy(collision.gameObject);
        }
    }
}
