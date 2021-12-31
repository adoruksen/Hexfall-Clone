using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BombController : HexagonController
{
    public int counter; 

    void Start()
    {
        counter = Random.Range(4, 10);
        CounterTextUpdate();
    }
   
    public
    override void ColorDetector()
    {
        base.ColorDetector();
        if (!GameController.instance.bombsInGameBoard.Contains(gameObject))
            GameController.instance.bombsInGameBoard.Add(gameObject);
    }


    public
    override void DeletedObject()
    {
        GameController.instance.colorOfMyHexagon[row, col] = ' ';
        if (GameController.instance.bombsInGameBoard.Contains(gameObject))
            GameController.instance.bombsInGameBoard.Remove(gameObject);

        Destroy(this.gameObject, 1);
    }


    public void CounterTextUpdate()
    {
        --counter;
        transform.GetChild(0).gameObject.GetComponent<TextMeshPro>().text = "" + counter;
    }
    
}
