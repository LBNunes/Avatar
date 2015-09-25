using UOS;
using UnityEngine;
using System.Collections.Generic;

public class AvatarFeedback : MonoBehaviour {

    #region Members

    static Dictionary<string, Member> memberTable = new Dictionary<string, Member>();

    FeedbackDriver driver;

    #endregion

    #region Unity Methods
    void Start () {

        driver = new FeedbackDriver();

        memberTable.Add("leftArm", new Member("leftArm", transform.Find("Skeleton/Hips/Spine/Spine1/Spine2/Neck/LeftShoulder/LeftArm"), Vector3.right, Vector3.down, Vector3.forward));
        memberTable.Add("leftForearm", new Member("leftForearm", transform.Find("Skeleton/Hips/Spine/Spine1/Spine2/Neck/LeftShoulder/LeftArm/LeftForeArm"), Vector3.right, Vector3.zero, Vector3.forward));
        memberTable.Add("leftHand", new Member("leftHand", transform.Find("Skeleton/Hips/Spine/Spine1/Spine2/Neck/LeftShoulder/LeftArm/LeftForeArm/LeftHand"), Vector3.forward, Vector3.left, Vector3.zero));
        memberTable.Add("rightArm", new Member("rightArm", transform.Find("Skeleton/Hips/Spine/Spine1/Spine2/Neck/RightShoulder/RightArm"), Vector3.left, Vector3.up, Vector3.forward));
        memberTable.Add("rightForearm", new Member("rightForearm", transform.Find("Skeleton/Hips/Spine/Spine1/Spine2/Neck/RightShoulder/RightArm/RightForeArm"), Vector3.left, Vector3.zero, Vector3.forward));
        memberTable.Add("rightHand", new Member("rightHand", transform.Find("Skeleton/Hips/Spine/Spine1/Spine2/Neck/RightShoulder/RightArm/RightForeArm/RightHand"), Vector3.back, Vector3.right, Vector3.zero)); 
	}
	
	void Update () {
        if (driver.memberRotations.Count > 0) {
            lock (driver.rotationLock) {
                foreach (var entry in driver.memberRotations) {
                    memberTable[entry.Key].SetRotation(entry.Value);
                }
            }
        }
	}
    #endregion
}
