using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCommands : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            HandleEscapeKeyCommand();
    }

    private void HandleEscapeKeyCommand()
    {
        switch (GameManager.state)
        {
            case GameState.Paused:
                GameManager.Resume();
                break;
            case GameState.InGame:
                GameManager.Pause();
                break;
            default:
                break;
        }
    }
}
