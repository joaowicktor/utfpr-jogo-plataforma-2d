using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIScript : MonoBehaviour
{
    private TMP_Text coinCountText;

    void Start()
    {
        coinCountText = GameObject.Find("UI/CollectedCoins").GetComponent<TMP_Text>();
    }

    void Update()
    {
        coinCountText.text = GameManager.coinsCollected.ToString();
    }
}
