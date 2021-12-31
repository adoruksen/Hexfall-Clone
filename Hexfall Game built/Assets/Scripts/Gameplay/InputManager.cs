using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Swipe { Up, Down, Left, Right };

public enum DirectionOfRotation { Clockwise, CounterClockwise }
public class InputManager : MonoBehaviour
{
    private Vector2 firstClickPos;         
    private Vector2 secondClickPos;         
    private Vector3 currentSwipe;           
    private Vector2 vectorHexagonalGroup;   

    private float minSwipeLength = 70f;     

    private int layer_mask;
    private RaycastHit2D hit;

    public static Swipe swipeDirection;
    public static DirectionOfRotation directionOfRotation;

    void Start()
    {
        layer_mask = LayerMask.GetMask("Hexagon");
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)  && GameController.instance.isGamePlaying && GameController.instance.isPlayable && HexagonTrioAnimator.instance.isAnimating == false)
        {
            firstClickPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        }
        if (Input.GetMouseButtonUp(0) && GameController.instance.isGamePlaying && GameController.instance.isPlayable && HexagonTrioAnimator.instance.isAnimating == false)
        {
            secondClickPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            currentSwipe = new Vector3(secondClickPos.x - firstClickPos.x, secondClickPos.y - firstClickPos.y);

            if (currentSwipe.magnitude < minSwipeLength)
            {
                GameController.instance.isPlayable = false;
                SelectObject();
                GameController.instance.isPlayable = true;
                return;
            }

            if (HexagonTrioAnimator.instance.isHexTrio == false)
                return;

            GameController.instance.isPlayable = false;

            currentSwipe.Normalize();

            firstClickPos = Camera.main.ScreenToWorldPoint(firstClickPos);

            if (currentSwipe.y > 0 && currentSwipe.x > -0.5f && currentSwipe.x < 0.5f)
            {
                swipeDirection = Swipe.Up;

                if (firstClickPos.x < vectorHexagonalGroup.x)
                    directionOfRotation = DirectionOfRotation.Clockwise;
                else
                    directionOfRotation = DirectionOfRotation.CounterClockwise;
            }
            else if (currentSwipe.y < 0 && currentSwipe.x > -0.5f && currentSwipe.x < 0.5f)
            {
                swipeDirection = Swipe.Down;

                if (firstClickPos.x > vectorHexagonalGroup.x)
                    directionOfRotation = DirectionOfRotation.Clockwise;
                else
                    directionOfRotation = DirectionOfRotation.CounterClockwise;
            }
            else if (currentSwipe.x < 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f)
            {
                swipeDirection = Swipe.Left;

                if (firstClickPos.y < vectorHexagonalGroup.y)
                    directionOfRotation = DirectionOfRotation.Clockwise;
                else
                    directionOfRotation = DirectionOfRotation.CounterClockwise;
            }
            else if (currentSwipe.x > 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f)
            {
                swipeDirection = Swipe.Right;

                if (firstClickPos.y > vectorHexagonalGroup.y)
                    directionOfRotation = DirectionOfRotation.Clockwise;
                else
                    directionOfRotation = DirectionOfRotation.CounterClockwise;
            }

            HexagonTrioAnimator.instance.Rotater(directionOfRotation);
        }

    }

    private void SelectObject()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        hit = Physics2D.Raycast(ray.origin, ray.direction * 10, Mathf.Infinity, layer_mask);
        if (hit.collider != null && hit.collider && hit.collider.CompareTag("Hexagon"))
        {
            vectorHexagonalGroup = hit.collider.GetComponent<HexagonController>().FindClosestHexagonalGroup(ray.origin);
        }
    }
}
