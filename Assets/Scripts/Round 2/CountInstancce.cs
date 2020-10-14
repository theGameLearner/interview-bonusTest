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

public class CountInstancce : MonoBehaviour
{
    public int startIndex = 0;
    public int endIndex = 100;
    public int instanceToCount = 5;
    public int count = 0;


    public string strInstanceToCount = "";

    void Start()
    {
        strInstanceToCount = instanceToCount + "";
        int numberOfTen = (endIndex - startIndex) / 10; 
        count = numberOfTen; // each number of ten = 1 five
        int reminderTen = (endIndex - startIndex) % 10; //4

        for (int i = endIndex; i < endIndex + reminderTen; i++)
        {
            string str = (endIndex + i) + "";
            while(str.Length >0)
            {
                if (str.Contains(strInstanceToCount))
                {
                    count++; //counting all five in reminder even if present multiple times
                    str = str.Substring(str.IndexOf(strInstanceToCount));
                }
                else
				{
                    break;
				}
            }
        }

        int startTenInstance = instanceToCount * 10; //50
        int endTenInstance = ((instanceToCount + 1) * 10) - 1; //59
        if(startTenInstance >= startIndex && endTenInstance <= endIndex)
		{
            count += 10; //55 already included in 'numberOfTen'
        }
        else if(startTenInstance >= startIndex && endTenInstance > endIndex)
		{
            count += (endIndex - startTenInstance); // range starts at 25 & ends at 53 
		}
        else if(startTenInstance < startIndex && endTenInstance <= endIndex)
		{
            count += (endTenInstance - startIndex); //range starts from 54 to 76
        }

        Debug.Log("count : " + count);

    }

    void Update()
    {
        
    }
}
