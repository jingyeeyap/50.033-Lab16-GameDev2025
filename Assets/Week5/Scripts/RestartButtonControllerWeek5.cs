using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RestartButtonControllerWeek5 : MonoBehaviour
{
    public UnityEvent gameRestart;

    public void ButtonClick()
    {
        gameRestart.Invoke();
    }

}
