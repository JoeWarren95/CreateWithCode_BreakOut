using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif


public class MenuUIHandler : MonoBehaviour
{
    public MainManager mainManager;
    
    // Start is called before the first frame update
    void Start()
    {
        mainManager.LoadHighScore();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //this function will be called whenever a new game is started
    public void StartNew()
    {
        //load scene 1 in build settings which is our game
        SceneManager.LoadScene(1);
    }

    //this function is called whenever we want to exit the game
    public void Exit()
    {
        //this line will save the user's last high score
        MainManager.Instance.SaveHighScore();

#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
        //this line will take us back to the main menu
        SceneManager.LoadScene(0);
#else
        Application.Quit();
        SceneManager.LoadScene(0);
#endif

    }
}
