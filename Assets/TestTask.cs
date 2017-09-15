using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTask : MonoBehaviour {

    float timeStamp1;
    float timeStamp2;
    float[] array1;
    float[] array2;

    public int countTrues = 0;
    public int countFalses = 0;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void setArray1(float[] array1)
    {
        this.array1 = array1;
        timeStamp1 = timeStamp2;
    }

    public void setArray2(float[] array2)
    {
        this.array2 = array2;
        timeStamp2 = Time.realtimeSinceStartup;
    }

    public float getTimeBetweenFrames()
    {

        return (timeStamp2 - timeStamp1);
    }

    public bool compareTwoArrays(float[] array1,float[] array2)
    {
        if (array1 == array2)
            return true;

        if (array1 == null || array2 == null)
            return false;

        if (array1.Length != array2.Length)
            return false;

        for (int i = 0; i < array1.Length; i++)
        {
            if (array1[i] != array2[i])
                return false;
        }
        return true;
    }

    public bool compareTwoArrays()
    {
        if (array1 == array2)
            return true;

        if (array1 == null || array2 == null)
            return false;

        if (array1.Length != array2.Length)
            return false;

        for (int i = 0; i < array1.Length; i++)
        {
            if (array1[i] != array2[i])
                return false;
        }
        return true;
    }


}
