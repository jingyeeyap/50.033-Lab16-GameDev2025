using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManagerWeek5 : MonoBehaviour
{
    public void GameRestart()
    {
        foreach (Transform child in transform)
        {
            child.GetComponent<EnemyMovementWeek5>().GameRestart();
        }
    }
}
