using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindMatches : MonoBehaviour
{
    private Board _board;
    public List<GameObject> currentMatches;


    // Start is called before the first frame update
    void Start()
    {
        _board = FindObjectOfType<Board>();
        currentMatches = new List<GameObject>();
        FindAllMatches();
    }

    public void FindAllMatches()
    {
        StartCoroutine(FindAllMatchesCo());
    }

    private IEnumerator FindAllMatchesCo()
    {
        yield return new WaitForSeconds(0.2f);

        for (int i = 0; i < _board.width; i++)
        {
            for (int j = 0; j < _board.height; j++)
            {
                GameObject currentCircle = _board.allCircles[i, j];
                if(currentCircle != null)
                {
                    if(i > 0 && i < _board.width - 1)
                    {
                        GameObject leftCircle = _board.allCircles[i - 1, j];
                        GameObject rightCircle = _board.allCircles[i + 1, j];
                        if(leftCircle != null && rightCircle != null)
                        {
                            if(currentCircle.CompareTag(leftCircle.tag) && currentCircle.CompareTag(rightCircle.tag))
                            {
                                if(!currentMatches.Contains(leftCircle))
                                {
                                    currentMatches.Add(leftCircle);
                                }
                                if (!currentMatches.Contains(rightCircle))
                                {
                                    currentMatches.Add(rightCircle);
                                }
                                leftCircle.GetComponent<Circle>().isMatched = true;
                                rightCircle.GetComponent<Circle>().isMatched = true;
                                currentCircle.GetComponent<Circle>().isMatched = true;
                            }
                        }
                    }

                    if (j > 0 && j < _board.height - 1)
                    {
                        GameObject UpCircle = _board.allCircles[i, j + 1];
                        GameObject DownCircle = _board.allCircles[i, j - 1];
                        if (UpCircle != null && DownCircle != null)
                        {
                            if (currentCircle.CompareTag(UpCircle.tag) && currentCircle.CompareTag(DownCircle.tag))
                            {
                                if (!currentMatches.Contains(UpCircle))
                                {
                                    currentMatches.Add(UpCircle);
                                }
                                if (!currentMatches.Contains(DownCircle))
                                {
                                    currentMatches.Add(DownCircle);
                                }
                                UpCircle.GetComponent<Circle>().isMatched = true;
                                DownCircle.GetComponent<Circle>().isMatched = true;
                                currentCircle.GetComponent<Circle>().isMatched = true;
                            }
                        }
                    }
                }
            }
        }
    }
}
