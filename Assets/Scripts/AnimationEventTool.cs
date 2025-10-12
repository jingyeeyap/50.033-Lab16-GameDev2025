using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimationEventTool : MonoBehaviour
{
    public int parameter;
    public UnityEvent<int> useInt;
    public UnityEvent use;
    GameManager gameManager;

    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("Manager").GetComponent<GameManager>();
    }

    public void TriggerIntEvent()
    {
        if (use == null)
        {
            gameManager.IncreaseScore(1);
        }
        else
        {
            useInt.Invoke(parameter);   // safe to invoke even without callbacks
        }

    }

    public void TriggerEvent()
    {
        use.Invoke();
    }

}