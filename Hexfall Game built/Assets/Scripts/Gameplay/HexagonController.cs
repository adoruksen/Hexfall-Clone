using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class HexagonController : MonoBehaviour
{
    public int row, col;
    public char color = ' ';

    private Vector2 rightTop, rightSide, rightBottom, leftTop, leftSide, leftBottom;

    private float rightTopDistance, rightSideDistance, rightBottomDistance, leftTopDistance, leftSideDistance, leftBottomDistance;

    private List<float> distanceBetweenCornerAndOrigin;



    void Start()
    {
        DOTween.Init();
    }
    public void SetPos(int row, int col)
    {

        this.row = row;
        this.col = col;
    }

    public void MoveAnimation(Vector2 vector2)
    {
        transform.DOMove(vector2, 0.3f, false);
    }

    public virtual void ColorDetector()
    {
        GameController.instance.colorOfMyHexagon[row, col] = color;
        GameController.instance.myHexagon[row, col] = this.gameObject;
    }

    public virtual void DeletedObject()
    {
        GameController.instance.colorOfMyHexagon[row, col] = ' ';
        Destroy(this.gameObject, .5f);

    }

    private void VectorCalculator()
    {
        rightTop = new Vector2(transform.position.x + 0.23f, transform.position.y + 0.37f);
        rightSide = new Vector2(transform.position.x + 0.46f, transform.position.y);
        rightBottom = new Vector2(transform.position.x + 0.23f, transform.position.y - 0.38f);
        leftTop = new Vector2(transform.position.x - 0.23f, transform.position.y + 0.37f);
        leftSide = new Vector2(transform.position.x - 0.46f, transform.position.y);
        leftBottom = new Vector2(transform.position.x - 0.23f, transform.position.y - 0.38f);
    }

    private void CalculateDistances(Vector2 rayOrigin)
    {
        rightTopDistance = Vector2.Distance(rayOrigin, rightTop);
        rightSideDistance = Vector2.Distance(rayOrigin, rightSide);
        rightBottomDistance = Vector2.Distance(rayOrigin, rightBottom);
        leftTopDistance = Vector2.Distance(rayOrigin, leftTop);
        leftSideDistance = Vector2.Distance(rayOrigin, leftSide);
        leftBottomDistance = Vector2.Distance(rayOrigin, leftBottom);
    }

    private void DistanceListAdder()
    {
        distanceBetweenCornerAndOrigin = new List<float>();
        distanceBetweenCornerAndOrigin.Add(rightTopDistance);
        distanceBetweenCornerAndOrigin.Add(rightSideDistance);
        distanceBetweenCornerAndOrigin.Add(rightBottomDistance);
        distanceBetweenCornerAndOrigin.Add(leftTopDistance);
        distanceBetweenCornerAndOrigin.Add(leftSideDistance);
        distanceBetweenCornerAndOrigin.Add(leftBottomDistance);

        distanceBetweenCornerAndOrigin.Sort();
    }

    public Vector2 FindClosestHexagonalGroup(Vector2 rayOrigin)
    {
        VectorCalculator();
        CalculateDistances(rayOrigin);
        DistanceListAdder();    

        int topIndex = row - 1,
            bottomIndex = row + 1,
            leftIndex = col - 1,
            rightIndex = col + 1;

        GameObject first, second, third;

        foreach (double distance in distanceBetweenCornerAndOrigin)
        {
            if (distance == rightTopDistance) 
            {
                switch (col % 2)
                {
                    
                    case 0:

                        if (topIndex >= 0 && rightIndex < GameController.instance.width)
                        {
                            first = GameController.instance.myHexagon[topIndex, col];
                            second = GameController.instance.myHexagon[row, col];
                            third = GameController.instance.myHexagon[topIndex, rightIndex];

                            HexagonTrioAnimator.instance.SelectedHexagonTrio(first, second, third);
                            return rightTop;
                        }
                        break;
                    case 1:
                        if (topIndex >= 0 && rightIndex < GameController.instance.width)
                        {
                            first = GameController.instance.myHexagon[topIndex, col];
                            second = GameController.instance.myHexagon[row, col];
                            third = GameController.instance.myHexagon[row, rightIndex];

                            HexagonTrioAnimator.instance.SelectedHexagonTrio(first, second, third);
                            return rightTop;
                        }
                        break;
                    default:
                        break;
                }

            }
            else if (distance == rightSideDistance) 
            {
                switch (col % 2)
                {
                    case 0:
                        if (topIndex >= 0 && rightIndex < GameController.instance.width)
                        {
                            first = GameController.instance.myHexagon[row, col];
                            second = GameController.instance.myHexagon[row, rightIndex];
                            third = GameController.instance.myHexagon[topIndex, rightIndex];

                            HexagonTrioAnimator.instance.SelectedHexagonTrio(first, second, third);
                            return rightSide;
                        }
                        break;
                    case 1:
                        if (bottomIndex < GameController.instance.height && rightIndex < GameController.instance.width)
                        {
                            first = GameController.instance.myHexagon[row, col];
                            second = GameController.instance.myHexagon[bottomIndex, rightIndex];
                            third = GameController.instance.myHexagon[row, rightIndex];

                            HexagonTrioAnimator.instance.SelectedHexagonTrio(first, second, third);
                            return rightSide;
                        }
                        break;
                    default:
                        break;
                }
            }
            else if (distance == rightBottomDistance) 
            {
                switch (col % 2)
                {
                    case 0:
                        if (bottomIndex < GameController.instance.height && rightIndex < GameController.instance.width)
                        {
                            first = GameController.instance.myHexagon[row, col];
                            second = GameController.instance.myHexagon[bottomIndex, col];
                            third = GameController.instance.myHexagon[row, rightIndex];

                            HexagonTrioAnimator.instance.SelectedHexagonTrio(first, second, third);
                            return rightBottom;
                        }
                        break;
                    case 1:
                        if (bottomIndex < GameController.instance.height && rightIndex < GameController.instance.width)
                        {
                            first = GameController.instance.myHexagon[row, col];
                            second = GameController.instance.myHexagon[bottomIndex, col];
                            third = GameController.instance.myHexagon[bottomIndex, rightIndex];

                            HexagonTrioAnimator.instance.SelectedHexagonTrio(first, second, third);
                            return rightBottom;
                        }
                        break;
                    default:
                        break;
                }
            }
            else if (distance == leftTopDistance) 
            {
                switch (col % 2)
                {
                    case 0:
                        if (topIndex >= 0 && leftIndex >= 0)
                        {
                            first = GameController.instance.myHexagon[topIndex, leftIndex];
                            second = GameController.instance.myHexagon[row, col];
                            third = GameController.instance.myHexagon[topIndex, col];

                            HexagonTrioAnimator.instance.SelectedHexagonTrio(first, second, third);
                            return leftTop;
                        }
                        break;
                    case 1:
                        if (topIndex >= 0 && leftIndex >= 0)
                        {
                            first = GameController.instance.myHexagon[row, leftIndex];
                            second = GameController.instance.myHexagon[row, col];
                            third = GameController.instance.myHexagon[topIndex, col];

                            HexagonTrioAnimator.instance.SelectedHexagonTrio(first, second, third);
                            return leftTop;
                        }
                        break;
                    default:
                        break;
                }
            }
            else if (distance == leftSideDistance) 
            {
                switch (col % 2)
                {
                    case 0:
                        if (topIndex >= 0 && leftIndex >= 0)
                        {
                            first = GameController.instance.myHexagon[topIndex, leftIndex];
                            second = GameController.instance.myHexagon[row, leftIndex];
                            third = GameController.instance.myHexagon[row, col];

                            HexagonTrioAnimator.instance.SelectedHexagonTrio(first, second, third);
                            return leftSide;
                        }
                        break;
                    case 1:
                        if (bottomIndex < GameController.instance.height && leftIndex >= 0)
                        {
                            first = GameController.instance.myHexagon[row, leftIndex];
                            second = GameController.instance.myHexagon[bottomIndex, leftIndex];
                            third = GameController.instance.myHexagon[row, col];

                            HexagonTrioAnimator.instance.SelectedHexagonTrio(first, second, third);
                            return leftSide;
                        }
                        break;
                    default:
                        break;
                }
            }
            else if (distance == leftBottomDistance) 
            {
                switch (col % 2)
                {
                    case 0:
                        if (bottomIndex < GameController.instance.height && leftIndex >= 0)
                        {
                            first = GameController.instance.myHexagon[row, leftIndex];
                            second = GameController.instance.myHexagon[bottomIndex, col];
                            third = GameController.instance.myHexagon[row, col];

                            HexagonTrioAnimator.instance.SelectedHexagonTrio(first, second, third);
                            return leftBottom;
                        }
                        break;
                    case 1:
                        if (bottomIndex < GameController.instance.height && leftIndex >= 0)
                        {
                            first = GameController.instance.myHexagon[bottomIndex, leftIndex];
                            second = GameController.instance.myHexagon[bottomIndex, col];
                            third = GameController.instance.myHexagon[row, col];

                            HexagonTrioAnimator.instance.SelectedHexagonTrio(first, second, third);
                            return leftBottom;
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        return new Vector2(0f, 0f);
    }
}
