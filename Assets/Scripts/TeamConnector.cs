using UnityEngine;
using System;
using System.Text;
using System.Net;
using System.Net.Sockets;

public class TeamConnector {
    public delegate void ResponseHandler(string data);
    public class Requests {
        public static readonly string READY     = "ready";
        public static readonly string ACTIONS   = "actions";
        public static readonly string GAMEOVER  = "gameOver";
    }

    private string host;
    private int port;
    private UdpClient udp;
    private IPEndPoint endpoint;
    private string waitingForResponseType = "";
    private ResponseHandler errorHandler;

    public TeamConnector(string host, int port) {
        this.host = host;
        this.port = port;
        udp = new(host, port);
        udp.Connect(host, port);
        endpoint = new(IPAddress.Any, port);
        Debug.Log($"New Team on {host}:{port}");
    }


    public void setErrorHandler(ResponseHandler errorHandler) { 
        this.errorHandler = errorHandler;
    }


    // sendRequest:
    // @reqType is one of static string constants of Requests class
    public void sendRequest(string reqType, string data, ResponseHandler responseHandler) {
        void receiver(IAsyncResult a_res) {
            byte[] buffer = udp.EndReceive(a_res, ref endpoint);
            if (buffer != null) {
                waitingForResponseType = "";
                string response = Encoding.UTF8.GetString(buffer);
                if (responseHandler != null) {
                    responseHandler(response);
                }
            }
        }

        if (reqType == waitingForResponseType && reqType == Requests.ACTIONS) {
            errorHandler("disconnected or processing actions too long");
            return;
        }

        if (data == "") data = "\"\"";
        data = "{\"" + reqType + "\": " + data + "}";
        if (send(data)) {
            //Debug.Log("UDP data out: " + data);
            waitingForResponseType = reqType;
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
