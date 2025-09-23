using UnityEngine;
using TMPro;

public class GameOverScreen : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public JumpOverGoomba jumpOverGoomba;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnEnable()
    {
        scoreText.text = jumpOverGoomba.scoreText.text;
    }
}
