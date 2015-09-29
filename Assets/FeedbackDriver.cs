using UnityEngine;
using System.Collections.Generic;
using UOS;

public class FeedbackDriver : UOSDriver {

    #region Members

    const string DRIVER_ID = "avatar.FeedbackDriver";

    UpDriver driver;

    public Dictionary<string, float[]> memberRotations = new Dictionary<string,float[]>();

    public bool eulerAngles;

    public Object rotationLock = new Object();

    #endregion

    #region Methods

    public FeedbackDriver() {
        driver = new UpDriver(DRIVER_ID);
        driver.AddService("RotateEuler").AddParameter("member", UpService.ParameterType.MANDATORY)
                                        .AddParameter("rotation", UpService.ParameterType.MANDATORY);
        driver.AddService("RotateQuaternion").AddParameter("member", UpService.ParameterType.MANDATORY)
                                             .AddParameter("rotation", UpService.ParameterType.MANDATORY);
    }

    #endregion

    #region Services
    public void RotateEuler(Call call, Response response, CallContext context) {
        string paramMember = call.GetParameterString("member");
        float[] paramRotation = (float[])call.GetParameter("rotation");

        lock (rotationLock) {
            memberRotations.Add(paramMember, paramRotation);
            eulerAngles = true;
        }
    }
    public void RotateQuaternion(Call call, Response response, CallContext context) {
        string paramMember = call.GetParameterString("member");
        float[] paramRotation = (float[])call.GetParameter("rotation");

        lock (rotationLock) {
            memberRotations.Add(paramMember, paramRotation);
            eulerAngles = false;
        }
    }
    #endregion

    #region UOS Methods
    public UpDriver GetDriver() {
        return driver;
    }

    public List<UpDriver> GetParent() {
        return null;
    }

    public void Init(IGateway gateway, uOSSettings settings, string instanceId) {
        Debug.Log("Initializing Avatar Feedback Driver...");
    }

    public void Destroy() {
    }
    #endregion
}
