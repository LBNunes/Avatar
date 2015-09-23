using UOS;
using UnityEngine;
using System.Collections.Generic;

public class AvatarFeedback : MonoBehaviour, UOSDriver {

    #region Constants

    const string DRIVER_ID = "avatar.FeedbackDriver";

    #endregion

    #region Members

    static Dictionary<string, Member> memberTable = new Dictionary<string, Member>();

    UpDriver driver;

    #endregion

    #region Unity Methods
    void Start () {
        memberTable.Add("leftArm", new Member("leftArm", transform.Find("Skeleton/Hips/Spine/Spine1/Spine2/Neck/LeftShoulder/LeftArm"), Vector3.right, Vector3.down, Vector3.forward));
        memberTable.Add("leftForearm", new Member("leftForearm", transform.Find("Skeleton/Hips/Spine/Spine1/Spine2/Neck/LeftShoulder/LeftArm/LeftForeArm"), Vector3.right, Vector3.zero, Vector3.forward));
        memberTable.Add("leftHand", new Member("leftHand", transform.Find("Skeleton/Hips/Spine/Spine1/Spine2/Neck/LeftShoulder/LeftArm/LeftForeArm/LeftHand"), Vector3.forward, Vector3.left, Vector3.zero));
        memberTable.Add("rightArm", new Member("rightArm", transform.Find("Skeleton/Hips/Spine/Spine1/Spine2/Neck/RightShoulder/RightArm"), Vector3.left, Vector3.up, Vector3.forward));
        memberTable.Add("rightForearm", new Member("rightForearm", transform.Find("Skeleton/Hips/Spine/Spine1/Spine2/Neck/RightShoulder/RightArm/RightForeArm"), Vector3.left, Vector3.zero, Vector3.forward));
        memberTable.Add("rightHand", new Member("rightHand", transform.Find("Skeleton/Hips/Spine/Spine1/Spine2/Neck/RightShoulder/RightArm/RightForeArm/RightHand"), Vector3.back, Vector3.right, Vector3.zero)); 
	}
	
	void Update () {
	
	}
    #endregion

    #region Services
    public void Rotate(Call call, Response response, CallContext context) {
        string member = call.GetParameterString("member");
        float[] rotation = (float[])call.GetParameter("rotation");

        memberTable[member].SetRotation(rotation);
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
        driver = new UpDriver(DRIVER_ID);
        driver.AddService("Rotate").AddParameter("member", UpService.ParameterType.MANDATORY)
                                   .AddParameter("rotation", UpService.ParameterType.MANDATORY);
    }

    public void Destroy() {
    }
    #endregion
}
