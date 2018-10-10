using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeDetect : MonoBehaviour {

    public EnemyAI enemyAIScript;

    
    void OnTriggerEnter(Collider playerange)
    {
        if (playerange.tag == "Player")
        {
            GameObject enemyAI = GameObject.FindGameObjectWithTag("Mini");
            enemyAI.SendMessage("playerRangeTrue");
        }
    }

    void OnTriggerExit(Collider playerange)
    {
        if (playerange.tag == "Player")
        {
            GameObject enemyAI = GameObject.FindGameObjectWithTag("Mini");
            enemyAI.SendMessage("playerRangeFalse");
        }
    }

}
