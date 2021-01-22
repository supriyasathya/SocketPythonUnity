# SocketPythonUnity
This repository has a Python script that runs a TCP/IP server and a C# script that runs a TCP/IP client on Unity and connects with the server. 
script creates a TCP/IP socket and awaits for a client to connect to the socket. Once the clinet is connected, the script sends an array with 3 floats to the client code.

In order to run this script from command line, type: python ServerPython.py 

The C# script runs a client on Unity and connects to the server and received the float array sent by the server and
rotates te gameobject to which it is attached by the amount specified in the float array received.
This script can also been tested by deploying an app on Magicleap with this script attached to a gameobject.
