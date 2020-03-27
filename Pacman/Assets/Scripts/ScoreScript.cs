using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreScript : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI HighscoreText;
    public TextMeshProUGUI CurrencyText;
    public int score;
    private int Highscore;
    public double expectedScore = 0;

    public void Start()
    {
        Highscore = GetHighScore();
        SetHighScoreText(Highscore);
    }

    public void ScorePellet()
    {
        float popupChance = Random.value;
        score += 10;
        scoreText.text = score.ToString();
        SetHighScoreText(score);
        if (popupChance < 0.01f)
        {
            //popup callen
        }
    }

    public void ScorePowerPellet()
    {
        score += 50;
        scoreText.text = score.ToString();
        SetHighScoreText(score);
    }

    public void ScoreGhost()
    {
        score += 100;
        scoreText.text = score.ToString();
        SetHighScoreText(score);
    }

    private int GetHighScore()
    {
        return PlayerPrefs.GetInt("HighScore", 0);
    }

    private void SetHighScoreText(int highscore)
    {
        if (highscore >= Highscore)
        {
            HighscoreText.SetText("High Score: " + highscore);
            Highscore = highscore;
        }
    }

    public void SetHighscore()
    {
        if (score > GetHighScore())
        {
            PlayerPrefs.SetInt("HighScore", score);
        }
    }
    void Update()
    {
        if (score < 1000)
        {
            expectedScore += 0.03 * Time.time;
        }

        if (score > 1000)
        {
            expectedScore += 0.01 * Time.time;
        }

    }
}
