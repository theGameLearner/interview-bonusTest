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

public class GameConstants : MonoBehaviour
{
    public static GameConstants gameConstants;
	private void Awake()
	{
        if (gameConstants != null && gameConstants != this)
        {
            Debug.LogError("more than one GameConstants in scene");
            Destroy(gameObject);
            return;
        }
        gameConstants = this;
    }

	public Transform awardMiniPrefab;
    public Transform awardMinorPrefab;
    public Transform awardMajorPrefab;
    public Transform awardGrandPrefab;
    public Transform awardClosedPregfab;
    public Transform winPanel;

    public Text totalText;
    public Text totalGrandText;
    public Text totalMajorText;
    public Text totalMinorText;
    public Text totalMiniText;
}
