using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBarrier : MonoBehaviour
{
    // ������»����ǵ��˵��������ʱ���������������������µĻ�Ҫ������Ϸ��
    // ��������Ϸ���嶼��collider��ʱ����һ����trigger�Ļ���ô���߶��ܵ��ø÷������ж϶Է�
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
