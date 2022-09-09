using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif


public class MenuUIHandler : MonoBehaviour
{
    //public MainManager mainManager;
    //public string PlayerName;

    //variable for the player's name
    [SerializeField] Text PlayerNameInput;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    //this function will be called whenever a new game is started
    public void StartNew()
    {
        //load scene 1 in build settings which is our game
        SceneManager.LoadScene(1);
    }

    #region Save Data that may get deleted
/*    
    [System.Serializable]
    class SaveNameData
    {
        public string PlayerName;
    }


    public void SaveName()
    {
        
        //save the entered name
        if (Input.GetKeyDown(KeyCode.Return))
        {
            //take player back to the start menu
            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            

            //save the players name between scenes
            SaveNameData data = new SaveNameData();
            data.PlayerName = PlayerName;

            string json = JsonUtility.ToJson(data);

            File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
            SceneManager.LoadScene(1);
        }

        //SceneManager.LoadScene(SceneManager.LoadScene(1));

        //SaveName();

        //disable the gameobject
        //HighScoreText.gameObject.SetActive(false);
    }*/
    #endregion

    public void SetPlayerName()
    {
        PlayerDataHandler.Instance.PlayerName = PlayerNameInput.text;
    }

    //this function is called whenever we want to exit the game
    public void Exit()
    {

#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
        //this line will take us back to the main menu
        //SceneManager.LoadScene(0);
#else
        Application.Quit();
        //SceneManager.LoadScene(0);
#endif

    }
}
