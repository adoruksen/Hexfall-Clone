using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GameController : MonoBehaviour
{
    #region Singleton
    public static GameController instance;
    private void Awake()
    {
        instance = this;
    }
    #endregion

    #region variables
    [Header("Game Settings")]
    [System.NonSerialized] public int height;
    [System.NonSerialized] public int width;
    int colorCounter;
    private readonly int explodedHexPoint = 5;
    private int bombPoint;


    [Header("Lists and Arrays")]
    public GameObject[] bombList;
    public GameObject[] hexagonList;
    public GameObject[,] myHexagon;
    public char[,] colorOfMyHexagon;
    [System.NonSerialized] public List<GameObject> bombsInGameBoard;
    
    
    
    int colorIndex;
    private int moveCounter ;
    [System.NonSerialized] public int scoreBoard ;

    private readonly float xPositionEven = -2.35f;
    private readonly float yPositionEven = 2.65f;
    private readonly float xPositionOdd = -1.68f;
    private readonly float yPositionOdd = 2.295f;
    private readonly float xDistanceBetweenHexagons = 1.35f;
    private readonly float yDistanceBetweenHexagons = 0.74f;

    public Transform hexagonParent;
    public Transform hexagonBornPlace;
   
     public bool isPlayable;
     public bool isGamePlaying;
    #endregion



    public void PlayButtonFunc()
    {
        SetParameterValues();
        UIManager.instance.transform.GetChild(0).gameObject.SetActive(false);
        UIManager.instance.transform.GetChild(1).gameObject.SetActive(true);
        isPlayable = false;
        isGamePlaying = false;
        scoreBoard = 0;
        moveCounter = 0;
        bombPoint = 1000;
        HexagonTrioAnimator.instance.Reset();
        InitializeBoard();
        IsMatching();
        isGamePlaying = true;
    }


    void SetParameterValues()
    {
        width = (int)UIManager.instance.transform.GetChild(0).GetChild(1).GetChild(0).GetChild(1).GetChild(1).GetComponent<Slider>().value;
        height = (int)UIManager.instance.transform.GetChild(0).GetChild(1).GetChild(1).GetChild(1).GetChild(1).GetComponent<Slider>().value;
        colorCounter = (int)UIManager.instance.transform.GetChild(0).GetChild(1).GetChild(2).GetChild(1).GetChild(1).GetComponent<Slider>().value;
    }

    public Vector2 PositionCalculator (int row, int col)
    {
        float resultPositionX = 0f, tempPositionY = 0f;

        switch (col % 2)
        {
            case 0:
                resultPositionX = xPositionEven;
                tempPositionY = yPositionEven;

                break;

            case 1:
                resultPositionX = xPositionOdd;
                tempPositionY = yPositionOdd;
                break;
        }

        float xPos = resultPositionX + (xDistanceBetweenHexagons * (col / 2));
        float yPos = tempPositionY - (row * yDistanceBetweenHexagons);

        return new Vector2(xPos, yPos);
    }

    public Vector2 PositionCalculator(GameObject myHex)
    {
        int row = myHex.GetComponent<HexagonController>().row;
        int col = myHex.GetComponent<HexagonController>().col;

        return PositionCalculator(row, col);
    }

    public void HexagonsMached()
    {
        ++moveCounter;
        UIManager.instance.updateMoveCounter(moveCounter);

        foreach (GameObject bomb in bombsInGameBoard)
        {
            if (bomb != null)
            {
                bomb.GetComponent<BombController>().CounterTextUpdate();
            }
        }
    }

    public void BoardUpdater(GameObject firstHex, GameObject secondHex, GameObject thirdHex)
    {
        if (firstHex == null || secondHex == null || thirdHex == null)
            return;

        int firstRow, firstCol, secondRow, secondCol, thirdRow, thirdCol;

        firstRow = firstHex.GetComponent<HexagonController>().row;
        firstCol = firstHex.GetComponent<HexagonController>().col;

        secondRow = secondHex.GetComponent<HexagonController>().row;
        secondCol = secondHex.GetComponent<HexagonController>().col;

        thirdRow = thirdHex.GetComponent<HexagonController>().row;
        thirdCol = thirdHex.GetComponent<HexagonController>().col;

        if (InputManager.directionOfRotation == DirectionOfRotation.Clockwise)
        {
            firstHex.GetComponent<HexagonController>().SetPos(thirdRow, thirdCol);
            secondHex.GetComponent<HexagonController>().SetPos(firstRow, firstCol);
            thirdHex.GetComponent<HexagonController>().SetPos(secondRow, secondCol);
        }
        else
        {
            firstHex.GetComponent<HexagonController>().SetPos(secondRow, secondCol);
            secondHex.GetComponent<HexagonController>().SetPos(thirdRow, thirdCol);
            thirdHex.GetComponent<HexagonController>().SetPos(firstRow, firstCol);
        }
        firstHex.GetComponent<HexagonController>().ColorDetector();
        secondHex.GetComponent<HexagonController>().ColorDetector();
        thirdHex.GetComponent<HexagonController>().ColorDetector();
    }

    public bool IsMatching()
    {
        isPlayable = false;
        bool result = false;
        List<GameObject> disappearedHexsList = new List<GameObject>();

        for (int col = 0; col < width; ++col)
        {

            for (int row = 0; row < height; ++row)
            {
                bool hexagonWillBeExploded = false;

                switch (col % 2)
                {
                    case 0:
                        if (row - 1 >= 0 && col + 1 < width)
                        {
                            if (colorOfMyHexagon[row - 1, col] == colorOfMyHexagon[row, col] && colorOfMyHexagon[row, col] == colorOfMyHexagon[row - 1, col + 1])
                                hexagonWillBeExploded = true;
                        }
                        if (row - 1 >= 0 && col + 1 < width)
                        {
                            if (colorOfMyHexagon[row, col] == colorOfMyHexagon[row, col + 1] && colorOfMyHexagon[row, col] == colorOfMyHexagon[row - 1, col + 1])
                                hexagonWillBeExploded = true;
                        }
                        if (row + 1 < height && col + 1 < width)
                        {
                            if (colorOfMyHexagon[row, col] == colorOfMyHexagon[row + 1, col] && colorOfMyHexagon[row, col] == colorOfMyHexagon[row, col + 1])
                                hexagonWillBeExploded = true;
                        }
                        if (row - 1 >= 0 && col - 1 >= 0)
                        {
                            if (colorOfMyHexagon[row - 1, col - 1] == colorOfMyHexagon[row, col] && colorOfMyHexagon[row, col] == colorOfMyHexagon[row - 1, col])
                                hexagonWillBeExploded = true;
                        }
                        if (row - 1 >= 0 && col - 1 >= 0)
                        {
                            if (colorOfMyHexagon[row - 1, col - 1] == colorOfMyHexagon[row, col - 1] && colorOfMyHexagon[row, col - 1] == colorOfMyHexagon[row, col])
                                hexagonWillBeExploded = true;
                        }
                        if (row + 1 < height && col - 1 >= 0)
                        {
                            if (colorOfMyHexagon[row, col - 1] == colorOfMyHexagon[row + 1, col] && colorOfMyHexagon[row + 1, col] == colorOfMyHexagon[row, col])
                                hexagonWillBeExploded = true;
                        }
                        break;
                    case 1:
                        if (row - 1 >= 0 && col + 1 < width)
                        {
                            if (colorOfMyHexagon[row - 1, col] == colorOfMyHexagon[row, col] && colorOfMyHexagon[row, col] == colorOfMyHexagon[row, col + 1])
                                hexagonWillBeExploded = true;
                        }
                        if (row + 1 < height && col + 1 < width)
                        {
                            if (colorOfMyHexagon[row, col] == colorOfMyHexagon[row + 1, col + 1] && colorOfMyHexagon[row + 1, col + 1] == colorOfMyHexagon[row, col + 1])
                                hexagonWillBeExploded = true;
                        }
                        if (row + 1 < height && col + 1 < width)
                        {
                            if (colorOfMyHexagon[row, col] == colorOfMyHexagon[row + 1, col] && colorOfMyHexagon[row + 1, col] == colorOfMyHexagon[row + 1, col + 1])
                                hexagonWillBeExploded = true;
                        }
                        if (row - 1 >= 0 && col - 1 >= 0)
                        {
                            if (colorOfMyHexagon[row, col - 1] == colorOfMyHexagon[row, col] && colorOfMyHexagon[row, col] == colorOfMyHexagon[row - 1, col])
                                hexagonWillBeExploded = true;
                        }
                        if (row + 1 < height && col - 1 >= 0)
                        {
                            if (colorOfMyHexagon[row, col - 1] == colorOfMyHexagon[row + 1, col - 1] && colorOfMyHexagon[row + 1, col - 1] == colorOfMyHexagon[row, col])
                                hexagonWillBeExploded = true;
                        }
                        if (row + 1 < height && col - 1 >= 0)
                        {
                            if (colorOfMyHexagon[row + 1, col - 1] == colorOfMyHexagon[row + 1, col] && colorOfMyHexagon[row + 1, col] == colorOfMyHexagon[row, col])
                                hexagonWillBeExploded = true;
                        }
                        break;
                    default:
                        break;
                }
                if (hexagonWillBeExploded)
                {
                    disappearedHexsList.Add(myHexagon[row, col]);
                    result = true;
                }
            }
        }

        if (HexagonTrioAnimator.instance.isHexTrio)
            scoreBoard += disappearedHexsList.Count * explodedHexPoint;

        foreach (GameObject myHex in disappearedHexsList) 
            myHex.GetComponent<HexagonController>().DeletedObject();
        
            

        UIManager.instance.updateScore(scoreBoard);

        if (result)
        {
            StartCoroutine(BoardFiller());
        }
        else
        {
            TımeOfBombCounter();
            isPlayable = true;
        }

        return result;
    }

    public bool TımeOfBombCounter()
    {
        bool result = false;
        
        foreach (GameObject bomb in bombsInGameBoard)
        {
            if (bomb != null && bomb.GetComponent<BombController>().counter <= 0)
                result = true;
        }
        if (result)
        {
            isGamePlaying = false;
            UIManager.instance.GameOverFunc();
        }
        return result;
    }

    private IEnumerator BoardFiller()
    {
        yield return new WaitForSeconds(.5f);

        for (int col = 0; col < width; ++col)
        {
            for (int row = height - 1; row >= 0; --row)
            {
                if (colorOfMyHexagon[row, col] == ' ')
                {
                    bool foundAnObject = false;
                    for (int tempRow = row - 1; !foundAnObject && tempRow >= 0; --tempRow)
                    {
                        if (colorOfMyHexagon[tempRow, col] != ' ' )
                        {
                            colorOfMyHexagon[tempRow, col] = ' ';
                            myHexagon[tempRow, col].GetComponent<HexagonController>().MoveAnimation(PositionCalculator(row, col));
                            myHexagon[tempRow, col].GetComponent<HexagonController>().SetPos(row, col);
                            myHexagon[tempRow, col].GetComponent<HexagonController>().ColorDetector();
                            foundAnObject = true;
                        }
                    }
                    if (foundAnObject == false)
                    {
                        float tempPositionX = 0f;
                        switch (col % 2)
                        {
                            case 0:
                                tempPositionX = xPositionEven;
                                break;

                            case 1:
                                tempPositionX = xPositionOdd;
                                break;
                        }
                        float xPos = tempPositionX + (xDistanceBetweenHexagons * (col / 2)); ;
                        if (scoreBoard >= bombPoint)
                        {
                            colorIndex = Random.Range(0, colorCounter);
                            myHexagon[row, col] = Instantiate(bombList[colorIndex], new Vector2(xPos, 6.40f), Quaternion.identity);
                            bombPoint += 1000;
                        }
                        else {
                            colorIndex = Random.Range(0, colorCounter);
                            myHexagon[row, col] = Instantiate(hexagonList[colorIndex], new Vector2(xPos, 6.40f), Quaternion.identity, hexagonParent);
                        }

                        myHexagon[row, col].GetComponent<HexagonController>().MoveAnimation(PositionCalculator(row, col));
                        myHexagon[row, col].GetComponent<HexagonController>().SetPos(row, col);
                        myHexagon[row, col].GetComponent<HexagonController>().ColorDetector();
                    }
                }
            }
        }
        if (IsMatching() == false)
            HexagonTrioAnimator.instance.RefreshHexagonTrio();
    }

    void InitializeBoard()
    {
        if (myHexagon == null)
        {
            colorOfMyHexagon = new char[height, width];
            myHexagon = new GameObject[height, width];
            bombsInGameBoard = new List<GameObject>();
        }
        else
        {
            bombsInGameBoard.Clear();
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    Destroy(myHexagon[i, j].gameObject);
                    colorOfMyHexagon[i, j] = ' ';
                }
            }
        }
        for (int col = 0; col < width; col++)
        {
            for (int row = 0; row < height; row++)
            {
                colorIndex = Random.Range(0, colorCounter);
                myHexagon[row, col] = Instantiate(hexagonList[colorIndex],hexagonBornPlace.transform.position ,Quaternion.identity, hexagonParent);
                myHexagon[row, col].transform.DOMove(PositionCalculator(row, col), .5f);
                myHexagon[row, col].GetComponent<HexagonController>().SetPos(row, col);
                colorOfMyHexagon[row, col] = myHexagon[row, col].GetComponent<HexagonController>().color;
            }

        }
    }


   
}
