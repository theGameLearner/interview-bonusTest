/*
 * Copyright (c) The Game Learner
 * https://connect.unity.com/u/rishabh-jain-1-1-1
 * https://www.linkedin.com/in/rishabh-jain-266081b7/
 * 
 * created on - #CREATIONDATE#
 */

using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[System.Serializable]
public class DataStructure 
{
    public int totalEarned;
    public int miniTotal;
    public int minorTotal;
    public int majorTotal;
    public int grandTotal;

    public string[] dictionarykeys;
    public jackpotState[] dictionaryValues;
    public bool scoreComplete;
}