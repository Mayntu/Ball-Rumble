using System.Collections;
using UnityEngine;
using Evereal.VideoCapture;
using Evereal.VideoCapture;

public class VideoRecorder : MonoBehaviour
{
    [SerializeField] private GameObject vc;
    private void Start()
    {
        
    }

    public void StartRecording()
    {
        vc.GetComponent<VideoCapture>().StartCapture();
    }

    public void StopRecording()
    {
        vc.GetComponent<VideoCapture>().StopCapture();
    }
    
    public string GetFilePath() 
    { 
        Debug.Log(VideoCache.lastVideoFile); 
        return VideoCache.lastVideoFile; 
    }
}
