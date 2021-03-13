using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Circle : MonoBehaviour
{
    public float swipeAngle = 0;
    public int column, row, targetX, targetY;

    private Vector2 firstTouchPoint, finalTouchPoint;
    private Vector2 tempPosition;
    private GameObject otherCircle;
    private Board board;

    // Start is called before the first frame update
    void Start()
    {
        board = FindObjectOfType<Board>();
        targetX = (int)transform.position.x;
        targetY = (int)transform.position.y;
        row = targetY;
        column = targetX;
    }

    // Update is called once per frame
    void Update()
    {
        targetX = column;
        targetY = row;

        if(Mathf.Abs(targetX - transform.position.x) > .1)
        {
            tempPosition = new Vector2(targetX, transform.position.y);
            transform.position = Vector2.Lerp(transform.position, tempPosition, 5f);
            
        }
        else
        {
            tempPosition = new Vector2(targetX, transform.position.y);
            transform.position = tempPosition;
            board.allCircles[column, row] = this.gameObject;
        }

        if (Mathf.Abs(targetY - transform.position.y) > .1)
        {
            tempPosition = new Vector2(transform.position.x, targetY);
            transform.position = Vector2.Lerp(transform.position, tempPosition, 5f);

        }
        else
        {
            tempPosition = new Vector2(transform.position.x, targetY);
            transform.position = tempPosition;
            board.allCircles[column, row] = this.gameObject;
        }
    }

    private void OnMouseDown()
    {
        firstTouchPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private void OnMouseUp()
    {
        finalTouchPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        CalculateAngle();
    }

    private void CalculateAngle()
    {
        swipeAngle = Mathf.Atan2(finalTouchPoint.y - firstTouchPoint.y, finalTouchPoint.x - firstTouchPoint.x) * 180 / Mathf.PI;
        Debug.Log(swipeAngle);
        
        MovePieces();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("Press position + " + eventData.pressPosition);
        Debug.Log("End position + " + eventData.position);
        Vector3 dragVectorDirection = (eventData.position - eventData.pressPosition).normalized;
        Debug.Log("norm + " + dragVectorDirection);
       // GetDragDirection(dragVectorDirection);
    }

    private void MovePieces()
    {
        if(swipeAngle > -45 && swipeAngle <= 45 && column < board.width-1) //свайп вправо
        {
            otherCircle = board.allCircles[column + 1, row];
            otherCircle.GetComponent<Circle>().column -= 1;
            column += 1;
        }
        else if (swipeAngle > 45 && swipeAngle <= 135 && row < board.height-1) //свайп вверх
        {
            otherCircle = board.allCircles[column, row + 1];
            otherCircle.GetComponent<Circle>().row -= 1;
            row += 1;
        }
        else if ((swipeAngle > 135 || swipeAngle <= -135) && column > 0) //свайп влево
        {
            otherCircle = board.allCircles[column - 1, row];
            otherCircle.GetComponent<Circle>().column += 1;
            column -= 1;
        }
        else if (swipeAngle < -45 && swipeAngle >= -135 && row > 0) //свайп вниз
        {
            otherCircle = board.allCircles[column, row - 1];
            otherCircle.GetComponent<Circle>().row += 1;
            row -= 1;
        }
    }
}
