using UnityEngine;
using TMPro;

public class FrameRateCounter : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI display;


    // Average FPS
    [SerializeField, Range(0.1f, 2f)]
    float sampleDuration = 1f;

    int frames;
    float duration, bestDuration = float.MaxValue, worstDuration;

    public enum DisplayMode {FPS, MS}

    [SerializeField]
    DisplayMode displayMode = DisplayMode.FPS;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Calculate current time in frames
        float frameDuration = Time.unscaledDeltaTime;
        frames += 1; // Update frame counter
        duration += frameDuration; // Update total duration 

        

        // Graphing best and worst frame durations
        if(frameDuration < bestDuration){
            bestDuration = frameDuration;
        }

        if(frameDuration > worstDuration){
            worstDuration = frameDuration;
        }

        // only update the counter when we collectd enough from the last window
        if(duration >= sampleDuration){// If the duration goes over the sample duration re-set tics
            if(displayMode == DisplayMode.FPS){
                display.SetText("FPS\n{0:0}\n{1:0}\n{2:0}", 1f/bestDuration, frames/duration, 1f/worstDuration);
                
            }else {
                display.SetText("FPS\n{0:1}\n{1:1}\n{2:1}", 1000f * bestDuration, 1000f *  duration/frames, 1000f * worstDuration);

            }
            frames = 0;
            duration = 0f;
            bestDuration = float.MaxValue;
            worstDuration = 0f;
            
            
        }
        
    }
}
