using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnOff : MonoBehaviour
{
    public bool begin = false;
    public bool initialized = false;

    //assign cube prefab as element for grid
    public GameObject cube;
    public Camera mainCam;

    //grid size: width and height
    public int width = 10;
    public int height = 10;

    //declare array for grid
    public GameObject[,] grid;
    public bool[,] tempGrid;

    //counts time
    public int timeCount = 0;
    //speed of evolution
    public int speedOfEvolution = 5;

    //UI elements
    public Text widthText;
    public Text heightText;
    public Slider widthSlider;
    public Slider heightSlider;
    //public Button beginButton;

    //material colors
    public Material violet;
    public Material indigo;
    public Material blue;
    public Material green;
    public Material yellow;
    public Material orange;
    public Material red;

    void Start()
    {
    

    }

    void Update()
    {
        if(initialized == false)
        {

            SelectParameters();

        }

        if(begin == true)
        {
            if(initialized == true)
            {
                mainCam.transform.position = new Vector3(((width * 3) - 2) / 2, ((height * 3) - 2) / 2, (width+height)*-1);

                //initilaize grid size
                grid = new GameObject[width, height];
                tempGrid = new bool[width, height];

                //fill each element of grid array with cube prefab
                for (int i = 0; i < width; i++)
                {
                    for (int j = 0; j < height; j++)
                    {
                        Vector2 gridPose = new Vector2(i * 2.0f, j * 2.0f);
                        grid[i, j] = Instantiate(cube);
                        grid[i, j].transform.Translate(gridPose);
                        grid[i, j].GetComponent<Renderer>().material = violet;

                        //randomly generate a new grid with some prefabs rendered and others not (on and off)
                        int randomSeed = UnityEngine.Random.Range(0, 2);
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

            initialized = false;

            }



            if (timeCount % speedOfEvolution == 0)
            {
                //variable for number of cube prefabs that have mesh renderer on relative to each element in the grid array
                int numberOn;

                //checks how many cube prefab mesh renderes are on
                for (int i = 0; i < width; i++)
                {
                    for (int j = 0; j < height; j++)
                    {
                        //returns number of cube prefab mesh renderes are on next to each element in the grid array
                        numberOn = CheckOnOff(i, j);
                        Debug.Log(numberOn);
                        ExecuteRules(numberOn, i, j);

                    }

                }
                FillArray();
            }
            timeCount++;

        }

    }


    public void SelectParameters()
    {
        width = Convert.ToInt32(widthSlider.value);
        height = Convert.ToInt32(heightSlider.value);

        widthText.text = "Width = " + width;
        heightText.text = "Height = " + height;

        

    }

    //Determines how many cubes are on or off relative to each each cube element in the grid array
    public int CheckOnOff(int x, int y)
    {
        int count = 0;

        //boundaries
        if (x == 0 || x == (width-1))
        {
            return 0;
        }
        if (y == 0 || y == (height-1))
        {
            return 0;
        }

        //check top left cell
        if (grid[x - 1, y + 1].GetComponent<MeshRenderer>().enabled == true)
        {
            count++;
        }
        //check top middle cell
        if (grid[x, y + 1].GetComponent<MeshRenderer>().enabled == true)
        {
            count++;
        }
        //check top right cell
        if (grid[x + 1, y + 1].GetComponent<MeshRenderer>().enabled == true)
        {
            count++;
        }
        //check middle left cell
        if (grid[x - 1, y].GetComponent<MeshRenderer>().enabled == true)
        {
            count++;
        }
        //check middle right cell
        if (grid[x + 1, y].GetComponent<MeshRenderer>().enabled == true)
        {
            count++;
        }
        //check bottom left cell
        if (grid[x - 1, y - 1].GetComponent<MeshRenderer>().enabled == true)
        {
            count++;
        }
        //check bottom right cell
        if (grid[x + 1, y - 1].GetComponent<MeshRenderer>().enabled == true)
        {
            count++;
        }

        return count;
    }

    public void ExecuteRules(int numberOn, int x, int y)
    {
        //Rule 1: Any live cell with fewer than two live neighbours dies, as if by underpopulation.
        if (numberOn < 2 && grid[x, y].GetComponent<MeshRenderer>().enabled == true)
        {
            tempGrid[x, y] = false;
        }
        //Rule 3: Any live cell with more than three live neighbours dies, as if by overpopulation.
        if (numberOn > 3 && grid[x, y].GetComponent<MeshRenderer>().enabled == true)
        {
            tempGrid[x, y] = false;
        }
        //Rule 4: Any dead cell with exactly three live neighbours becomes a live cell, as if by reproduction.
        if (numberOn == 3 && grid[x, y].GetComponent<MeshRenderer>().enabled == false)
        {
            tempGrid[x, y] = true;
        }
    }

    public void FillArray()
    { 
        for (int x = 0; x< width; x++)
        {
            for (int y = 0; y< height; y++)
            {
                grid[x, y].GetComponent<MeshRenderer>().enabled = tempGrid[x, y];
            }
        }
    }

    public void OnButtonClicked()
    {
        begin = true;
        initialized = true;
    }

}