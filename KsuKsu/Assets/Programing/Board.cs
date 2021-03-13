using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public int width;
    public int height;
    public GameObject backgroundTilePrefab;
    public GameObject[] circles;
    public GameObject[,] allCircles;

    private BackgroundTile[,] _allTiles;


    void Start()
    {
        _allTiles = new BackgroundTile[width,height];
        allCircles = new GameObject[width, height];
        AllTilesSetUp();
    }

    private void AllTilesSetUp()
    {
        for(int i= 0;i < width;i++)
        {
            for (int j = 0; j < height; j++)
            {
                Vector2 tempPosition = new Vector2(i, j);
                GameObject backgroundTile = Instantiate(backgroundTilePrefab, tempPosition, Quaternion.identity) as GameObject;
                backgroundTile.transform.SetParent(this.transform);
                backgroundTile.name = $"({i} - {j})";

                int selectedCircle = Random.Range(0, circles.Length);
                GameObject circle = Instantiate(circles[selectedCircle], tempPosition, Quaternion.identity);
                circle.transform.SetParent(this.transform);
                circle.name = $"({i} - {j})";
                allCircles[i, j] = circle;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
