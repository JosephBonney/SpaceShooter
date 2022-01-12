using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text scoreText;

    [SerializeField]
    private Image LivesImg;

    [SerializeField]
    private Sprite[] liveSprites;

    [SerializeField]
    private Text GameOverText;

    [SerializeField]
    private Text RestartText;

    [SerializeField]
    private float flickerTime = 0.5f;

    [SerializeField]
    private GameManager gm;



    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        GameOverText.gameObject.SetActive(false);
        RestartText.gameObject.SetActive(false);
        scoreText.text = ("Score: " + 0);

        if(gm == null)
        {
            Debug.LogError("Game Manager is NULL");
        }
        
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void UpdateScore(int playerScore)
    {
        scoreText.text = ("Score: " + playerScore);
    }

    public void UpdateLives(int currentLives)
    {
        LivesImg.sprite = liveSprites[currentLives];

        if(currentLives <= 0)
        {
            GameOverMethod();
        }
    }

    void GameOverMethod()
    {
        GameOverText.gameObject.SetActive(true);
        StartCoroutine(GameOverFlickerRoutine());
        RestartText.gameObject.SetActive(true);
        gm.GameOver();
    }

    IEnumerator GameOverFlickerRoutine()
    {
        while(true)
        {
            GameOverText.text = ("GAME OVER");
            yield return new WaitForSeconds(flickerTime);
            GameOverText.text = ("");
            yield return new WaitForSeconds(flickerTime);
            GameOverText.text = ("GAME OVER");
        }

    }
}
