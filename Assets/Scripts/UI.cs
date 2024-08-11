using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    public Text CoinsCount;

    private void Update()
    {
        CoinsCount.text = GameManager.Instance.coins.ToString();
    }
}
