using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverText : MonoBehaviour
{
    protected UnityEngine.UI.Text gameOverText;
    protected float currentTime;
    protected int currentAutoMessage = 0;
    public float messageDisplayTime = 0f;
    protected Color originalColor;
    public float fadeSpeed = 1f;
    public float colorTolerence = 0f;
    public static readonly float DEFAULT_COLOR_TOLERENCE_RO = .003f;
    protected string[] autoMessages = new string[] {
        "Your computer automatically called called space highway - side assistance.",
        "Your going to have to call someone to pick you up from Mars.",
        "Probably your parents...", "Who told you not to buy this ship.",
        "Game over."
    };
    void Start()
    {
        gameOverText = GetComponent<UnityEngine.UI.Text>();
        gameOverText.text = SceneChanger.gameOverMessage;
        if( colorTolerence == 0f )
            colorTolerence = DEFAULT_COLOR_TOLERENCE_RO;
        originalColor = gameOverText.color;
        gameOverText.color = Color.clear;
        if (messageDisplayTime == 0f)
            messageDisplayTime = 2f;
        currentTime = messageDisplayTime;
    }


    public static bool ColorsAreClose( Color first, Color second, float tolerence )
    {
        return Mathf.Sqrt(Mathf.Pow((first.b - second.b), 2f) +
            Mathf.Pow((first.g - second.g), 2f) +
            Mathf.Pow((first.r - second.r), 2f)) < tolerence;
    }

    void Update()
    {
        if (currentTime <= 0f)
        {
            if (ColorsAreClose(gameOverText.color, Color.clear, colorTolerence) != true)
                gameOverText.color = Color.Lerp(gameOverText.color, Color.clear, fadeSpeed * Time.deltaTime);
            else if (currentAutoMessage < autoMessages.Length)
            {
                gameOverText.text = autoMessages[currentAutoMessage];
                currentTime = messageDisplayTime;
                ++currentAutoMessage;
            }
            else
                SceneChanger.GoToMainMenu();
        }
        else
        {
            if (ColorsAreClose( gameOverText.color, originalColor, colorTolerence ) != true)
                gameOverText.color = Color.Lerp(gameOverText.color, originalColor, fadeSpeed * Time.deltaTime);
            else
                currentTime -= Time.deltaTime;
        }
    }
}
