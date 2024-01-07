using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathManager : MonoBehaviour
{
    public List<Vector3> translationValues;
    public List<Quaternion> rotationValues;
    public float motionSpeed = 2.0f;
    public GameObject tool;
    public LineRenderer tracingLine;
    public bool isRecording = false;
    public bool isPlaying = false;
    public int currentTarget = 0;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (isRecording)
        {
            translationValues.Add(tool.transform.position);
            rotationValues.Add(tool.transform.rotation);
        }
        else if (isPlaying)
        {
            PlayMotionPath();
        }
    }

    public void ToggleRecording()
    {
        isRecording = isRecording is false? true: false;
    }

    public void TogglePlaying()
    {
        isPlaying = isPlaying is false ? true : false;
    }

    public void PlayMotionPath()
    {
        if (translationValues.Count > 0 && rotationValues.Count > 0)
        {
            Vector3 nextTranslation = Vector3.Lerp(tool.transform.position, translationValues[currentTarget], Time.deltaTime * motionSpeed);
            Quaternion nextRotation = Quaternion.Slerp(tool.transform.rotation, rotationValues[currentTarget], Time.deltaTime * motionSpeed);

            tool.transform.position = nextTranslation;
            tool.transform.rotation = nextRotation;

            if (Vector3.Distance(tool.transform.position, translationValues[currentTarget]) < 0.05f)
            {
                currentTarget = (currentTarget + 1) % translationValues.Count;
            }
        }
    }

    public void DisplayTracingLine()
    {
        tracingLine.positionCount = translationValues.Count;

        for (int i = 0; i < translationValues.Count; i++)
        {
            tracingLine.SetPosition(i, translationValues[i]);
        }
    }

    public void ClearTracingLine()
    {
        tracingLine.positionCount = 0;
    }

    public void ClearMotionPath()
    {
        currentTarget = 0;
        translationValues.Clear();
        rotationValues.Clear();
    }
}
