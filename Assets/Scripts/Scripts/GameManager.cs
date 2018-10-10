using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance = null;

    public GameObject winner;
    public GameObject gameOver;


    void Awake()
    {


        if (Instance == null)
        {
            Instance = this;
        }

        else if (Instance != this)
        {
            Destroy(gameObject);
        }

    }

    public PlayerController Player;



    public void IvokeGameOver()
    {
        Invoke("GameOver", 5);
    }

    public void GameOver()
    {
        gameOver.SetActive(true);
        Time.timeScale = 0f;
    }

    public void YoureWinner()
    {
        winner.SetActive(true);
        Time.timeScale = 0f;
    }
}
