using MixedReality.Toolkit.SpatialManipulation;
using MixedReality.Toolkit.UX;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveRobot : MonoBehaviour
{
    public GameObject tool;
    private NetworkCommunication socket;
    private Vector3 lastPosition;
    public bool isMovingEnabled = true;

    void Start()
    {
        lastPosition = tool.transform.position;
        socket = gameObject.GetComponent<NetworkCommunication>();
    }

    public void ToggleRobotMovement()
    {
        isMovingEnabled = isMovingEnabled is false ? true : false;
    }

    void Update()
    {
        if (tool.transform.hasChanged)
        {
            Vector3 displacement = lastPosition - tool.transform.position;

            if (isMovingEnabled)
            {
                socket.UpdatePosition(socket.x + (displacement.z * 15000), socket.y + -(displacement.x * 15000), socket.z + -(displacement.y * 15000), socket.rx, socket.ry, socket.rz);
            }

            lastPosition = tool.transform.position;
            tool.transform.hasChanged = false;
        }
    }
}
