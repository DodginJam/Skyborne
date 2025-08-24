using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

[DefaultExecutionOrder(-501)]
public class SceneHandler : MonoBehaviour
{
    public static SceneHandler Instance
    { get; private set; }

    public event Action<int> SceneTransition;

    public event Action<bool> ScenePause;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        // Initialise the starting music.
        SceneTransition?.Invoke(SceneManager.GetActiveScene().buildIndex);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PauseScene()
    {
        if (Time.timeScale != 0)
        {
            Time.timeScale = 0;
        }

        ScenePause?.Invoke(true);
    }

    public void ResumeScene()
    {
        if (Time.timeScale != 1)
        {
            Time.timeScale = 1;
        }

        ScenePause?.Invoke(false);
    }

    public void LoadScene(int newSceneIndex)
    {
        // Reset the time scale on scene change.
        Time.timeScale = 1;

        SceneManager.LoadScene(newSceneIndex);

        SceneTransition?.Invoke(newSceneIndex);
    }

    /// <summary>
    /// Quit the game depending on the game running envionment.
    /// </summary>
    public void QuitGame()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void RestartScene()
    {
        LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
