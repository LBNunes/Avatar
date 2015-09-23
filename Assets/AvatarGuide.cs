using System.Collections.Generic;
using UnityEngine;
using UOS;
using WyrmTale;

public class AvatarGuide : MonoBehaviour, UOSDriver {

    #region Constants

    const string DRIVER_ID = "avatar.GuideDriver";

    #endregion

    #region Members

    static Dictionary<string, Member> memberTable = new Dictionary<string,Member>();

    UpDriver driver;

    AvatarAnimation avatarAnimation = null;
    State state = State.IDLE;
    float waitTime = 2.5f;
    float timer = 0.0f;
    int animationStage = 0;

    #endregion

    #region Methods
    private void SetCamera(string cameraPosition) {
        Vector3 yAxis = new Vector3(0, 1, 0);
        switch (cameraPosition) {
            case "leftArmSide":
                Camera.main.transform.position = new Vector3(-0.7f, 1.0f, 0.0f);
                Camera.main.transform.rotation = Quaternion.AngleAxis(90.0f, yAxis) * Quaternion.identity;
                break;
            case "rightArmSide":
                Camera.main.transform.position = new Vector3(0.7f, 1.0f, 0.0f);
                Camera.main.transform.rotation = Quaternion.AngleAxis(270.0f, yAxis) * Quaternion.identity;
                break;
            case "chest": 
            case "default":
            default:
                Camera.main.transform.position = new Vector3(0.0f, 1.1f, 0.7f);
                Camera.main.transform.rotation = Quaternion.AngleAxis(180.0f, yAxis) * Quaternion.identity;
                break;
        }
    }
    private void RestartAnimation(int initialStage = 0) {
        animationStage = initialStage + 1;
        Dictionary<string, float[]> initial = avatarAnimation.GetAnimationStage(initialStage);
        Dictionary<string, float[]> final = avatarAnimation.GetAnimationStage(animationStage);

        if (initial == null) {
            Debug.LogError("RestartAnimation() failure: No initial state set");
            return;
        }
        if (final == null) {
            Debug.LogError("RestartAnimation() failure: No final state set");
            return;
        }

        foreach (var entry in memberTable) {
            entry.Value.Reset();
        }

        foreach (var entry in initial) {
            float[] rotation = entry.Value;
            memberTable[entry.Key].SetRotation(rotation);
        }
        
        foreach (var entry in final) {
            float[] rotation = entry.Value;
            memberTable[entry.Key].SetNewGoal(rotation);
        }
    }

    private void MockMovement() {
        var list = new List<Dictionary<string, float[]>>();
        var initial = new Dictionary<string, float[]>();
        var final = new Dictionary<string, float[]>();
        var final2 = new Dictionary<string, float[]>();
        var final3 = new Dictionary<string, float[]>();
        initial.Add("leftArm",      new float[] { 0, 80, -105, 2 });
        initial.Add("leftForearm",  new float[] { 75, 0, 0, 2 });
        final.Add("leftHand",       new float[] { 0, 20, 0, 1 });
        final2.Add("leftHand",      new float[] { 0, -40, 0, 2 });
        final3.Add("leftHand",      new float[] { 0, 20, 0, 1 });
        initial.Add("rightArm",     new float[] { 0, 80, -105, 2 });
        initial.Add("rightForearm", new float[] { 75, 0, 0, 2 });
        final.Add("rightHand",      new float[] { 0, 20, 0, 1 });
        final2.Add("rightHand",     new float[] { 0, -40, 0, 2 });
        final3.Add("rightHand",     new float[] { 0, 20, 0, 1 });
        list.Add(initial);
        list.Add(final);
        list.Add(final2);
        list.Add(final3);
        avatarAnimation = new AvatarAnimation(list, 3.0f, "leftArmSide");

        SetCamera("chest");

        JSON json = new JSON();
        json["avatarAnimation"] = (JSON)avatarAnimation;

        Debug.Log(json.serialized);

        RestartAnimation();

        state = State.ANIMATING;
    }
    #endregion

    #region Unity Methods
    void Awake() {
    }

	void Start (){ 
        memberTable.Add("leftArm",      new Member("leftArm", transform.Find("Skeleton/Hips/Spine/Spine1/Spine2/Neck/LeftShoulder/LeftArm"), Vector3.right, Vector3.down, Vector3.forward));
        memberTable.Add("leftForearm",  new Member("leftForearm", transform.Find("Skeleton/Hips/Spine/Spine1/Spine2/Neck/LeftShoulder/LeftArm/LeftForeArm"), Vector3.right, Vector3.zero, Vector3.forward));
        memberTable.Add("leftHand",     new Member("leftHand", transform.Find("Skeleton/Hips/Spine/Spine1/Spine2/Neck/LeftShoulder/LeftArm/LeftForeArm/LeftHand"), Vector3.forward, Vector3.left, Vector3.zero));
        memberTable.Add("rightArm",     new Member("rightArm", transform.Find("Skeleton/Hips/Spine/Spine1/Spine2/Neck/RightShoulder/RightArm"), Vector3.left, Vector3.up, Vector3.forward));
        memberTable.Add("rightForearm", new Member("rightForearm", transform.Find("Skeleton/Hips/Spine/Spine1/Spine2/Neck/RightShoulder/RightArm/RightForeArm"), Vector3.left, Vector3.zero, Vector3.forward));
        memberTable.Add("rightHand",    new Member("rightHand", transform.Find("Skeleton/Hips/Spine/Spine1/Spine2/Neck/RightShoulder/RightArm/RightForeArm/RightHand"), Vector3.back, Vector3.right, Vector3.zero)); 

        MockMovement();
	}

    void Update() {
        if (state == State.ANIMATING) {
            bool animationOver = true;
            foreach (KeyValuePair<string, Member> member in memberTable) {
                member.Value.Update();
                animationOver = animationOver && member.Value.IsDone();
            }
            if (animationOver) {
                ++animationStage;
                var stage = avatarAnimation.GetAnimationStage(animationStage);
                if (stage == null) {
                    timer = waitTime;
                    state = State.WAITING;
                }
                else {
                    foreach (var entry in stage) {
                        float[] rotation = entry.Value;
                        memberTable[entry.Key].SetNewGoal(rotation);
                    }
                }
            }
        }
        else if (state == State.WAITING) {
            timer -= Time.deltaTime;
            if (timer <= 0) {
                RestartAnimation();
                state = State.ANIMATING;
            }
        }
    }
    #endregion

    #region Services
    public void Animate(Call call, Response response, CallContext context) {
        JSON js = new JSON();
        js.serialized = call.GetParameterString("animation");
        avatarAnimation = (AvatarAnimation)js.ToJSON("avatarAnimation");
        
        SetCamera(avatarAnimation.GetCameraPosition());
        RestartAnimation();
        state = State.ANIMATING;
    }
    public void Pause(Call call, Response response, CallContext context) {
        state = State.PAUSED;
    }
    public void Resume(Call call, Response response, CallContext context) {
        if (avatarAnimation == null) {
            Debug.LogError("Attempting to resume a null animation!");
            return;
        }
        if (timer > 0)
            state = State.WAITING;
        else
            state = State.ANIMATING;
    }
    public void Reset(Call call, Response response, CallContext context) {
        foreach (KeyValuePair<string, Member> entry in memberTable) {
            entry.Value.Reset();
        }
    }
    #endregion

    #region Auxiliary Classes
    private enum State { IDLE, PAUSED, ANIMATING, WAITING }

    private class AvatarAnimation {
        float waitTime;
        string cameraPosition;
        List<Dictionary<string, float[]>> animationStages;

        public AvatarAnimation(List<Dictionary<string, float[]>> animationStages, float waitTime, string cameraPosition) {
            this.animationStages = animationStages;
            this.waitTime = waitTime;
            this.cameraPosition = cameraPosition;
        }

        public float GetWaitTime() { return waitTime; }
        public void SetWaitTime(float value) { waitTime = value; }
        public string GetCameraPosition() { return cameraPosition; }
        public void SetCameraPosition(string value) { cameraPosition = value; }
        public List<Dictionary<string, float[]>> GetAnimationStages() { return animationStages; } /* For debugging purposes only! */
        public Dictionary<string, float[]> GetAnimationStage(int stage) {
            if (stage >= animationStages.Count) 
                return null;

            return animationStages[stage]; 
        }

        public static implicit operator JSON(AvatarAnimation a) {
            JSON js = new JSON();
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
                    (string)js["cameraPosition"]
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
        driver = new UpDriver(DRIVER_ID);
        driver.AddService("Animate").AddParameter("animation", UpService.ParameterType.MANDATORY);
        driver.AddService("Pause");
        driver.AddService("Resume");
        driver.AddService("Reset");
    }

    public void Destroy() {
    }
    #endregion

}
