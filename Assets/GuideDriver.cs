using UnityEngine;
using System.Collections.Generic;
using UOS;
using WyrmTale;

using GuideCall = System.Collections.Generic.KeyValuePair<AvatarGuide.State, AvatarAnimation>; 

public class GuideDriver : UOSDriver {

    #region Members
    const string DRIVER_ID = "avatar.GuideDriver";

    UpDriver driver;

    Queue<GuideCall> callQueue = new Queue<GuideCall>();

    Object queueLock = new Object();
    #endregion

    #region Methods

    public GuideDriver() {
        driver = new UpDriver(DRIVER_ID);
        driver.AddService("Animate").AddParameter("animation", UpService.ParameterType.MANDATORY);
        driver.AddService("Pause");
        driver.AddService("Resume");
        driver.AddService("Reset");
    }

    public bool HasCall() {
        bool value = false;
        lock (queueLock) {
            value = callQueue.Count > 0;
        }
        return value;
    }
    public GuideCall Dequeue() {
        GuideCall call;
        lock (queueLock) {
            call = callQueue.Dequeue();
        }
        return call;
    }
    #endregion

    #region Services
    public void Animate(Call call, Response response, CallContext context) {
        JSON js = new JSON();
        js.serialized = call.GetParameterString("animation");
        lock (queueLock) {
            callQueue.Enqueue(new GuideCall(AvatarGuide.State.ANIMATING, (AvatarAnimation)js.ToJSON("avatarAnimation")));
        }
    }
    public void Pause(Call call, Response response, CallContext context) {
        callQueue.Enqueue(new GuideCall(AvatarGuide.State.PAUSED, null));
    }
    public void Resume(Call call, Response response, CallContext context) {
        callQueue.Enqueue(new GuideCall(AvatarGuide.State.ANIMATING, null));
    }
    public void Reset(Call call, Response response, CallContext context) {
        callQueue.Enqueue(new GuideCall(AvatarGuide.State.IDLE, null));
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
        Debug.Log("Initializing Avatar Guide Driver...");
    }

    public void Destroy() {
    }
    #endregion
}