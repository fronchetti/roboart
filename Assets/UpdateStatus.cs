using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpdateStatus : MonoBehaviour
{
    private NetworkCommunication socket;
    public GameObject connectionField;
    public GameObject translationField;
    public GameObject rotationField;

    void Start()
    {
        socket = gameObject.GetComponent<NetworkCommunication>();
    }

    void Update()
    {
        if (connectionField.GetComponent<TextMeshProUGUI>().text != socket.egmState)
        {
            connectionField.GetComponent<TextMeshProUGUI>().text = socket.egmState;
        }

        if (translationField.GetComponent<TextMeshProUGUI>().text != "X: " + (int)Math.Round(socket.x) + ", Y: " + (int)Math.Round(socket.y) + ", Z: " + (int)Math.Round(socket.z))
        {
            translationField.GetComponent<TextMeshProUGUI>().text = "X: " + (int)Math.Round(socket.x) + ", Y: " + (int)Math.Round(socket.y) + ", Z: " + (int)Math.Round(socket.z);

        }

        if (rotationField.GetComponent<TextMeshProUGUI>().text != "X: " + (int)Math.Round(socket.rx) + ", Y: " + (int)Math.Round(socket.ry) + ", Z: " + (int)Math.Round(socket.rz))
        {
            rotationField.GetComponent<TextMeshProUGUI>().text = "X: " + (int)Math.Round(socket.rx) + ", Y: " + (int)Math.Round(socket.ry) + ", Z: " + (int)Math.Round(socket.rz);
        }
    }
}
