using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneCameraScript : MonoBehaviour
{
    void Start()
    {
        transform.position = GameManager.GetMainCameraPosition();
    }
}
