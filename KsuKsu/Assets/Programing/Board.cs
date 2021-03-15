using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public int width;
    public int height;
    public int offSet;
    public GameObject backgroundTilePrefab;
    public GameObject[] circles;
    public GameObject[,] allCircles;

    private BackgroundTile[,] _allTiles;
    private FindMatches _findMatches;


    void Start()
    {
        _allTiles = new BackgroundTile[width,height];
        allCircles = new GameObject[width, height];
        _findMatches = FindObjectOfType<FindMatches>();
        AllTilesSetUp();
    }

    private void AllTilesSetUp()
    {
        for(int i= 0;i < width;i++)
        {
            for (int j = 0; j < height; j++)
            {
                Vector2 tempPosition = new Vector2(i, j + offSet);
                GameObject backgroundTile = Instantiate(backgroundTilePrefab, tempPosition, Quaternion.identity) as GameObject;
                backgroundTile.transform.SetParent(this.transform);
                backgroundTile.name = $"({i} - {j})";

                int selectedCircle = Random.Range(0, circles.Length);
                int maxIterations = 0;
                while(MatchesAt(i,j, circles[selectedCircle]) && maxIterations < 100)
                {
                    maxIterations++;
                    selectedCircle = Random.Range(0, circles.Length);
                }
                maxIterations = 0;

                GameObject circle = Instantiate(circles[selectedCircle], tempPosition, Quaternion.identity);
                circle.GetComponent<Circle>().row = j;
                circle.GetComponent<Circle>().column = i;

                circle.transform.SetParent(this.transform);
                circle.name = $"({i} - {j})";
                allCircles[i, j] = circle;
            }
        }
    }

    private bool MatchesAt(int column, int row, GameObject piece)
    {

        if (column > 1 && row > 1)
        {
            if (allCircles[column - 1,row].tag == piece.tag & allCircles[column - 2, row].tag == piece.tag)
            {
                return true;
            }

            if (allCircles[column, row - 1].tag == piece.tag & allCircles[column, row - 2].tag == piece.tag)
            {
                return true;
            }
        }
        else if(column <= 1 || row <= 1)
        {
            if(row > 1)
            {
                if(allCircles[column, row - 1].tag == piece.tag & allCircles[column,row - 2].tag == piece.tag)
                {
                    return true;
                }
            }
            if (column > 1)
            {
                if (allCircles[column - 1, row].tag == piece.tag & allCircles[column - 2, row].tag == piece.tag)
                {
                    return true;
                }
            }
        }

        return false;
    }

    private void DestroyMatchesAt(int column, int row)
    {
        if(allCircles[column,row].GetComponent<Circle>().isMatched == true)
        {
            _findMatches.currentMatches.Remove(allCircles[column, row]);
            Destroy(allCircles[column, row]);
            allCircles[column, row] = null;
        }
    }

    public void DestroyMatches()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if(allCircles[i,j] != null)
                {
                    DestroyMatchesAt(i, j);
                }
            }
        }

        StartCoroutine(DecreaseRowCo());
    }

    private IEnumerator DecreaseRowCo()
    {
        int nullCount = 0;

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (allCircles[i, j] == null)
                {
                    nullCount++;
                }
                else if (nullCount > 0)
                {
                    allCircles[i, j].GetComponent<Circle>().row -= nullCount;
                    allCircles[i, j] = null;
                } 
            }
            nullCount = 0;
        }
        yield return new WaitForSeconds(0.4f);

        StartCoroutine(FillBoardCo());
    }

    private void RefillBoard()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if(allCircles[i,j] == null)
                {
                    Vector2 tempPosition = new Vector2(i, j + offSet);
                    int selectedCircle = Random.Range(0, circles.Length);
                    GameObject circle = Instantiate(circles[selectedCircle], tempPosition, Quaternion.identity);
                    allCircles[i, j] = circle;
                    circle.GetComponent<Circle>().row = j;
                    circle.GetComponent<Circle>().column = i;
                }
            }
        }
    }

    private bool MatchesOnBoard()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (allCircles[i, j] != null)
                {
                    if(allCircles[i,j].GetComponent<Circle>().isMatched == true)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    private IEnumerator FillBoardCo()
    {
        RefillBoard();
        yield return new WaitForSeconds(.5f);
        while(MatchesOnBoard())
        {
            yield return new WaitForSeconds(.5f);
            DestroyMatches();
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
