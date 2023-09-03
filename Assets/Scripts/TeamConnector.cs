using System;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;

public class TeamConnector {
    public enum Requests {
        READY,
        ACTIONS,
        GAMEOVER
    }

    private string[] request_names = new string[] { "ready", "actions", "gameover" };

    public string RequestName(Requests i) {
        return request_names[(int)i];
    }

    private enum State {
        CONNECT,
        READY,
        WAIT_FOR_ACTION,
        GAMEOVER,
        FAILED,
        ERROR
    }

    private string host;
    private int port;
    private State state;
    private UdpClient udp;
    private IPEndPoint endpoint;


    public TeamConnector(string host, int port) {
        this.host = host;
        this.port = port;
        state = State.CONNECT;
        udp = new(host, port);
        udp.Connect(host, port);
        endpoint = new(IPAddress.Any, port);
        Debug.Log("New Team on " + host + ":" + port.ToString());
    }


    public delegate void ResponseHandler(string data);
    public void sendRequest(Requests type, string data, ResponseHandler responseHandler) {
        void receiver(IAsyncResult ares) {
            //Debug.Log("UDP receiver called");
            byte[] buffer = udp.EndReceive(ares, ref endpoint);
            if (buffer != null) {
                string response = Encoding.UTF8.GetString(buffer);
                //Debug.Log("UDP got message: " + response);
                responseHandler(response);
            }
        }
        if (data == "") {
            data = "\"\"";
        }
        data = "{\"" + RequestName(type) + "\": " + data + "}";
        if (send(data)) {
            //Debug.Log("UDP data out: " + data);
            udp.BeginReceive(new AsyncCallback(receiver), null);
        }
    }



    private void worker() {
        switch(state) {
            case State.CONNECT:
                break;

            case State.READY:
                break;

            case State.WAIT_FOR_ACTION:
                break;

            case State.FAILED:
                break;

            case State.ERROR:
                break;

            default:
                break;
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
