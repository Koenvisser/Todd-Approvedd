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

    public void Start()
    {
        Highscore = GetHighScore();
        SetHighScoreText(Highscore);
    }

    public void ScorePellet()
    {
        score += 10;
        scoreText.text = score.ToString();
        SetHighScoreText(score);
        SetCurrency(10);
    }

    public void ScorePowerPellet()
    {
        score += 50;
        scoreText.text = score.ToString();
        SetHighScoreText(score);
        SetCurrency(50);
    }

    public void ScoreGhost()
    {
        score += 100;
        scoreText.text = score.ToString();
        SetHighScoreText(score);
        SetCurrency(100);
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

    private void SetCurrency(int Currency)
    {
        Currency += PlayerPrefs.GetInt("Currency", 0);
        CurrencyText.SetText("Currency: " + Currency);
        PlayerPrefs.SetInt("Currency", Currency);
    }

    public void SetHighscore()
    {
        if (score > GetHighScore())
        {
            PlayerPrefs.SetInt("HighScore", score);
        }
    }
}
