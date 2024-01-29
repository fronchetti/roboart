# RoboART: Artistic Robot Programming in Mixed Reality
Welcome to the replication package of our paper entitled "RoboART: Artistic Robot Programming in Mixed Reality", accepted at IEEE VR 2024.

### :warning: Warning 
The organization and authors of this repository are not liable for any consequential damage or injury that any code or information available in this repository may produce to you or others. The code available in this repository should be used only for reading purposes as different robots and settings may act differently during  program execution. Use the code and information available here at your own risk, and always make sure you are following all the safety procedures recommended by your robot manufacturer. Robots can be dangerous if used inappropriately, be careful!

### Installing
This project is divided into two parts:
- **HoloLens 2 + Unity:** The root folder of this repository contains a Hololens 2 (UWP) application made in Unity 2021.3.21f1. If you want to build it on your computer, make sure to install [Unity Hub](https://docs.unity3d.com/hub/manual/InstallHub.html), download this repository, and open this folder using the "Add from disk" option on Unity Hub. Unity should guide you through the necessary steps to make it work on your computer. Please also make sure you have all the [requirements](https://learn.microsoft.com/en-us/windows/mixed-reality/mrtk-unity/mrtk3-overview/getting-started/overview) installed for HoloLens 2 development. If you have never deployed a HoloLens 2 application before, I highly recommend you to set up a sample project following their [documentation](https://learn.microsoft.com/en-us/windows/mixed-reality/mrtk-unity/mrtk3-overview/getting-started/setting-up/setup-dev-env), and play with it before working on our application.
- RAPID: Inside this repository, you will also find a [source code](https://github.com/fronchetti/roboart/blob/main/EGMPoseCommunication.modx) written in RAPID that you should run in your ABB robot to establish the UDP communication with HoloLens 2. If you are unfamiliar with EGM communication, I highly recommend you start with this [tutorial](https://github.com/fronchetti/egm-for-abb-robots). In this secondary repository, you will learn how to set up a UDP device on your robot controller, create serialized messages using the Google Protocol Buffer, and more.

### Demonstration
Our paper also comes with a demonstration, click on the image below to watch it:

[![IMAGE ALT TEXT HERE](https://img.youtube.com/vi/LXDUrxYXdPw/0.jpg)](https://www.youtube.com/watch?v=LXDUrxYXdPw)

