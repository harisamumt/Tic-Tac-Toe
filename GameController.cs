using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


[System.Serializable]
public class Player
{
    public Image panel;
    public Text text;
    public Button button;
}
[System.Serializable]
public class PlayerColor
{
    public Color panelColor;
    public Color textColor;
}

public class GameController : MonoBehaviour
{
    public Text[] buttonList;
    private string playerSide;

    public GameObject gameOverPanel;
    public Text gameOverText;

    private int moveCount;

    public GameObject restartButton;

    public Player playerX;
    public Player playerO;
    public PlayerColor activePlayerColor;
    public PlayerColor inactivePlayerColor;
    public Text chooseSideText;
    public GameObject quitButton;
    public GameObject quitConfirmationPanel;
    public Button yesButton;
    public Button noButton;
    private int gamesPlayed;
    private int xWins;
    private int oWins;
    private int totalGamesToPlay = 5;
    private int gamesToWin = 3;
    public Text playerXScoreText;
    public Text playerOScoreText;
    private bool gameStarted = false;
    private bool matchOver = false;

    



    void Awake()
    {

        gameOverPanel.SetActive(false);
        SetGameControllerReferenceOnButtons();

        moveCount = 0;
        restartButton.SetActive(false);
        chooseSideText.gameObject.SetActive(true);
        quitButton.SetActive(false);
        quitConfirmationPanel.SetActive(false);
        quitButton.SetActive(true);

    }

    void SetGameControllerReferenceOnButtons()
    {
        for (int i = 0; i < buttonList.Length; i++)
        {
            buttonList[i].GetComponentInParent<GridSpace>().SetGameControllerReference(this);
        }
    }

    public void SetStartingSide(string startingSide)
    {
        if (!gameStarted)
        {
            playerSide = startingSide;
            if (playerSide == "X")
            {
                SetPlayerColors(playerX, playerO);
            }
            else
            {
                SetPlayerColors(playerO, playerX);
            }

            chooseSideText.gameObject.SetActive(false);
            StartGame();
            gameStarted = true;
        }
    }
    void StartGame()
    {
        SetBoardInteractable(true);
    }

    public string GetPlayerSide()
    {
        return playerSide;
    }

    void UpdateScoreDisplay()
    {
        playerXScoreText.text = "Player X Score: " + xWins;
        playerOScoreText.text = "Player O Score: " + oWins;
    }

    void Start()
    {
        // Initialize the score display
        UpdateScoreDisplay();
    }

    public void EndTurn()
    {

        moveCount++;

        if (buttonList[0].text == playerSide && buttonList[1].text == playerSide && buttonList[5].text == playerSide)
        {
            GameOver(playerSide);
        }

        else if (buttonList[2].text == playerSide && buttonList[3].text == playerSide && buttonList[8].text == playerSide)
        {
            GameOver(playerSide);
        }

        else if (buttonList[6].text == playerSide && buttonList[7].text == playerSide && buttonList[4].text == playerSide)
        {
            GameOver(playerSide);
        }

        else if (buttonList[0].text == playerSide && buttonList[3].text == playerSide && buttonList[7].text == playerSide)
        {
            GameOver(playerSide);
        }

        else if (buttonList[1].text == playerSide && buttonList[4].text == playerSide && buttonList[8].text == playerSide)
        {
            GameOver(playerSide);
        }

        else if (buttonList[2].text == playerSide && buttonList[5].text == playerSide && buttonList[6].text == playerSide)
        {
            GameOver(playerSide);
        }

        else if (buttonList[0].text == playerSide && buttonList[6].text == playerSide && buttonList[8].text == playerSide)
        {
            GameOver(playerSide);
        }

        else if (buttonList[5].text == playerSide && buttonList[7].text == playerSide && buttonList[8].text == playerSide)
        {
            GameOver(playerSide);
        }
        else if (moveCount >= 9)
        {
            GameOver("draw");
        }
        else
        {
            ChangeSides();
        }


    }


    void SetPlayerColors(Player newPlayer, Player oldPlayer)
    {
        newPlayer.panel.color = activePlayerColor.panelColor;
        newPlayer.text.color = activePlayerColor.textColor;
        oldPlayer.panel.color = inactivePlayerColor.panelColor;
        oldPlayer.text.color = inactivePlayerColor.textColor;
    }

    void GameOver(string winningPlayer)
    {
        if (winningPlayer == "draw")
        {
            SetGameOverText("Draw");
            StartCoroutine(RestartGameDelay());
        }
        else
        {
            if (playerSide == "X")
            {
                xWins++;
            }
            else
            {
                oWins++;
            }
            UpdateScoreDisplay();

            if (xWins >= gamesToWin || oWins >= gamesToWin)
            {
                string winner = (xWins >= gamesToWin) ? "X" : "O";
                SetGameOverText("Match Over " + winner + " Player Wins the match!");
                ResetScores(); // Reset scores when a player wins the match
            }

            if (gamesPlayed >= totalGamesToPlay || xWins >= gamesToWin || oWins >= gamesToWin)
            {
                matchOver = true; // Set matchOver to true when the entire match is over
            }
            else
            {
                ChangeSides();
            }

            if (matchOver)
            {
                // Show game over panel and restart button
                gameOverPanel.SetActive(true);
                restartButton.SetActive(true);
                quitButton.SetActive(true);
            }
            else
            {
                StartCoroutine(RestartGameDelay());
            }
        }
    }

    void ResetScores()
    {
        xWins = 0;
        oWins = 0;
        UpdateScoreDisplay();
    }


    IEnumerator RestartGameDelay()
    {
        yield return new WaitForSeconds(1f); // Adjust the delay time as needed
        RestartGame();
        StartGame();
    }

    void ChangeSides()
    {
        playerSide = (playerSide == "X") ? "O" : "X";

        if (playerSide == "X")
        {
            SetPlayerColors(playerX, playerO);
        }
        else
        {
            SetPlayerColors(playerO, playerX);
        }
    }

    void SetGameOverText(string value)
    {
        gameOverPanel.SetActive(true);
        gameOverText.text = value;
    }

    public void RestartGame()
    {
        moveCount = 0;
        gameOverPanel.SetActive(false);
        restartButton.SetActive(false);
        SetPlayerButtons(true);
        SetPlayerColorsInactive();

        for (int i = 0; i < buttonList.Length; i++)
        {
            buttonList[i].text = "";
        }
        SetBoardInteractable(true);

        // Increment gamesPlayed only if the match is not over
        if (!matchOver)
        {
            gamesPlayed++;
        }

        // Reset scores to 0 when completing a match of five games
        if (gamesPlayed % totalGamesToPlay == 0)
        {
            xWins = 0;
            oWins = 0;
            UpdateScoreDisplay();
        }
    }




    void SetBoardInteractable(bool toggle)
    {
        for (int i = 0; i < buttonList.Length; i++)
        {
            buttonList[i].GetComponentInParent<Button>().interactable = toggle;

        }

    }

    void SetPlayerButtons(bool toggle)
    {
        playerX.button.interactable = toggle;
        playerO.button.interactable = toggle;
    }

    void SetPlayerColorsInactive()
    {
        playerX.panel.color = inactivePlayerColor.panelColor;
        playerX.text.color = inactivePlayerColor.textColor;
        playerO.panel.color = inactivePlayerColor.panelColor;
        playerO.text.color = inactivePlayerColor.textColor;
    }

    public void QuitGame()
    {
        quitConfirmationPanel.SetActive(true);
        yesButton.onClick.AddListener(ConfirmQuit);
        noButton.onClick.AddListener(CancelQuit);
        quitButton.SetActive(false);

    }
    public void ConfirmQuit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }

    public void CancelQuit()
    {

        quitConfirmationPanel.SetActive(false);
        quitButton.SetActive(true);
    }



}