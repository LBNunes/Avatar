using UnityEngine;
using System.Collections.Generic;
using WyrmTale;

public class AvatarAnimation {
    bool eulerAngles;
    float waitTime;
    string cameraPosition;
    List<Dictionary<string, float[]>> animationStages;

    public AvatarAnimation(List<Dictionary<string, float[]>> animationStages, float waitTime, string cameraPosition, bool eulerAngles = false) {
        this.animationStages = animationStages;
        this.waitTime = waitTime;
        this.cameraPosition = cameraPosition;
        this.eulerAngles = eulerAngles;
    }

    public float GetWaitTime() { return waitTime; }
    public void SetWaitTime(float value) { waitTime = value; }
    public string GetCameraPosition() { return cameraPosition; }
    public void SetCameraPosition(string value) { cameraPosition = value; }
    public bool IsEulerAngles() { return eulerAngles; }
    public bool IsQuaternions() { return !eulerAngles; }
    public List<Dictionary<string, float[]>> GetAnimationStages() { return animationStages; } /* For debugging purposes only! */
    public Dictionary<string, float[]> GetAnimationStage(int stage) {
        if (stage >= animationStages.Count)
            return null;

        return animationStages[stage];
    }

    public static implicit operator JSON(AvatarAnimation a) {
        JSON js = new JSON();
        js["eulerAngles"] = a.eulerAngles;
        js["waitTime"] = a.waitTime;
        js["cameraPosition"] = a.cameraPosition;
        js["animationStages"] = a.animationStages.ToArray();
        return js;
    }

    public static explicit operator AvatarAnimation(JSON js) {
        checked {
            return new AvatarAnimation(
                (List<Dictionary<string, float[]>>)js["animationStages"],
                (float)js["waitTime"],
                (string)js["cameraPosition"],
                (bool)js["eulerAngles"]
            );
        }
    }

    public static AvatarAnimation[] Array(JSON[] array) {
        List<AvatarAnimation> tc = new List<AvatarAnimation>();
        for (int i = 0; i < array.Length; i++)
            tc.Add((AvatarAnimation)array[i]);
        return tc.ToArray();
    }
}