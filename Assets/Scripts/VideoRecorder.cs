using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using NatSuite.Recorders;
using NatSuite.Recorders.Clocks;
using NatSuite.Recorders.Inputs;

public class VideoRecorder : MonoBehaviour
{
    private int w;
    private int h;
    private MP4Recorder videoRecorder;
    private Coroutine recordVideoCoroutine;
    private int frameSkip = 5;
    private float recordingDuration = 60.0f;
    private float captureFrameRate = 5.0f;
    private int gpuReadbackInterval = 2;
    private int frameCount;
    private float nextCaptureTime;

    private void Start()
    {
        w = Screen.width;
        h = Screen.height;
        StartRecording();
    }

    private void OnDestroy()
    {
        StopRecording();
    }

    private string recordingPath = "Assets/Recordings/myVideo.mp4";

    public void StartRecording()
    {
        videoRecorder = new MP4Recorder(w, h, 30, 60, 1);
        recordVideoCoroutine = StartCoroutine(Recording());
        StartCoroutine(StopRecordingAfterDuration());
    }

    public async void StopRecording()
    {
        StopCoroutine(recordVideoCoroutine);
        var recordingPath = await videoRecorder.FinishWriting();

        Debug.Log($"Recording saved to: {recordingPath}");

        #if !UNITY_STANDALONE
        Handheld.PlayFullScreenMovie($"file://{recordingPath}");
        #endif
    }


    private IEnumerator Recording()
    {
        var clock = new RealtimeClock();

        yield return new WaitForSeconds(1.0f);

        while (true)
        {
            float currentTime = Time.time;

            yield return new WaitForEndOfFrame();

            if (currentTime < nextCaptureTime)
                continue;

            if (Time.frameCount % frameSkip != 0)
                continue;

            frameCount++;

            if (frameCount % gpuReadbackInterval == 0)
            {
                RenderTexture renderTexture = new RenderTexture(w, h, 24);
                yield return new WaitForEndOfFrame();
                Graphics.Blit(null, renderTexture);
                AsyncGPUReadbackRequest request = AsyncGPUReadback.Request(renderTexture);
                yield return new WaitUntil(() => request.done);

                videoRecorder.CommitFrame(request.GetData<Color32>().ToArray(), clock.timestamp);
            }

            nextCaptureTime = currentTime + 1f / captureFrameRate;
        }
    }

    private IEnumerator StopRecordingAfterDuration()
    {
        yield return new WaitForSeconds(recordingDuration);
        StopRecording();
    }
}
