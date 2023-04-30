using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovablePlatformScript : MonoBehaviour
{
    private Vector2 initialPosition;
    private float timeCount = 0f;

    [Header("Movement")]
    public float x = 1f;
    public float y = 1f;

    void Start()
    {
        initialPosition = transform.position;
    }

    void Update()
    {
        timeCount += Time.deltaTime;

        float positionX = Mathf.Cos(timeCount) * x;
        float positionY = Mathf.Sin(timeCount) * y;

        transform.position = initialPosition + new Vector2(positionX, positionY);
    }
}
