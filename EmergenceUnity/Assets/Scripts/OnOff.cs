using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnOff : MonoBehaviour
{

	public GameObject cube;
	public GameObject[,] grid = new GameObject[10, 10];

	// Use this for initialization
	void Start()
	{
		for (int i = 0; i < 10; i++)
		{
			for (int j = 0; j < 10; j++)
			{
				Vector2 gridPose = new Vector2(i * 2.0f, j * 2.0f);
				//grid[i,j] = Instantiate(cube.transform, gridPose, Quaternion.identity);
				grid[i, j] = Instantiate(cube);
				grid[i, j].transform.Translate(gridPose);

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

	// Update is called once per frame
	void Update()
	{
		int numberOn;

		for (int i = 0; i < 10; i++)
		{
			for (int j = 0; j < 10; j++)
			{
				numberOn = CheckOnOff(i, j);
                Debug.Log(numberOn);
			}

		}
	}

    //Determines how many cubes are on or off
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
}