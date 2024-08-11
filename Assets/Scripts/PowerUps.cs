using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUps : MonoBehaviour
{
    public enum Type
    {
        Coin,
        ExtraLife,
        MagicMushroom,
        Starpower,
        UnderGroundCoin
    }

    public Type type;
    public float frameRate = 1f / 6;

    private void Start()
    {
        if(type == Type.UnderGroundCoin)
        {
            InvokeRepeating("Rotate", frameRate, frameRate);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Collect(collision.gameObject);
            Destroy(gameObject);
        }
    }

    private void Collect(GameObject player)
    {
        switch (type)
        {
            case Type.Coin:
                GameManager.Instance.AddCoins();
                break;
            case Type.ExtraLife:
                GameManager.Instance.AddLifes();
                break;
            case Type.MagicMushroom:
                player.GetComponent<Player>().Grow();
                break;
            case Type.Starpower:
                player.GetComponent<Player>().StarPower();
                break;
            case Type.UnderGroundCoin:
                GameManager.Instance.AddCoins();
                break;
        }
    }

    private void Rotate()
    {
        if(transform.rotation.y >= 360)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        else
        {
            //transform.eulerAngles = new Vector3(0, transform.eulerAngles.y + 46, 0);
            transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y + 46, 0);
            // 这两个都行
        }
    }
}
