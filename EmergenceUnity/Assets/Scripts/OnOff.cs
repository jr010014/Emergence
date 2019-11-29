using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnOff : MonoBehaviour
{
    public bool begin = false;
    public bool ranOnce = false;

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
    public int lengthOfEra = 50;

    //UI elements
    public Text widthText;
    public Text heightText;
    public Text eraText;
    public Slider widthSlider;
    public Slider heightSlider;
    public Slider eraSlider;
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
        if(begin == false)
        {

            SelectParameters();

        }

        if (begin == true)
        {  
            //set up
            if(ranOnce == false)
            {
                mainCam.transform.position = new Vector3(((width * 3) - 2) / 2, ((height * 3) - 2) / 2, (width + height) * -1);

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
                        //grid[i, j].GetComponent<Renderer>().material = violet;


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
                ranOnce = true;
            }

            CheckColors();
            Debug.Log(lengthOfEra);

            if (timeCount % lengthOfEra == 0)
            {
                //variable for number of cube prefabs that have mesh renderer on relative to each element in the grid array
                int numberOn;

                //checks how many cube prefab mesh renderes are on
                for (int i = 0; i < width; i++)
                {
                    for (int j = 0; j < height; j++)
                    {
                        //returns number of cube prefab mesh renderers are on next to each element in the grid array
                        numberOn = CheckOnOff(i, j);
                        Debug.Log(timeCount);
                        //if (grid[i, j].GetComponent<Renderer>().material == violet)
                        //{
                            ExecuteLifeRules(numberOn, i, j);
                            Debug.Log("here");
                        //}
  
                     }
                }
                FillArray();
            }
            timeCount++;
        }
            
        

    }
    

    //allows user to select parameter via the user interface
    public void SelectParameters()
    {
        width = Convert.ToInt32(widthSlider.value);
        height = Convert.ToInt32(heightSlider.value);
        lengthOfEra = Convert.ToInt32(eraSlider.value);

        widthText.text = "Width = " + width;
        heightText.text = "Height = " + height;
        eraText.text = "Length of Era = " + lengthOfEra;

        

    }

    //Determines how many cubes are on or off relative to each each cube element in the grid array add returns that number to int count
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

    //executes rules to determine if cells/cubes should be on or off for next generation and loads on or off bool values into a seperate grid array without alterting current generation
    public void ExecuteLifeRules(int numberOn, int x, int y)
    {
        Debug.Log("here2");

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

    //adjust new grid based on population of old grid
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
        timeCount = 0;  //resets timeCount in the case that the user resets the game
        begin = true;
    }
    

    //determine if colors of cubes should be changed and if so, change them
    public void CheckColors()
    {
        if (timeCount == 0)
        {
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    grid[i, j].GetComponent<Renderer>().material = violet;
                }
            }
            Debug.Log("turned violet");
        }
        else
        {
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    if (grid[i, j].GetComponent<Renderer>().material == violet && timeCount > 30)
                    {
                        grid[i, j].GetComponent<Renderer>().material = indigo;
                        Debug.Log("turned indigo");
                    }
                    else if (grid[i, j].GetComponent<Renderer>().material == indigo && timeCount > 60)
                    {
                        grid[i, j].GetComponent<Renderer>().material = blue;
                        Debug.Log("turned blue");
                    }
                    else if (grid[i, j].GetComponent<Renderer>().material == blue && timeCount > 90)
                    {
                        grid[i, j].GetComponent<Renderer>().material = green;
                        Debug.Log("turned green");
                    }
                    else if (grid[i, j].GetComponent<Renderer>().material == green && timeCount > 120)
                    {
                        grid[i, j].GetComponent<Renderer>().material = yellow;
                        Debug.Log("turned yellow");
                    }
                    else if (grid[i, j].GetComponent<Renderer>().material == yellow && timeCount > 150)
                    {
                        grid[i, j].GetComponent<Renderer>().material = orange;
                        Debug.Log("turned orange");
                    }
                    else if (grid[i, j].GetComponent<Renderer>().material == orange && timeCount > 180)
                    {
                        grid[i, j].GetComponent<Renderer>().material = red;
                        Debug.Log("turned red");
                    }
                }
     

            }
        }
        
    }

}