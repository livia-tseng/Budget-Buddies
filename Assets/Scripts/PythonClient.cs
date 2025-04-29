using UnityEngine;
using System.Net.Sockets;
using System.Text;

public class PythonClient : MonoBehaviour
{
    private TcpClient client;
    private NetworkStream stream;

    private string serverIP = "127.0.0.1"; // Localhost
    private int serverPort = 9999; // Must match Python server port

    void Start()
    {
        ConnectToPython();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) // Send message when Space is pressed
        {
            SendMessageToPython("Hello from Unity!");
            ReceiveMessageFromPython();
        }
    }

    void ConnectToPython()
    {
        try
        {
            client = new TcpClient(serverIP, serverPort);
            stream = client.GetStream();
            Debug.Log("Connected to Python server.");
        }
        catch (System.Exception e)
        {
            Debug.LogError("Failed to connect: " + e.Message);
        }
    }

    void SendMessageToPython(string message)
    {
        byte[] messageBytes = Encoding.UTF8.GetBytes(message);
        stream.Write(messageBytes, 0, messageBytes.Length);
        Debug.Log("Sent to Python: " + message);
    }

    void ReceiveMessageFromPython()
    {
        byte[] data = new byte[256];
        int bytesRead = stream.Read(data, 0, data.Length);
        string response = Encoding.UTF8.GetString(data, 0, bytesRead);
        Debug.Log("Received from Python: " + response);
    }

    void OnApplicationQuit()
    {
        stream.Close();
        client.Close();
        Debug.Log("Disconnected.");

    }
}
