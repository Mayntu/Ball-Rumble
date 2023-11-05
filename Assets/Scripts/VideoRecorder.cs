using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.ComponentModel;
using System.IO;
using UnityEditor;
using UnityEditor.Recorder;
using UnityEditor.Recorder.Encoder;
using UnityEditor.Recorder.Input;

public class VideoRecorder : MonoBehaviour
{
    [SerializeField] private string fileExtension;
    RecorderController m_RecorderController;
    public bool m_RecordAudio = true;
    internal MovieRecorderSettings m_Settings = null;
    private string filePath;
    
    private void Start()
    {
        m_Settings = ScriptableObject.CreateInstance<MovieRecorderSettings>();
        var fileName = m_Settings.OutputFile + fileExtension;
    }

    private void Update()
    {
        
    }

    private void Initialize()
    {
        var controllerSettings = ScriptableObject.CreateInstance<RecorderControllerSettings>();
        m_RecorderController = new RecorderController(controllerSettings);

        var mediaOutputFolder = new DirectoryInfo(Path.Combine(Application.dataPath, "..", "SampleRecordings"));

        m_Settings = ScriptableObject.CreateInstance<MovieRecorderSettings>();
        m_Settings.name = "VideoRecorder";
        m_Settings.Enabled = true;

        m_Settings.EncoderSettings = new CoreEncoderSettings
        {
            // EncodingQuality = CoreEncoderSettings.VideoEncodingQuality.Hight,
            EncodingQuality = CoreEncoderSettings.VideoEncodingQuality.Medium,
            // EncodingProfile = CoreEncoderSettings.H264EncodingProfile.Baseline,
            Codec = CoreEncoderSettings.OutputCodec.MP4
            // Codec = CoreEncoderSettings.OutputCodec.WEBM - мега качество
        };
        m_Settings.CaptureAlpha = true;

        m_Settings.ImageInputSettings = new GameViewInputSettings
        {
            OutputWidth = 1920,
            OutputHeight = 1080
        };

        // Simple file name (no wildcards) so that FileInfo constructor works in OutputFile getter.
        m_Settings.OutputFile = mediaOutputFolder.FullName + "/" + "video";

        // Setup Recording
        controllerSettings.AddRecorderSettings(m_Settings);
        controllerSettings.SetRecordModeToManual();
        controllerSettings.FrameRate = 60.0f;

        RecorderOptions.VerboseMode = false;
        m_RecorderController.PrepareRecording();
        m_RecorderController.StartRecording();

        Debug.Log("Started recording for file" +  mediaOutputFolder.FullName);
        filePath = m_Settings.OutputFile + fileExtension;
    }

    public void StartVideoCapture()
    {
        Initialize();
        GetFilePath();
    }
    
    public void StopVideoCapture()
    {
        m_RecorderController.StopRecording();
    }

    public string GetFilePath()
    {
        Debug.Log(filePath);
        return filePath;
    }
}
