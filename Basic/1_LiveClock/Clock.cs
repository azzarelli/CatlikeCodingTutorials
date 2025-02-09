using System;
using UnityEngine;

public class Clock :  MonoBehaviour
{
    [SerializeField]
	Transform hoursPivot, minPivot, secPivot;

    // We need to divide the hour by  30degrees (12/360), min by 6 (60/360) and same for sec
    const float hoursToDegrees = -30f, minutesToDegrees = -6f, secondsToDegrees = -6f;


    void Update (){
        // Initialize loops time (now)
        TimeSpan time = DateTime.Now.TimeOfDay;

        // Update the rotation of each clock hand
        hoursPivot.localRotation = Quaternion.Euler(0f, 0f, (float)time.TotalHours * hoursToDegrees);
        minPivot.localRotation = Quaternion.Euler(0f, 0f, (float)time.TotalMinutes * minutesToDegrees);
        secPivot.localRotation = Quaternion.Euler(0f, 0f, (float)time.TotalSeconds * secondsToDegrees);

    }
    
}
 