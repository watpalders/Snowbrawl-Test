using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

[System.Serializable]
public class GameStatus
{

    public string name;

    public GameStatus()
    {
        int activeScene = SceneManager.GetActiveScene().buildIndex;
    }
}