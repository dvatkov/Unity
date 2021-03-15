using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Circle : MonoBehaviour
{
    [Header("Board Variables")]
    public int column;
    public int row;
    public int targetX;
    public int targetY;
    public int previousColumn;
    public int previousRow;
    public bool isMatched = false;

    private FindMatches findMatches;
    private float swipeAngle = 0;
    private float swipeResist = 1f;
    private Vector2 firstTouchPoint, finalTouchPoint;
    private Vector2 tempPosition;
    private GameObject otherCircle;
    private Board board;

    // Start is called before the first frame update
    void Start()
    {
        board = FindObjectOfType<Board>();
        findMatches = FindObjectOfType<FindMatches>();
        //targetX = (int)transform.position.x;
        //targetY = (int)transform.position.y;
        //row = targetY;
        //column = targetX;
        //previousRow = row;
        //previousColumn = column;
    }

    // Update is called once per frame
    void Update()
    {
        //FindMatches();
        if (isMatched)
        {
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.color = Color.black;
        }

        targetX = column;
        targetY = row;

        if (Mathf.Abs(targetX - transform.position.x) > .1)
        {
            tempPosition = new Vector2(targetX, transform.position.y);
            transform.position = Vector2.Lerp(transform.position, tempPosition, .6f);
            if(board.allCircles[column,row] != this.gameObject)
            {
                board.allCircles[column, row] = this.gameObject;
            }
            findMatches.FindAllMatches();
        }
        else
        {
            tempPosition = new Vector2(targetX, transform.position.y);
            transform.position = Vector2.Lerp(transform.position, tempPosition, .6f);
        }

        if (Mathf.Abs(targetY - transform.position.y) > .1)
        {
            tempPosition = new Vector2(transform.position.x, targetY);
            transform.position = Vector2.Lerp(transform.position, tempPosition, .6f);
            if (board.allCircles[column, row] != this.gameObject)
            {
                board.allCircles[column, row] = this.gameObject;
            }
            findMatches.FindAllMatches();
        }
        else
        {
            tempPosition = new Vector2(transform.position.x, targetY);
            transform.position = Vector2.Lerp(transform.position, tempPosition, .6f);
        }
    }

    public IEnumerator CheckMoveCo()
    {
        yield return new WaitForSeconds(.5f);
        if(otherCircle != null)
        {
            if (!isMatched && !otherCircle.GetComponent<Circle>().isMatched)
            {
                otherCircle.GetComponent<Circle>().row = row;
                otherCircle.GetComponent<Circle>().column = column;
                row = previousRow;
                column = previousColumn;
            }
            else
            {
                board.DestroyMatches();
            }
            otherCircle = null;
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
        if (Mathf.Abs(finalTouchPoint.y - firstTouchPoint.y) > swipeResist || Mathf.Abs(finalTouchPoint.x - firstTouchPoint.x) > swipeResist)
        {
            swipeAngle = Mathf.Atan2(finalTouchPoint.y - firstTouchPoint.y, finalTouchPoint.x - firstTouchPoint.x) * 180 / Mathf.PI;
            MovePieces();
        }


    }

    private void MovePieces()
    {
        if (swipeAngle > -45 && swipeAngle <= 45 && column < board.width - 1) //свайп вправо
        {
            otherCircle = board.allCircles[column + 1, row];
            otherCircle.GetComponent<Circle>().column -= 1;
            previousRow = row;
            previousColumn = column;
            column += 1;
        }
        else if (swipeAngle > 45 && swipeAngle <= 135 && row < board.height - 1) //свайп вверх
        {
            otherCircle = board.allCircles[column, row + 1];
            otherCircle.GetComponent<Circle>().row -= 1;
            previousRow = row;
            previousColumn = column;
            row += 1;
        }
        else if ((swipeAngle > 135 || swipeAngle <= -135) && column > 0) //свайп влево
        {
            otherCircle = board.allCircles[column - 1, row];
            otherCircle.GetComponent<Circle>().column += 1;
            previousRow = row;
            previousColumn = column;
            column -= 1;
        }
        else if (swipeAngle < -45 && swipeAngle >= -135 && row > 0) //свайп вниз
        {
            otherCircle = board.allCircles[column, row - 1];
            otherCircle.GetComponent<Circle>().row += 1;
            previousRow = row;
            previousColumn = column;
            row -= 1;
        }

        StartCoroutine(CheckMoveCo());
    }

    private void FindMatches()
    {
      
        if(column > 0 && column < board.width - 1)
        {
            GameObject leftDot = board.allCircles[column - 1, row];
            GameObject rightDot = board.allCircles[column + 1, row];
            if (leftDot != null && rightDot != null && leftDot != this.gameObject && rightDot != this.gameObject)
            {
                if (this.CompareTag(leftDot.tag) && this.CompareTag(rightDot.tag))
                {
                    leftDot.GetComponent<Circle>().isMatched = true;
                    rightDot.GetComponent<Circle>().isMatched = true;
                    isMatched = true;
                }
            }
        }

        if (row > 0 && row < board.height - 1)
        {
            GameObject upDot = board.allCircles[column, row + 1];
            GameObject downDot = board.allCircles[column, row - 1];
            if (upDot != null && downDot != null && upDot != this.gameObject && downDot != this.gameObject)
            {
                if (this.CompareTag(upDot.tag) && this.CompareTag(downDot.tag))
                {
                    upDot.GetComponent<Circle>().isMatched = true;
                    downDot.GetComponent<Circle>().isMatched = true;
                    isMatched = true;
                }
            }
        }
    }
}
