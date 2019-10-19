using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnOff : MonoBehaviour
{
    //assign cube prefab as element for grid
	public GameObject cube;

    public int width = 10;
    public int height = 10;

    //declare array for grid
	public GameObject[,] grid = new GameObject[10, 10];

	void Start()
	{
        //fill each element of grid array with cube prefab
		for (int i = 0; i < 10; i++)
		{
			for (int j = 0; j < 10; j++)
			{
				Vector2 gridPose = new Vector2(i * 2.0f, j * 2.0f);
				//grid[i,j] = Instantiate(cube.transform, gridPose, Quaternion.identity);
				grid[i, j] = Instantiate(cube);
				grid[i, j].transform.Translate(gridPose);

                //randomly generate a new grid with some prefabs rendered and others not (on and off)
				int randomSeed = Random.Range(0, 2);
				if (randomSeed == 0)
				{
					grid[i, j].GetComponent<MeshRenderer>().enabled = true;
				}
				else
				{
					grid[i, j].GetComponent<MeshRenderer>().enabled = false;
				}
			}
		}
	}

	void Update()
	{
        //variable for number of cube prefabs that have mesh renderer on relative to each element in the grid array
		int numberOn;

        //checks how many cube prefab mesh renderes are on
		for (int i = 0; i < 10; i++)
		{
			for (int j = 0; j < 10; j++)
			{
                //returns number of cube prefab mesh renderes are on next to each element in the grid array
				numberOn = CheckOnOff(i, j);
                Debug.Log(numberOn);
                ExecuteRules(numberOn, i, j);
			}

		}
	}

    //Determines how many cubes are on or off relative to each each cube element in the grid array
    public int CheckOnOff(int x, int y)
	{
		int count = 0;

        if(x == 0 || x == 9)
		{
            return 0;
		}
        if(y == 0 || y == 9)
		{
            return 0;
		}

        //check top left cell
        if(grid[x-1, y+1].GetComponent<MeshRenderer>().enabled == true)
		{
			count++;
		}
        //check top middle cell
        if (grid[x, y+1].GetComponent<MeshRenderer>().enabled == true)
        {
            count++;
        }
        //check top right cell
        if (grid[x+1, y+1].GetComponent<MeshRenderer>().enabled == true)
        {
            count++;
        }
        //check middle left cell
        if (grid[x-1, y].GetComponent<MeshRenderer>().enabled == true)
        {
            count++;
        }
        //check middle right cell
        if (grid[x+1, y].GetComponent<MeshRenderer>().enabled == true)
        {
            count++;
        }
        //check bottom left cell
        if (grid[x-1, y-1].GetComponent<MeshRenderer>().enabled == true)
        {
            count++;
        }
        //check bottom right cell
        if (grid[x+1, y-1].GetComponent<MeshRenderer>().enabled == true)
        {
            count++;
        }

        return count;
	}

    public void ExecuteRules(int numberOn, int x, int y)
    {
        //Rule 1: Any live cell with fewer than two live neighbours dies, as if by underpopulation.
        if (numberOn < 2)
        {
            grid[x, y].GetComponent<MeshRenderer>().enabled = false;
        }
        //Rule 2: Any live cell with two or three live neighbours lives on to the next generation.
        if (numberOn == 2 || numberOn == 3)
        {
            grid[x, y].GetComponent<MeshRenderer>().enabled = true;
        }
        //Rule 3: Any live cell with more than three live neighbours dies, as if by overpopulation.
        if (numberOn > 3)
        {
            grid[x, y].GetComponent<MeshRenderer>().enabled = false;
        }
        //Rule 4: Any dead cell with exactly three live neighbours becomes a live cell, as if by reproduction.
        if (numberOn > 3 && grid[x, y].GetComponent<MeshRenderer>().enabled == false)
        {
            grid[x, y].GetComponent<MeshRenderer>().enabled = true;
        }
    }

}