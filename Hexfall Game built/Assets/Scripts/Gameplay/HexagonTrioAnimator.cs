using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class HexagonTrioAnimator : MonoBehaviour
{
    #region Singleton
    public static HexagonTrioAnimator instance;
    private void Awake()
    {
        instance = this;
    }
    #endregion


    [System.NonSerialized] public bool isAnimating = false;

    public GameObject firstHex,secondHex,thirdHex;

    private GameObject tempHexagon;

    [System.NonSerialized] public bool isHexTrio;

    private Vector2 firstHexagonPosition;
    private Vector2 secondHexagonPosition;
    private Vector2 thirdHexagonPosition;

    private int rowOfFirstHex, colOfFirstHex, rowOfSecondHex, colOfSecondHex, rowOfThirdHex, colOfThirdHex;
                 
    void Start()
    {
        DOTween.Init();
    }

    public void Reset()
    {
        isHexTrio = false;
    }

    private void OutlineActivator()
    {
        if (firstHex == null || secondHex == null || thirdHex == null)
            return;

        firstHex.transform.GetChild(0).gameObject.SetActive(true);
        this.secondHex.transform.GetChild(0).gameObject.SetActive(true);
        this.thirdHex.transform.GetChild(0).gameObject.SetActive(true);
    }

    private void OutlineDeactivetor()
    {
        if (firstHex == null || secondHex == null || thirdHex == null)
            return;

        this.firstHex.transform.GetChild(0).gameObject.SetActive(false);
        this.secondHex.transform.GetChild(0).gameObject.SetActive(false);
        this.thirdHex.transform.GetChild(0).gameObject.SetActive(false);
    }

    private void RowAndColGetter()
    {
        rowOfFirstHex = this.firstHex.GetComponent<HexagonController>().row;
        colOfFirstHex = this.firstHex.GetComponent<HexagonController>().col;

        rowOfSecondHex = this.secondHex.GetComponent<HexagonController>().row;
        colOfSecondHex = this.secondHex.GetComponent<HexagonController>().col;

        rowOfThirdHex = this.thirdHex.GetComponent<HexagonController>().row;
        colOfThirdHex = this.thirdHex.GetComponent<HexagonController>().col;
    }

    public void SelectedHexagonTrio(GameObject firstHexagon, GameObject secondHexagon, GameObject thirdHexagon)
    {
        if (firstHexagon == null || secondHexagon == null || thirdHexagon == null)
            return;

        OutlineDeactivetor();

        this.firstHex = firstHexagon;
        this.secondHex = secondHexagon;
        this.thirdHex = thirdHexagon;

        RowAndColGetter();

        OutlineActivator();

        this.isHexTrio = true;
    }

    public void RefreshHexagonTrio()
    {
        if (isHexTrio == false)
            return;

        this.SelectedHexagonTrio(GameController.instance.myHexagon[rowOfFirstHex, colOfFirstHex],
                         GameController.instance.myHexagon[rowOfSecondHex, colOfSecondHex],
                         GameController.instance.myHexagon[rowOfThirdHex, colOfThirdHex]);
    }

    public void Rotater(DirectionOfRotation directionOfRotation)
    {
        if (firstHex == null || secondHex == null || thirdHex == null)
            return;

        isAnimating = true;

        firstHexagonPosition = GameController.instance.PositionCalculator(firstHex);
        secondHexagonPosition = GameController.instance.PositionCalculator(secondHex);
        thirdHexagonPosition = GameController.instance.PositionCalculator(thirdHex);

        StartCoroutine(MoverCo(directionOfRotation));
    }

    IEnumerator MoverCo(DirectionOfRotation directionOfRotation)
    {
        if (directionOfRotation == DirectionOfRotation.Clockwise)
        {
            for (int i = 0; i < 3; ++i)
            {
                this.firstHex.GetComponent<HexagonController>().MoveAnimation(thirdHexagonPosition);
                this.secondHex.GetComponent<HexagonController>().MoveAnimation(firstHexagonPosition);
                this.thirdHex.GetComponent<HexagonController>().MoveAnimation(secondHexagonPosition);

                yield return new WaitForSeconds(1);

                GameController.instance.BoardUpdater(firstHex, secondHex, thirdHex);

                if (i < 2 && GameController.instance.IsMatching())
                {
                    this.OutlineDeactivetor();
                    GameController.instance.HexagonsMached();
                    break;
                }
                tempHexagon = firstHex;
                firstHex = secondHex;
                secondHex = thirdHex;
                thirdHex = tempHexagon;
            }
        }
        else
        {
            for (int i = 0; i < 3; ++i)
            {
                this.firstHex.GetComponent<HexagonController>().MoveAnimation(secondHexagonPosition);
                this.secondHex.GetComponent<HexagonController>().MoveAnimation(thirdHexagonPosition);
                this.thirdHex.GetComponent<HexagonController>().MoveAnimation(firstHexagonPosition);

                yield return new WaitForSeconds(1);

                GameController.instance.BoardUpdater(firstHex, secondHex, thirdHex);

                if (i < 2 && GameController.instance.IsMatching())
                {
                    this.OutlineDeactivetor();
                    GameController.instance.HexagonsMached();
                    break;
                }
                tempHexagon = firstHex;
                firstHex = thirdHex;
                thirdHex = secondHex;
                secondHex = tempHexagon;
            }
        }
        isAnimating = false;
    }
}


