using UnityEngine;
using System.Collections;


[System.Serializable]
public class Game
{
    public static Game current;
    public GameStatus currentLvl;


    public Game()
    {
        currentLvl = new GameStatus();

    }

}