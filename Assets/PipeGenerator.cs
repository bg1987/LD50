using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PipeGenerator : MonoBehaviour
{
    const int GRID_SIZE = 10;

    public GameObject Pipe;
    
    public bool generate;

    public static int[,] grid = new int[GRID_SIZE,GRID_SIZE];

    public static int pipeCount = 0;

    public static int Cell(int x, int y)
    {
        if (x >= GRID_SIZE || x < 0 || y >= GRID_SIZE || y < 0)
        {
            return Int32.MaxValue;
        }

        return grid[x, y];
    }
    
    // Start is called before the first frame update
    private void Update()
    {
        if (generate)
        {
            Generate();
        }
    }

    public void Generate()
    {
        generate = false;
        ClearChildren();
        GenerateLine();
    }

    void ClearChildren()
    {
        for (int i = 0; i < this.transform.childCount; i++)
        {
            Destroy(this.transform.GetChild(i).gameObject);
        }
        grid = new int[GRID_SIZE,GRID_SIZE];
    }

    //returns a postion at 0 X or 0 Y
    int[] StartPosition()
    {
        var result = new[] {0,0};
        var chosenIndex = Mathf.RoundToInt(Random.value);
        result[chosenIndex] = Random.Range(0, GRID_SIZE);

        return result;
    }

    int[] Up(int[] pos)
    {
        return new[] {pos[0], pos[1] + 1};
    }
    
    int[] Down(int[] pos)
    {
        return new[] {pos[0], pos[1] - 1};
    }
    
    int[] Left(int[] pos)
    {
        return new[] {pos[0] - 1, pos[1]};
    }
    
    int[] Right(int[] pos)
    {
        return new[] {pos[0] + 1, pos[1]};
    }

    bool InBounds(int[] pos)
    {
        return pos[0] >= 0 && pos[0] < GRID_SIZE
            && pos[1] >= 0 && pos[1] < GRID_SIZE;
    }

    bool FreeSpace(int[] pos)
    {
        return grid[pos[0], pos[1]] == 0;
    }

    int[] NextPipe(int[] currentPosition)
    {
        var up = Up(currentPosition);
        var down = Down(currentPosition);
        var left = Left(currentPosition);
        var right = Right(currentPosition);
        
        var options = new List<int[]>();
        if (InBounds(up) && FreeSpace(up))
        {
            options.Add(up);
        }        
        if (InBounds(down) && FreeSpace(down))
        {
            options.Add(down);
        }        
        if (InBounds(left) && FreeSpace(left))
        {
            options.Add(left);
        }        
        if (InBounds(right) && FreeSpace(right))
        {
            options.Add(right);
        }

        if (options.Count == 0)
        {
            throw new Exception("bad");
        }
        
        var chosen = options[Random.Range(0, options.Count)];
        return chosen;
    }
    
    public int GetRandomWeightedIndex(int[] weights)
    {
        // Get the total sum of all the weights.
        int weightSum = 0;
        for (int i = 0; i < weights.Length; ++i)
        {
            weightSum += weights[i];
        }
 
        // Step through all the possibilities, one by one, checking to see if each one is selected.
        int index = 0;
        int lastIndex = weights.Length - 1;
        while (index < lastIndex)
        {
            // Do a probability check with a likelihood of weights[index] / weightSum.
            if (Random.Range(0, weightSum) < weights[index])
            {
                return index;
            }
 
            // Remove the last item from the sum of total untested weights and try again.
            weightSum -= weights[index++];
        }
 
        // No other item was selected, so return very last index.
        return index;
    }

    void InstantiatePipe(int[] current)
    {
        pipeCount += 1;
        var newPipe = Instantiate(Pipe, new Vector3(current[0], current[1]), Quaternion.identity);
        newPipe.transform.parent = this.transform;
        newPipe.name = "Pipe " + pipeCount;
    }

    void GenerateLine(int retry = 0)
    {
        try
        {
            PipeGenerator.pipeCount = 0;
            var current = StartPosition();
            var pipeCount = 0;
            InstantiatePipe(current);
            grid[current[0], current[1]] = ++pipeCount;
            
            
            bool stopOnYMax = current[1] == 0;

            while(true)
            {
                current = NextPipe(current);
                InstantiatePipe(current);
                grid[current[0], current[1]] = ++pipeCount;
                
                if (stopOnYMax && current[1] == GRID_SIZE - 1)
                {
                    break;
                }

                if (!stopOnYMax && current[0] == GRID_SIZE - 1)
                {
                    break;
                }
            }
        }
        catch (Exception e)
        {
            if (retry < 1000 && e.Message == "bad")
            {
                ClearChildren();
                GenerateLine(retry+1);
            }
            else
            {
                throw e;
            }
        }
    }



}
