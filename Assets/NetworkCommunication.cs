using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

/* EGM */
using Abb.Egm;
using Google.Protobuf;
using System.Threading.Tasks;

#if !UNITY_EDITOR
/* Datagram Sockets */
using Windows.Networking;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;
#endif

public class NetworkCommunication : MonoBehaviour
{
    /* UDP port where EGM communication should happen (specified in RobotStudio) */
    public string port = "6511";
    /* Robot IP address over the network. */
    public string address = "192.168.0.5";
    /* Robot cartesian position and rotation values */
    public double x, y, z, rx, ry, rz;
    public double xi, yi, zi, rxi, ryi, rzi;
    /* Variable used to count the number of messages sent */
    public uint sequenceNumber = 0;
    /* Current state of EGM communication (disconnected, connected or running) */
    public string egmState = "Undefined";

#if !UNITY_EDITOR
    /* UDP socket */
    DatagramSocket socket;
    /* Robot IP address over the network. */
    public HostName robotAddress;
#endif

    public void StartCommunication()
    {
#if !UNITY_EDITOR
        CreateSocket();
#endif
    }

    public void UpdatePosition(double x, double y, double z, double rx, double ry, double rz)
    {
#if !UNITY_EDITOR
        SendPoseMessageToRobotAsync(x, y, z, rx, ry, rz);
#endif
    }

    public void MoveRobotHome()
    {
#if !UNITY_EDITOR
        SendPoseMessageToRobotAsync(xi, yi, zi, rxi, ryi, rzi);
#endif
    }

#if !UNITY_EDITOR
    private void CreateSocket()
    {
        robotAddress = new HostName(address);
        socket = new DatagramSocket();
        socket.MessageReceived += CollectRobotMessage; /* Listens to messages from robot */
        StartConnection();
        xi = x; yi = y; zi = z;
        rxi = rx; ryi = ry; rzi = rz;
    }

    private async void StartConnection()
    {
        /* We must specify which remote address we will listen to in order to receive messages. 
           Read more about how DatagramSocket works at:
           https://learn.microsoft.com/en-us/uwp/api/windows.networking.sockets.datagramsocket.connectasync*/

        await socket.ConnectAsync(new EndpointPair(null, port, robotAddress, port));
    }

    private void CollectRobotMessage(DatagramSocket sender, DatagramSocketMessageReceivedEventArgs args)
    {
        DataReader reader = args.GetDataReader();
        byte[] bytes = new byte[reader.UnconsumedBufferLength];
        reader.ReadBytes(bytes);

        /* If message isn't empty, parse message
           content using EGM */
        if (bytes != null)
        {
            /* De-serializes the byte array using the EGM protocol */
            EgmRobot message = EgmRobot.Parser.ParseFrom(bytes);

            ParseCurrentPositionFromMessage(message);
        }
    }

    private void ParseCurrentPositionFromMessage(EgmRobot message)
    {
        /* Parse the current robot position and EGM state from message
            received from robot and update the related variables */
        if (message.Header.HasSeqno && message.Header.HasTm)
        {
            x = message.FeedBack.Cartesian.Pos.X;
            y = message.FeedBack.Cartesian.Pos.Y;
            z = message.FeedBack.Cartesian.Pos.Z;
            rx = message.FeedBack.Cartesian.Euler.X;
            ry = message.FeedBack.Cartesian.Euler.Y;
            rz = message.FeedBack.Cartesian.Euler.Z;
            egmState = message.MciState.State.ToString();
        }
        else
        {
            Console.WriteLine("The message received from robot is invalid.");
        }
    }

    private async Task SendPoseMessageToRobotAsync(double x, double y, double z, double rx, double ry, double rz)
    {
        /* Send message containing new positions to robot in EGM format.
         * This is the primary method used to move the robot in cartesian coordinates. */

        /* Warning: If you are planning to manipulate an ABB robot with Hololens, this implementation
         * will not work. Hololens runs under Universal Windows Platform (UWP), which at the present
         * moment does not work with UdpClient class. DatagramSocket should be used instead. */

        using (MemoryStream memoryStream = new MemoryStream())
        {
            EgmSensor message = new EgmSensor();
            /* Prepare a new message in EGM format */
            CreatePoseMessage(message, x, y, z, rx, ry, rz);

            message.WriteTo(memoryStream);

            /* Sends the message asynchronously as a byte array over the network to the robot */
            using (IOutputStream robotOutputStream = await socket.GetOutputStreamAsync(robotAddress, port))
            {
                using (DataWriter writer = new DataWriter(robotOutputStream))
                {
                    writer.WriteBytes(memoryStream.ToArray());
                    await writer.StoreAsync();
                }
            }
        }
    }

    private void CreatePoseMessage(EgmSensor message, double x, double y, double z, double rx, double ry, double rz)
    {
        /* Create a message in EGM format specifying a new location to where
           the ABB robot should move to. The message contains a header with general
           information and a body with the planned trajectory.

           Notice that in order for this code to work, your robot must be running a EGM client 
           in RAPID containing EGMActPose and EGMRunPose methods.

           See one example here: https://github.com/vcuse/egm-for-abb-robots/blob/main/EGMPoseCommunication.modx */

        EgmHeader hdr = new EgmHeader();
        hdr.Seqno = sequenceNumber++;
        hdr.Tm = (uint)DateTime.Now.Ticks;
        hdr.Mtype = EgmHeader.Types.MessageType.MsgtypeCorrection;

        message.Header = hdr;
        EgmPlanned planned_trajectory = new EgmPlanned();
        EgmPose cartesian_pos = new EgmPose();
        EgmCartesian tcp_p = new EgmCartesian();
        EgmEuler ea_p = new EgmEuler();

        /* Translation values */
        tcp_p.X = x;
        tcp_p.Y = y;
        tcp_p.Z = z;

        /* Rotation values (in Euler angles) */
        ea_p.X = rx;
        ea_p.Y = ry;
        ea_p.Z = rz;

        cartesian_pos.Pos = tcp_p;
        cartesian_pos.Euler = ea_p;

        planned_trajectory.Cartesian = cartesian_pos;
        message.Planned = planned_trajectory;
    }
#endif
}
