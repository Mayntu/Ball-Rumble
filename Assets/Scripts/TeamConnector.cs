using UnityEngine;
using System;
using System.Text;
using System.Net;
using System.Net.Sockets;

public class TeamConnector {
    public class Requests {
        public static readonly string READY     = "ready";
        public static readonly string ACTIONS   = "actions";
        public static readonly string GAMEOVER  = "gameOver";
    }

    private string host;
    private int port;
    private UdpClient udp;
    private IPEndPoint endpoint;

    public TeamConnector(string host, int port) {
        this.host = host;
        this.port = port;
        udp = new(host, port);
        udp.Connect(host, port);
        endpoint = new(IPAddress.Any, port);
        Debug.Log($"New Team on {host}:{port}");
    }


    public delegate void ResponseHandler(string data);

    // sendRequest:
    // @reqType is one of static string constants of Requests class
    public void sendRequest(string reqType, string data, ResponseHandler responseHandler) {
        void receiver(IAsyncResult a_res) {
            byte[] buffer = udp.EndReceive(a_res, ref endpoint);
            if (buffer != null) {
                string response = Encoding.UTF8.GetString(buffer);
                if (responseHandler != null) {
                    responseHandler(response);
                }
            }
        }
        if (data == "") {
            data = "\"\"";
        }
        data = "{\"" + reqType + "\": " + data + "}";
        if (send(data)) {
            //Debug.Log("UDP data out: " + data);
            udp.BeginReceive(new AsyncCallback(receiver), null);
        }
    }


    private bool send(string data) {
        try {
            byte[] buffer = Encoding.UTF8.GetBytes(data);
            udp.Send(buffer, buffer.Length);
        }
        catch(Exception ex) {
            Debug.LogError(ex.Message);
            return false;
        }
        return true;
    }

}
