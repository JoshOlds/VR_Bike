using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Target_Select_Generic : MonoBehaviour {

    public GameObject[] targets;
    private int[] targetSelectCount;
    public bool autoTarget;

    public bool debug = false;

    // Use this for initialization
    void Awake()
    {
        if (autoTarget)
        {
            targets = GameObject.FindGameObjectsWithTag("Target");
        }
        List<int> targetSelectList = new List<int>();
        for(int i = 0; i < targets.Length; i++)
        {
            targetSelectList.Add(0); //Initialize to all zeros
        }
        targetSelectCount = targetSelectList.ToArray();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public GameObject getRandomTarget()
    {
        int index = Random.Range(0, targets.Length);
        targetSelectCount[index]++;
        return targets[index];
    }

    public GameObject getDistributedTarget()
    {
        int min = targetSelectCount[0];
        List<GameObject> priorityTargetsList = new List<GameObject>();

        for(int i = 0; i < targetSelectCount.Length; i++)
        {
            int current = targetSelectCount[i];
            if(current < min)
            {
                min = current;
            }
        }
        for(int i = 0; i < targetSelectCount.Length; i++)
        {
            int current = targetSelectCount[i];
            if(current == min)
            {
                priorityTargetsList.Add(targets[i]);
            }
        }

        GameObject[] priorityTargets = priorityTargetsList.ToArray();
        int priorityIndex = Random.Range(0, priorityTargets.Length);
        GameObject target = priorityTargets[priorityIndex];
        int index = System.Array.IndexOf(targets, target);
        targetSelectCount[index]++;
        if (debug) { Debug.Log(targets[index].name + " has: " + targetSelectCount[index]); }
        return targets[index];
    }

}
