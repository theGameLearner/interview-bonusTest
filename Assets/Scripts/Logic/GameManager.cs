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
using System.IO;
using System.Linq;
using System;

public class GameManager : MonoBehaviour
{
	public static GameManager gameManager;

    public string jsonPath;
    int currTotal = 0;
    int currMiniTotal = 0;
    int currMinorTotal = 0;
    int currMajorTotal = 0;
    int currGrandTotal = 0;


    private void Awake()
    {
        jsonPath = Path.Combine(Application.persistentDataPath, "data.json");

        if (gameManager != null && gameManager != this)
        {
            Debug.LogError("more than one GameManager in scene");
            Destroy(gameObject);
            return;
        }
        gameManager = this;
    }

	private void Start()
	{
        //Invoke(nameof(StartGame), 1);
        StartGame();
    }

    void StartGame()
	{
        if (System.IO.File.Exists(jsonPath))
        {
            LoadJsonData();

        }
        else
        {
            GameHandler.gameHandler.PopulateClosedGrid();
            SetUiTotal();
        }
    }

    void LoadJsonData()
	{
        string loadedJsonDataString = File.ReadAllText(jsonPath);
        DataStructure jsonData = JsonUtility.FromJson<DataStructure>(loadedJsonDataString);
        currGrandTotal = jsonData.grandTotal;
        currMajorTotal = jsonData.majorTotal;
        currMiniTotal = jsonData.miniTotal;
        currMinorTotal = jsonData.minorTotal;
        currTotal = jsonData.totalEarned;
        SetUiTotal();

        if (jsonData.scoreComplete)
		{
            GameHandler.gameHandler.PopulateClosedGrid();
            return;
        }

        GameHandler.gameHandler.gridState.Clear();
        for (int i = 0; i < GridLayoutManager.gridLayoutManager.rowCount; i++)
        {
            for (int j = 0; j < GridLayoutManager.gridLayoutManager.columnCount; j++)
            {
                GameHandler.gameHandler.gridState.Add(
                    (string)(i+""+j)
                    ,
                    jsonData.dictionaryValues[(GridLayoutManager.gridLayoutManager.rowCount * i) + j]
                    );

				Debug.Log("grid value for key (" + i + "," + j + ") is " + GameHandler.gameHandler.gridState[i + "" + j]);
			}
		}

        for(int k =0; k< GameHandler.gameHandler.gridState.Count; k++)
		{
            Debug.LogWarning(GameHandler.gameHandler.gridState.Values.ElementAt(k));

        }

        GameHandler.gameHandler.SetDataInGrid();
    }

    public void SaveJsonData(bool rewardsAssigned, jackpotState newState)
	{
        DataStructure newJsonData = new DataStructure();

        //Set Json Data in new variable
        if(rewardsAssigned)
		{
            switch(newState)
            {
                case jackpotState.Mini:
                    currMiniTotal += 100000;
                    currTotal += 100000;
                    break;
                case jackpotState.Minor:
                    currMinorTotal += 200000;
                    currTotal += 200000;
                    break;
                case jackpotState.Major:
                    currMajorTotal += 500000;
                    currTotal += 500000;
                    break;
                case jackpotState.Grand:
                    currGrandTotal += 1000000;
                    currTotal += 1000000;
                    break;
            }
		}

        newJsonData.totalEarned = currTotal;
        newJsonData.miniTotal = currMiniTotal;
        newJsonData.minorTotal = currMinorTotal;
        newJsonData.majorTotal = currMajorTotal;
        newJsonData.grandTotal = currGrandTotal;
        SetUiTotal();

        newJsonData.dictionarykeys = new string[
            GridLayoutManager.gridLayoutManager.rowCount * GridLayoutManager.gridLayoutManager.columnCount
            ];
        newJsonData.dictionaryValues = new jackpotState[
            GridLayoutManager.gridLayoutManager.rowCount * GridLayoutManager.gridLayoutManager.columnCount
            ];

  //      for (int i=0; i< GridLayoutManager.gridLayoutManager.rowCount; i++)
		//{
  //          for(int j=0; j<GridLayoutManager.gridLayoutManager.columnCount; j++)
		//	{
  //              newJsonData.dictionarykeys[(GridLayoutManager.gridLayoutManager.rowCount * i) + j] = (string)(i+""+j) ;
  //              newJsonData.dictionaryValues[(GridLayoutManager.gridLayoutManager.rowCount * i) + j] = GameHandler.gameHandler.gridState[i + "" + j];
  //              Debug.Log("saving ("+i+", "+j+"), kay as : " + 
  //                  newJsonData.dictionarykeys[(GridLayoutManager.gridLayoutManager.rowCount * i) + j]+
  //                  " : and value as : " +
  //                  newJsonData.dictionaryValues[(GridLayoutManager.gridLayoutManager.rowCount * i) + j]);
  //          }
		//}

        for (int i = 0; i < GridLayoutManager.gridLayoutManager.rowCount; i++)
        {
            for (int j = 0; j < GridLayoutManager.gridLayoutManager.columnCount; j++)
            {
                newJsonData.dictionarykeys[(GridLayoutManager.gridLayoutManager.rowCount * i) + j] = (string)(i + "" + j);
                newJsonData.dictionaryValues[(GridLayoutManager.gridLayoutManager.rowCount * i) + j] = GameHandler.gameHandler.gridState[i + "" + j];
                Debug.Log("saving (" + i + ", " + j + "), kay as : " +
                    newJsonData.dictionarykeys[(GridLayoutManager.gridLayoutManager.rowCount * i) + j] +
                    " : and value as : " +
                    newJsonData.dictionaryValues[(GridLayoutManager.gridLayoutManager.rowCount * i) + j]);
            }
        }

        //Debug.Log("saving keys, value as : " + newJsonData.dictionarykeys);
        //Debug.Log("saving values as : " + newJsonData.dictionaryValues);
        newJsonData.scoreComplete = rewardsAssigned;


        //save new data in file
        string jsonDataString = JsonUtility.ToJson(newJsonData, true);
        File.WriteAllText(jsonPath, jsonDataString);

    }


    void SetUiTotal()
	{

        GameConstants.gameConstants.totalText.text = "final : " + currTotal;
        GameConstants.gameConstants.totalGrandText.text = "grand : " + currGrandTotal;
        GameConstants.gameConstants.totalMajorText.text = "major : " + currMajorTotal;
        GameConstants.gameConstants.totalMinorText.text = "minor : " + currMinorTotal;
        GameConstants.gameConstants.totalMiniText.text = "mini : " + currMiniTotal;
    }
}
