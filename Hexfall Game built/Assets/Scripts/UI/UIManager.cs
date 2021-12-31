using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    #region Singleton
    public static UIManager instance;
    private void Awake()
    {
        instance = this;
    }
    #endregion

    [System.NonSerialized] public Text gridWidthText;
    [System.NonSerialized] public Slider gridWidthSlider;


    [System.NonSerialized] public Text gridHeightText;
    [System.NonSerialized] public Slider gridHeightSlider;

    [System.NonSerialized] public Text colorCountText;
    [System.NonSerialized] public Slider colorCountSlider;

    [System.NonSerialized] public Text moveCountText;
    [System.NonSerialized] public Text scoreText;


    void Start()
    {
        SetValues();
    }
    public void SetValues()
    {
        gridWidthText = transform.GetChild(0).GetChild(1).GetChild(0).GetChild(1).GetChild(0).GetComponent<Text>();
        gridWidthSlider = transform.GetChild(0).GetChild(1).GetChild(0).GetChild(1).GetChild(1).GetComponent<Slider>();

        gridHeightText = transform.GetChild(0).GetChild(1).GetChild(1).GetChild(1).GetChild(0).GetComponent<Text>();
        gridHeightSlider = transform.GetChild(0).GetChild(1).GetChild(1).GetChild(1).GetChild(1).GetComponent<Slider>();

        colorCountText = transform.GetChild(0).GetChild(1).GetChild(2).GetChild(1).GetChild(0).GetComponent<Text>();
        colorCountSlider = transform.GetChild(0).GetChild(1).GetChild(2).GetChild(1).GetChild(1).GetComponent<Slider>();

        moveCountText = transform.GetChild(1).GetChild(1).GetComponent<Text>();
        scoreText = transform.GetChild(1).GetChild(0).GetComponent<Text>();

        gridWidthText.text = gridWidthSlider.value.ToString();
        gridHeightText.text = gridHeightSlider.value.ToString();
        colorCountText.text = colorCountSlider.value.ToString();


    }

    public void updateMoveCounter(int moveCount)
    {
        moveCountText.GetComponent<Text>().text = "Moves: " + moveCount;
    }

    public void updateScore(int score)
    {
        scoreText.GetComponent<Text>().text = "Score: " + score;
    }

    public void QuitButtonOnClick()
    {
        Application.Quit();
    }

    public void GameOverFunc()
    {
        transform.GetChild(2).gameObject.SetActive(true);
    }

    public void TryAgainButton()
    {
        GameController.instance.PlayButtonFunc();
        transform.GetChild(2).gameObject.SetActive(false);
        moveCountText.text = "Moves: " + 0.ToString();
        transform.GetChild(1).GetChild(3).GetChild(0).gameObject.SetActive(false);


    }
    public void MenuButtonFunc()
    {
        transform.GetChild(1).GetChild(3).GetChild(0).gameObject.SetActive(true);
    }
    public void TurnBackBtnFunc()
    {
        transform.GetChild(1).GetChild(3).GetChild(0).gameObject.SetActive(false);
    }

}

