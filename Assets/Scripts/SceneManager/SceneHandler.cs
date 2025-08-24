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

    public Dictionary<int, string> GameSceneIndexAndNames
    { get; private set; } = new Dictionary<int, string>();

    public event Action<int> SceneTransition;

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

        // Get a dictionary of all the scenes in the build and assgin the name to the build index. 
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            GameSceneIndexAndNames.Add(i, SceneManager.GetSceneByBuildIndex(i).name);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
        // Reset the time scale on scene change.
        Time.timeScale = 1;

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
