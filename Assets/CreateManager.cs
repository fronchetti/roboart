using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateManager : MonoBehaviour
{
    public GameObject targetPrefab;
    public List<GameObject> targets;
    public float motionSpeed = 2.0f;
    public GameObject tool;
    public LineRenderer tracingLine;
    public bool isPlaying = false;
    public int currentTarget = 0;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlaying)
        {
            PlayMotionPath();
        }
    }

    public void CreateTarget()
    {
        GameObject target = Instantiate(targetPrefab, tool.transform.position, tool.transform.rotation);
        target.transform.parent = transform;
        targets.Add(target);
        UpdateTracingLine();
    }

    public void RemoveTarget()
    {
        if (targets.Count > 0)
        {
            GameObject target = targets[targets.Count - 1];
            targets.RemoveAt(targets.Count - 1);
            Destroy(target);
            UpdateTracingLine();
        }
    }

    public void ClearTargets()
    {
        foreach (GameObject target in targets)
        {
            Destroy(target);
        }

        targets.Clear();
        UpdateTracingLine();
    }

    public void TogglePlaying()
    {
        isPlaying = isPlaying is false ? true : false;
    }

    public void UpdateTracingLine()
    {
        tracingLine.positionCount = targets.Count;

        for (int i = 0; i < targets.Count; i++)
        {
            tracingLine.SetPosition(i, targets[i].transform.position);
        }
    }

    
    public void PlayMotionPath()
    {
        if (targets.Count > 0)
        {
            Vector3 nextTranslation = Vector3.Lerp(tool.transform.position, targets[currentTarget].transform.position, Time.deltaTime * motionSpeed);
            Quaternion nextRotation = Quaternion.Slerp(tool.transform.rotation, targets[currentTarget].transform.rotation, Time.deltaTime * motionSpeed);

            tool.transform.position = nextTranslation;
            tool.transform.rotation = nextRotation;

            if (Vector3.Distance(tool.transform.position, targets[currentTarget].transform.position) < 0.05f)
            {
                currentTarget = (currentTarget + 1) % targets.Count;
            }
        }
    }

    public void ClearMotionPath()
    {
        currentTarget = 0;
    }
    
}
