using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScoreScript : MonoBehaviour
{
    public GameObject Popup;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI HighscoreText;
    public TextMeshProUGUI CurrencyText;
    public int score;
    private int Highscore;
    private float timer;
    private double expectedScore = 0;
    public GameObject PacMan;

    public void Start()
    {
        Highscore = GetHighScore();
        SetHighScoreText(Highscore);
    }

    public void ScorePellet()
    {
        float popupChance = UnityEngine.Random.value;
        score += 10;
        scoreText.text = score.ToString();
        SetHighScoreText(score);
        if (popupChance < 0.01f)
        {
            Popup.SetActive(true);
            Popup.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().SetText("You need to pay " + Math.Ceiling(expectedScore) + " score to continue");
            PacMan.GetComponent<PacManMoveScript>().enabled = false;
            if (score >= expectedScore)
            {
                Popup.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>().SetText("Press SPACE to pay!");
            }
            else
            {
                Popup.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>().SetText("You don't have enough score!");
            }
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
        if (Popup.activeSelf == false)
        {
            if (score < 1000 && Popup.activeSelf == false)
            {
                expectedScore += 0.03 * Time.time;
            }

            if (score > 1000 && Popup.activeSelf == false)
            {
                expectedScore += 0.01 * Time.time;
            } 
        }
        else
        {
            timer += Time.deltaTime;
            if (score < expectedScore && timer > 5)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }

            if (timer % 1 > .5f)
            {
                Popup.transform.GetChild(2).gameObject.SetActive(false);
            }
            else
            {
                Popup.transform.GetChild(2).gameObject.SetActive(true);
            }
            if (Input.GetKeyDown("space") && score >= expectedScore)
            {
                score -= (int)Math.Ceiling(expectedScore);
                scoreText.text = score.ToString();
                Popup.SetActive(false);
                PacMan.GetComponent<PacManMoveScript>().enabled = true;
            }
        }
    }
}
