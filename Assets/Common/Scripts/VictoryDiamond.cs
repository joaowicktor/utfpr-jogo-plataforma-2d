using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryDiamond : MonoBehaviour
{
    void Update()
    {
        transform.Rotate(Vector3.up * Time.deltaTime * 100f);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameManager.Victory();
        }
    }
}
