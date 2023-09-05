using UnityEngine;
using System.IO;

public class FileLogger : MonoBehaviour
{
    private string logFilePath;
    private StreamWriter logStreamWriter;

    private void Start()
    {
        string logsDirectory = Path.Combine(Application.dataPath, "Logs");
        Directory.CreateDirectory(logsDirectory);
        logFilePath = Path.Combine(logsDirectory, "log.txt");

        logStreamWriter = File.AppendText(logFilePath);
    }

    private void OnDestroy()
    {
        if (logStreamWriter != null)
        {
            logStreamWriter.Close();
        }
    }

    public void Log(string message)
    {
        string formattedMessage = $"{System.DateTime.Now}: {message}\n";
        Debug.Log(formattedMessage);
        logStreamWriter.WriteLine(formattedMessage);
    }
}
