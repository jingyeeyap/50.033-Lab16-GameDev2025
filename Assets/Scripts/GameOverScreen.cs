using UnityEngine;
using TMPro;

public class GameOverScreen : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public JumpOverGoomba jumpOverGoomba;

    void OnEnable()
    {
        scoreText.text = jumpOverGoomba.scoreText.text;
    }
}
