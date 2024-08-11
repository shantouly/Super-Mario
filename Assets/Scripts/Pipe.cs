using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pipe : MonoBehaviour
{
    public Transform connection;
    public KeyCode enterKeyCode = KeyCode.S;
    public Vector3 enterDirection = Vector3.down;
    public Vector3 exitDirection = Vector3.zero;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(connection!=null && collision.CompareTag("Player"))
        {
            if (Input.GetKeyDown(enterKeyCode))
            {
                Debug.Log("1");
                StartCoroutine(Enter(collision.transform));
            }
        }
    }

    private IEnumerator Enter(Transform player)
    {
        
        player.GetComponent<PlayerMovement>().enabled = false;

        Vector3 enteredPosition = transform.position + enterDirection;
        Vector3 enteredScale = Vector3.one * 0.5f;

        yield return Move(player, enteredPosition, enteredScale);

        yield return new WaitForSeconds(1f);

        bool underground = connection.position.y < 0f;
        Camera.main.GetComponent<SideScrolling>().SetUnderground(underground);

        if(exitDirection != Vector3.zero)
        {
            // ����һ���ܵ�ͨ����һ���ܵ�
            Debug.Log(connection.position);
            player.position = connection.position - exitDirection;
            yield return Move(player, connection.position + exitDirection, Vector3.one);
        }
        else
        {
            player.position = connection.position;
            player.localScale = Vector3.one;
        }

        player.GetComponent<PlayerMovement>().enabled = true;
    }

    private IEnumerator Move(Transform player,Vector3 endPosition,Vector3 endScale)
    {
        float elapsed = 0f;
        float druation = 1f;

        Vector3 startPosition = player.position;
        Vector3 startScale = player.localScale;

        while(elapsed < druation)
        {
            float t = elapsed / druation;

            player.position = Vector3.Lerp(startPosition, endPosition, t);
            player.localScale = Vector3.Lerp(startScale, endScale, t);
            elapsed += Time.deltaTime;

            yield return null;
        }

        player.position = endPosition;
        player.localScale = endScale;
    }
}
