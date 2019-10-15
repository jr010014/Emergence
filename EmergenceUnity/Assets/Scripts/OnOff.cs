using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnOff : MonoBehaviour {

	public GameObject cube;

	// Use this for initialization
	void Start () {
        for(int i = 0; i < 10; i++)
		{
            for(int j = 0; j < 10; j++)
			{
				Instantiate(cube.transform, new Vector3(i * 2.0f, j * 2.0f, 0), Quaternion.identity);
			}			
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
