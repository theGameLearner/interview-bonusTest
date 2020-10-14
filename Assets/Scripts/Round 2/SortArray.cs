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

public class SortArray : MonoBehaviour
{
    public int[] inpArr= {0,0,1,0,1,1,2,1,0,2,2,0};
    int temp;
    void Start()
    {
        SortArr();
    }

    void SortArr()
	{
        for(int i = 0; i< inpArr.Length-1;i++)
		{

            for(int j=i+1; j< inpArr.Length; j++)
			{
                if(inpArr[i]<inpArr[j])
				{
                    temp = inpArr[i];
                    inpArr[i] = inpArr[j];
                    inpArr[j] = temp;
                }
			}
		}

        for(int k=0; k<inpArr.Length; k++)
        {
            Debug.Log(" i as "+k+" has value "+ inpArr[k]);
        }
	}
}
