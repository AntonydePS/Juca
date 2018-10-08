using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Portal : MonoBehaviour

{
    public GameObject gameOverPanel;
    public float startHP;
    private float health;
    public Image HPBar;


    // Use this for initialization
    void Start()
    {
        gameOverPanel.SetActive(false);
        health = startHP;
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            gameOverPanel.SetActive(true);
        }

    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Mini")
        {
            GameObject GC = GameObject.FindGameObjectWithTag("GameController");
            GC.SendMessage("PlayMiniHit");
            HPBar.fillAmount = health / startHP;
            health -= 10;
        }
    }
}