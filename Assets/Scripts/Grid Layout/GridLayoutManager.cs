/*
 * Copyright (c) The Game Learner
 * https://connect.unity.com/u/rishabh-jain-1-1-1
 * https://www.linkedin.com/in/rishabh-jain-266081b7/
 * 
 * created on - #CREATIONDATE#
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridLayoutManager : MonoBehaviour
{
    public Transform[] gridLayout;
    public int rowCount = 3;
    public int columnCount = 4;

    //singleton
    public static GridLayoutManager gridLayoutManager;

    //actual gridd with all values
    private Transform[,] awardGrid;
	//private Dictionary<string, Transform> gridMap = new Dictionary<string, Transform>();

	private void Awake()
	{
        if(gridLayoutManager != null && gridLayoutManager!= this)
		{
            Debug.LogError("more than one GridLayoutManager in scene");
            Destroy(gameObject);
            return;
        }
        gridLayoutManager = this;
    }

	void OnEnable()
    {
        awardGrid = new Transform[rowCount, columnCount];
        SetGridData();
        //SetGripMap();
    }

    /// <summary>
    /// sets data from 'gridLayout' to 'awardGrid'
    /// </summary>
    void SetGridData()
    {
        if (rowCount*columnCount != gridLayout.Length) 
        {
            Debug.LogError("the row x column is not equat to array size");
            return;
        }

        for(int i=0; i<rowCount; i++)
		{
            for(int j=0; j<columnCount; j++)
			{
                awardGrid[i,j] = gridLayout[(i * columnCount) + j];
            }
		}
        
        /*
        // to print the referenced transform
        for (int k = 0; k < rowCount; k++)
        {
            for (int l = 0; l < columnCount; l++)
            {
                Debug.Log("grid data[" + k + "][" + l + "] : " + awardGrid[k,l]);
            }
        }
        */
    }


    /*
    void SetGripMap()
	{
        for (int k = 0; k < rowCount; k++)
        {
            for (int l = 0; l < columnCount; l++)
            {
                //Debug.Log("grid data[" + k + "][" + l + "] : " + awardGrid[k, l]);
                gridMap.Add(k + "" + l, awardGrid[k, l]);
            }
        }
    }
    */


	#region getter_setter
    public void setGridElement(int rowVal, int columnVal, Transform newElement)
	{
        awardGrid[rowVal, columnVal] = newElement;
    }      
    
    public Transform getGridElement(int rowVal, int columnVal)
	{
        return awardGrid[rowVal, columnVal];
    }

    public Transform[,] getCompleteGrid()
	{
        return awardGrid;
    }
	#endregion getter_setter
}
