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
using UnityEngine.AI;

public class GameHandler : MonoBehaviour
{
	public static GameHandler gameHandler;

	private Transform[,] myGameGrid;
	public Dictionary<string, jackpotState> gridState = new Dictionary<string, jackpotState>();
	private int closedCount=0;
	private int miniCount = 0;
	private int minorCount = 0;
	private int majorCount = 0;
	private int grandCount = 0;

	private List<GameObject> createdObjects;

	private void Awake()
	{
		if (gameHandler != null && gameHandler != this)
		{
			Debug.LogError("more than one GameHandler in scene");
			Destroy(gameObject);
			return;
		}
		gameHandler = this;
		createdObjects = new List<GameObject>();
	}

	public void PopulateClosedGrid()
	{
		if(createdObjects != null && createdObjects.Count > 0)
		{
			for(int i=createdObjects.Count-1; i>-1; i--)
			{
				Destroy(createdObjects[i]);
			}
			createdObjects = new List<GameObject>();
		}
		FetchGridTransforms();
		InstantiateClosedAwards();
		SetClosedGridParameters();
	}

	void FetchGridTransforms()
	{
		myGameGrid = GridLayoutManager.gridLayoutManager.getCompleteGrid();
	}

	void InstantiateClosedAwards()
	{
		for (int i = 0; i < GridLayoutManager.gridLayoutManager.rowCount; i++)
		{
			for (int j = 0; j < GridLayoutManager.gridLayoutManager.columnCount; j++)
			{
				//create a new closed award
				GameObject newAward = Instantiate(
					GameConstants.gameConstants.awardClosedPregfab.gameObject, 
					GridLayoutManager.gridLayoutManager.transform);

				RectTransform newRectTransform = newAward.GetComponent<RectTransform>();
				RectTransform gridPosTransform = GridLayoutManager.gridLayoutManager.getGridElement(i, j).GetComponent<RectTransform>();
				newRectTransform.anchorMin = gridPosTransform.anchorMin;
				newRectTransform.anchorMax = gridPosTransform.anchorMax;
				newRectTransform.anchoredPosition = gridPosTransform.anchoredPosition;
				newRectTransform.sizeDelta = gridPosTransform.sizeDelta;

				if(!newAward.activeSelf)
				{
					newAward.SetActive(true);
				}

				newRectTransform.GetComponent<AwardHandler>().rowVal = i;
				newRectTransform.GetComponent<AwardHandler>().columnVal = j;
			}
		}
	}

	void SetClosedGridParameters()
	{
		gridState.Clear();
		for (int i = 0; i < GridLayoutManager.gridLayoutManager.rowCount; i++)
		{
			for (int j = 0; j < GridLayoutManager.gridLayoutManager.columnCount; j++)
			{
				gridState.Add(i + "" + j, jackpotState.closed);
			}
		}
		closedCount = GridLayoutManager.gridLayoutManager.rowCount * GridLayoutManager.gridLayoutManager.columnCount;
		miniCount = 0;
		minorCount = 0;
		majorCount = 0;
		grandCount = 0;
	}

	public void ChangeParameterFor(int rowVal, int columnVal, RectTransform orignalRectTransform)
	{
		jackpotState newState = DecideNewParameter();
		ShowNewAward(rowVal,columnVal, orignalRectTransform,newState);

		CheckGridForRewards();
	}	

	private jackpotState DecideNewParameter()
	{
		float rand = Random.value;
		if(rand <= 0.5f)
		{
			return jackpotState.Mini;
		}
		else if(0.5f < rand && rand <= 0.75f)
		{
			return jackpotState.Minor;
		}
		else if (0.75f < rand && rand <= 0.95f)
		{
			return jackpotState.Major;
		}
		else
		{
			return jackpotState.Grand;
		}
	}

	private void ShowNewAward(int origRowVal, int origColumnVal, RectTransform orignalRectTransform, jackpotState newState)
	{
		GameObject newAward = gameObject;
		RectTransform newRectTransform = GetComponent<RectTransform>();
		switch (newState)
		{
			case jackpotState.Mini:
				closedCount--;
				miniCount++;
				newAward = Instantiate(
					GameConstants.gameConstants.awardMiniPrefab.gameObject,
					GridLayoutManager.gridLayoutManager.transform);
				newRectTransform = newAward.GetComponent<RectTransform>();
				break;
			case jackpotState.Minor:
				closedCount--;
				minorCount++;
				newAward = Instantiate(
					GameConstants.gameConstants.awardMinorPrefab.gameObject,
					GridLayoutManager.gridLayoutManager.transform);
				newRectTransform = newAward.GetComponent<RectTransform>();
				break;
			case jackpotState.Major:
				closedCount--;
				majorCount++;
				newAward = Instantiate(
					GameConstants.gameConstants.awardMajorPrefab.gameObject,
					GridLayoutManager.gridLayoutManager.transform);
				newRectTransform = newAward.GetComponent<RectTransform>();
				break;
			case jackpotState.Grand:
				closedCount--;
				grandCount++;
				newAward = Instantiate(
					GameConstants.gameConstants.awardGrandPrefab.gameObject,
					GridLayoutManager.gridLayoutManager.transform);
				newRectTransform = newAward.GetComponent<RectTransform>();
				break;
		}
		createdObjects.Add(newAward);
		newRectTransform.anchorMin = orignalRectTransform.anchorMin;
		newRectTransform.anchorMax = orignalRectTransform.anchorMax;
		newRectTransform.anchoredPosition = orignalRectTransform.anchoredPosition;
		newRectTransform.sizeDelta = orignalRectTransform.sizeDelta;

		gridState[origRowVal + "" + origColumnVal] = newState;

		if (!newAward.activeSelf)
		{
			newAward.SetActive(true);
		}

		//newRectTransform.GetComponent<AwardHandler>().rowVal = origRowVal;
		//newRectTransform.GetComponent<AwardHandler>().columnVal = origColumnVal;
		Destroy(orignalRectTransform.gameObject);
	}

	void CheckGridForRewards()
	{
		if (miniCount >= 3)
		{
			Debug.Log("MINI Reward - 100k = 100000");
			GameManager.gameManager.SaveJsonData(true, jackpotState.Mini);
			GameConstants.gameConstants.winPanel.GetComponent<WinPanelManager>().winText.text = "MINI Reward \n +100000";
			GameConstants.gameConstants.winPanel.gameObject.SetActive(true);
		}
		else if (minorCount >= 3)
		{
			Debug.Log("MINOR Reward- 200k = 200000");
			GameManager.gameManager.SaveJsonData(true, jackpotState.Minor);
			GameConstants.gameConstants.winPanel.GetComponent<WinPanelManager>().winText.text = "MINOR Reward \n +200000";
			GameConstants.gameConstants.winPanel.gameObject.SetActive(true);
		}
		else if (majorCount >= 3)
		{
			Debug.Log("MAJOR Reward - 500k = 500000");
			GameManager.gameManager.SaveJsonData(true, jackpotState.Major);
			GameConstants.gameConstants.winPanel.GetComponent<WinPanelManager>().winText.text = "MAJOR Reward \n +500000";
			GameConstants.gameConstants.winPanel.gameObject.SetActive(true);
		}
		else if (grandCount >= 3)
		{
			Debug.Log("GRAND Reward - 1M = 1000000");
			GameManager.gameManager.SaveJsonData(true, jackpotState.Grand);
			GameConstants.gameConstants.winPanel.GetComponent<WinPanelManager>().winText.text = "GRAND Reward \n +1000000";
			GameConstants.gameConstants.winPanel.gameObject.SetActive(true);
		}
		else
		{
			GameManager.gameManager.SaveJsonData(false, jackpotState.closed);
		}
	}



	public void SetDataInGrid()
	{
		FetchGridTransforms();
		InstantiateGridAwards();

	}


	void InstantiateGridAwards()
	{
		closedCount = 0;
		miniCount = 0;
		minorCount = 0;
		majorCount = 0;
		grandCount = 0;
		for (int i = 0; i < GridLayoutManager.gridLayoutManager.rowCount; i++)
		{
			for (int j = 0; j < GridLayoutManager.gridLayoutManager.columnCount; j++)
			{
				Transform tartetTransform = GameConstants.gameConstants.awardClosedPregfab;
				Debug.Log("testing for key " + (i + "" + j));
				Debug.Log("grid state has " + gridState.Count + " elements");
				switch(gridState[(i + "" + j)])
				{
					case jackpotState.closed:
						tartetTransform = GameConstants.gameConstants.awardClosedPregfab;
						closedCount++;
						break;
					case jackpotState.Mini:
						tartetTransform = GameConstants.gameConstants.awardMiniPrefab ;
						miniCount++;
						break;
					case jackpotState.Minor:
						tartetTransform = GameConstants.gameConstants.awardMinorPrefab ;
						minorCount++;
						break;
					case jackpotState.Major:
						tartetTransform = GameConstants.gameConstants.awardMajorPrefab;
						majorCount++;
						break;
					case jackpotState.Grand:
						tartetTransform = GameConstants.gameConstants.awardGrandPrefab;
						grandCount++;
						break;
					default:
						tartetTransform = GameConstants.gameConstants.awardClosedPregfab;
						closedCount++;
						Debug.Log("unable to find the key : " + (i + "" + j));

						break;
				}

				//create a new closed award
				GameObject newAward = Instantiate(
					tartetTransform.gameObject,
					GridLayoutManager.gridLayoutManager.transform);

				RectTransform newRectTransform = newAward.GetComponent<RectTransform>();
				RectTransform gridPosTransform = GridLayoutManager.gridLayoutManager.getGridElement(i, j).GetComponent<RectTransform>();
				newRectTransform.anchorMin = gridPosTransform.anchorMin;
				newRectTransform.anchorMax = gridPosTransform.anchorMax;
				newRectTransform.anchoredPosition = gridPosTransform.anchoredPosition;
				newRectTransform.sizeDelta = gridPosTransform.sizeDelta;
				createdObjects.Add(newAward);

				if (!newAward.activeSelf)
				{
					newAward.SetActive(true);
				}
				AwardHandler newAwardHandler = newRectTransform.GetComponent<AwardHandler>();
				if(newAwardHandler != null)
				{
					newRectTransform.GetComponent<AwardHandler>().rowVal = i;
					newRectTransform.GetComponent<AwardHandler>().columnVal = j;
				}
			}
		}
	}
}
