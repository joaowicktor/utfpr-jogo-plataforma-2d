using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState
{
    MainMenu,
    InGame,
    Paused,
    GameOver,
    Victory
}

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    public static GameState state = GameState.InGame;
    public static int coinsCollected = 0;

    private static AudioManager audioManager;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        DontDestroyOnLoad(gameObject);
    }

    private void DisableCursor()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public static void CollectCoin()
    {
        coinsCollected++;
        audioManager.PlaySound(audioManager.coin);
    }

    public static void ResetCoins()
    {
        coinsCollected = 0;
    }

    public static void Pause()
    {
        Time.timeScale = 0;
        state = GameState.Paused;
        SceneManager.LoadSceneAsync("PauseScene", LoadSceneMode.Additive);
    }

    public static void Resume()
    {
        Time.timeScale = 1;
        state = GameState.InGame;
        SceneManager.UnloadSceneAsync("PauseScene");
    }

    public static void Victory()
    {
        audioManager.PlaySound(audioManager.victory);
        Time.timeScale = 0;
        state = GameState.Victory;
        SceneManager.LoadSceneAsync("VictoryScene", LoadSceneMode.Additive);
    }

    public static void GameOver()
    {
        Time.timeScale = 0;
        state = GameState.GameOver;
        SceneManager.LoadSceneAsync("GameOverScene", LoadSceneMode.Additive);
    }

    public static void StartGame()
    {
        ResetCoins();
        Time.timeScale = 1;
        state = GameState.InGame;
        SceneManager.LoadSceneAsync("GameScene", LoadSceneMode.Single);
    }

    public static void ExitToMainMenu()
    {
        Time.timeScale = 1;
        state = GameState.MainMenu;
        SceneManager.LoadSceneAsync("MainMenuScene", LoadSceneMode.Single);
    }

    public static void ExitGame()
    {
        Application.Quit();
    }

    public static Vector3 GetMainCameraPosition()
    {
        return Camera.main.transform.position;
    }
}
