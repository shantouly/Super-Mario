using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagPole : MonoBehaviour
{
    public Transform flag;
    public Transform poleBottom;
    public Transform castle;
    public float speed = 6f;
    public int world = 1;
    public int stage = 1;
    public bool isFinish { get; private set; }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartCoroutine(MoveTo(flag, poleBottom.position));
            StartCoroutine(LevelCompeteSequence(collision.transform));
        }
    }

    private IEnumerator LevelCompeteSequence(Transform player)
    {
        isFinish = true;
        player.GetComponent<PlayerMovement>().enabled = false;

        yield return MoveTo(player, poleBottom.position);
        yield return MoveTo(player, poleBottom.position + Vector3.right);
        yield return MoveTo(player, poleBottom.position + Vector3.right + Vector3.down + Vector3.right * 0.5f);
        yield return MoveTo(player, castle.position);

        player.gameObject.SetActive(false);

        isFinish = false;

        yield return new WaitForSeconds(2f);
        GameManager.Instance.LoadLevel(world, stage);
    }

    private IEnumerator MoveTo(Transform subject,Vector3 destination)
    {
        while(Vector3.Distance(subject.position,destination) > 0.125f)
        {
            subject.position = Vector3.MoveTowards(subject.position, destination, speed * Time.deltaTime);
            yield return null;
        }

        subject.position = destination;
    }
}
