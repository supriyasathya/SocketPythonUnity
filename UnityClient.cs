using System;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

/// <summary>
/// This script runs a client on Unity and connects to the server and received the float array sent by the server and
/// rotates te gameobject by the amount specified in the float array received.
/// This script can also been tested by deploying an app on Magicleap with this script attached to a gameobject.
/// </summary>

public class SendAndReceiveClient2 : MonoBehaviour
{
    TcpClient client;
    string receivedMessage;
    byte[] buf = new byte[256];

    // change this to IP address of the computer where the python server is running. 
    [SerializeField, Tooltip("The server IP address")]
    private string ipAddress = "127.0.0.1";
    [SerializeField, Tooltip("The server port")]
    // port at which the client will connect to the server
    private int port = 2020;
    // game object to which this script will be attached to
	public GameObject cubeGameObject;
    // rotArr is the rotation vector that saves the rotation values about X,Y and Z received.
	public float[] rotArr = new float[3];


	void Start() {
		connect2Server ();
	}


    public void connect2Server()
    {
        // tries to connect for 2 seconds and then throws an error if it cannot connect
        // taken adapted from https://social.msdn.microsoft.com/Forums/vstudio/en-us/2281199d-cd28-4b5c-95dc-5a888a6da30d/tcpclientconnect-timeout
        client = new TcpClient();
        IAsyncResult ar = client.BeginConnect(ipAddress, port, null, null);
        System.Threading.WaitHandle wh = ar.AsyncWaitHandle;
        try
        {
            if (!ar.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(2), false))
            {
                client.Close();
                throw new TimeoutException();
            }

			client.EndConnect(ar);

        }
        finally
        {
            wh.Close();
        }
        print("SOCKET READY");
    }

    void OnDestroy()
    {
        if (client.Connected)
        {
            client.Close();
        }
    }



    void Update()
    {

            if (!client.Connected)
                return; // early out to stop the function from running if client is disconnected

			if (Input.GetKeyDown ("space"))
				OnDestroy ();
		
            receivedMessage = "";

            // Set up async read
            var stream = client.GetStream();
            // read from the stream. Message_Received function is called during read
            stream.BeginRead(buf, 0, buf.Length, Message_Received, null);
            // rotate gameobject to which this script is attached by the amount specified in rotArr in degrees.
            cubeGameObject.transform.Rotate(rotArr[0], rotArr[1], rotArr[2], Space.Self);

    }
        // this function is called during stream.BeginRead.
    	void Message_Received(IAsyncResult res)
    {
		print ("came to Message_received code");
		if (res.IsCompleted && client.Connected)
        {
            var stream = client.GetStream();
            int bytesIn = stream.EndRead(res);
			print ("Server: Preparing to receive");
			// Read the received buffer and save the read float data in the three variables rotX, rotY and rotZ. Note that since
			// it is float data received, it will be 4 bytes per float. The numebr of bytes will change based on the type of the 
			//data that is received.
			while (true){ 
				receivedMessage = Encoding.ASCII.GetString (buf, 0, bytesIn);
                 
				float rotX = System.BitConverter.ToSingle (buf, 0);
				float rotY = System.BitConverter.ToSingle (buf, 4);
				float rotZ = System.BitConverter.ToSingle (buf, 8);
				print (rotX + " " + rotY + " " + rotZ + "\n");
				// save the read floats in an array rotArr.
				rotArr [0] = rotX;
				rotArr [1] = rotY;
				rotArr [2] = rotZ;

		
			}
        }
    }

}