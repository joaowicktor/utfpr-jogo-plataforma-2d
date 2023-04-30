using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class VictorySceneScript : MonoBehaviour
{
    void Start()
    {
        TMP_Text coinsCollectedText = GameObject.Find("Statistics").GetComponent<TMP_Text>();
        coinsCollectedText.text = $"You have collected {GameManager.coinsCollected} coins!";
    }
}
