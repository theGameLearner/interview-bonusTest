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
using UnityEngine.UI;
using UnityEngine.XR;

public class AwardHandler : MonoBehaviour
{
    public static Color openAwardTintColor = new Color((133f/255f),(209f/255f), (209f/255f));

    private jackpotState myState = jackpotState.closed;
	//[HideInInspector]
	public int rowVal = -1;
	//[HideInInspector]
	public int columnVal = -1;

	private void Start()
	{
		Button awardButton = transform.GetComponent<Button>();
		if (awardButton != null)
		{
			awardButton.onClick.AddListener(
				delegate 
				{
					GameHandler.gameHandler.ChangeParameterFor(rowVal, columnVal, GetComponent<RectTransform>()); 
				});
		}
	}

	#region getter_setter
	public jackpotState getAwardState()
	{
        return myState;

    }

    public void setAwardState(jackpotState val)
	{
        //changing award state
        myState = val;
	}
	#endregion //getter_setter
}
