using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//this library is used for reading and writing data
using System.IO;

public class MainManager : MonoBehaviour
{
    #region My Variables
    //private Paddle player;
    private static string bestPlayer;
    private static int highScore;

    public Text CurrentPlayerName;
    public Text BestPlayerName;

    #endregion

    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    
    public GameObject GameOverText;
    
    private bool m_Started = false;
    private int m_Points;
    
    private bool m_GameOver = false;

    private void Awake()
    {
        LoadGame();
    }

    //I want to save the player's name, and their highscore if they got it
    [System.Serializable]
    class SaveData
    {
        //this is the data I want to save
        public string PlayerName;
        public int highScore;
    }

    // Start is called before the first frame update
    void Start()
    {
        //this is where the bricks are created in the scene
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

        CurrentPlayerName.text = PlayerDataHandler.Instance.PlayerName;

        SetBestPlayer();
    }

    private void Update()
    {
        if (!m_Started)
        {
            //this condition starts the game
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
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        //sets score to be saved?
        PlayerDataHandler.Instance.Score = m_Points;
        ScoreText.text = $"Score : {m_Points}";
    }

    public void GameOver()
    {
        m_GameOver = true;
        CheckBestPlayer();
        GameOverText.SetActive(true);
    }

    private void CheckBestPlayer()
    {
        int CurrentScore = PlayerDataHandler.Instance.Score;

        if(CurrentScore > highScore)
        {
            bestPlayer = PlayerDataHandler.Instance.PlayerName;
            highScore = CurrentScore;

            BestPlayerName.text = $"Best Score - {bestPlayer}: {highScore}";

            SaveGameRank(bestPlayer, highScore);
        }
    }

    private void SetBestPlayer()
    {
        //this function will set the name of the player with the highest score
        if(bestPlayer == null && highScore == 0)
        {
            BestPlayerName.text = "";
        }
        else
        {
            BestPlayerName.text = $"Best Score - {bestPlayer}: {highScore}";
        }
    }

    public void SaveGameRank(string bestPlayerName, int bestPlayerScore)
    {
        SaveData data = new SaveData();

        data.PlayerName = bestPlayerName;
        data.highScore = highScore;

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    public void LoadGame()
    {
        //this function will allow data to persist between sessions I believe
        string path = Application.persistentDataPath + "/savefile.json";

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            bestPlayer = data.PlayerName;
            highScore = data.highScore;
        }
    }
}
