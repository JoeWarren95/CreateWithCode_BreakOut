using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

public class MainManager : MonoBehaviour
{
    //make MainManager into a Singleton
    public static MainManager Instance;

    #region My Variables
    private Paddle player;
    public string PlayerName;

    public int highScore;

    //public GameObject HighScoreText;

    private bool isNewHighScore;

    #endregion
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public GameObject GameOverText;
    
    private bool m_Started = false;
    private int m_Points;
    
    private bool m_GameOver = false;

    #region My code additions
    #region notes
    //step 1 - get game to recognize when a new highscore has been hit
    //step 2 - get game to save that score between sessions
    //          - input field has to appear and allow user to enter their name
    //          - input field has to disappear once player has entered their name
    //step 3 - allow player to input their name if they get a highscore
    //          - get input field to appear
    //          - get input field to accept/save input
    //step 4 - build
    #endregion
    //add this to check there are no other main managers in the scene
    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        //HighScoreText.gameObject.SetActive(false);
        //call LoadHighScore to maintain high score between sessions
        LoadHighScore();
    }

    //I want to save the player's name, and their highscore if they got it
    [System.Serializable]
    class SaveData
    {
        //this is the data I want to save
        public string PlayerName;
        public int highScore;
    }

    public void SaveName()
    {
        //save the players name between scenes
        SaveData data = new SaveData();
        data.PlayerName = PlayerName;

        string json = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    public void LoadName()
    {
        string path = Application.persistentDataPath + "/savefile.json";

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);

            SaveData data = JsonUtility.FromJson<SaveData>(json);

            PlayerName = data.PlayerName;
        }
    }

    public void SaveHighScore()
    {
        SaveData data = new SaveData();
        data.highScore = highScore;

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    public void LoadHighScore()
    {
        string path = Application.persistentDataPath + "/savefile.json";

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);

            SaveData data = JsonUtility.FromJson<SaveData>(json);
            highScore = data.highScore;
        }
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        //get reference to our paddle
        player = GameObject.Find("Paddle").GetComponent<Paddle>();

        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);
        
        int[] pointCountArray = new [] {1,1,2,2,5,5};
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }

        #region Adding check here for new high score
        if(m_Points > highScore)
        {
            highScore = m_Points;
            isNewHighScore = true;

            PlayerPrefs.SetInt("Highscore", highScore);
        }

        //we need to check if there was a new high score first
        //if there was a new high score, set different text active
        //also set text box active for player to enter their name
        else if (isNewHighScore && m_GameOver)
        {
            Debug.Log("Your high score was " + highScore + "! Good Job!!");
            #region Enter name comments
            //HighScoreText.gameObject.SetActive(true);
            //disable the character controller

            /*if (Input.GetKeyDown(KeyCode.Return))
            {
                //save the entered name
                SaveName();
                //disable the gameobject
                HighScoreText.gameObject.SetActive(false);
                //take player back to the start menu
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }*/
            #endregion
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
       
        else if (m_GameOver && !isNewHighScore)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
        #endregion
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Score : {m_Points}";
    }

    public void GameOver()
    {
        m_GameOver = true;
        GameOverText.SetActive(true);
    }

    #region Button Code
    public void StartNew()
    {

    }

    public void Exit()
    {
        
    }
    #endregion
}
