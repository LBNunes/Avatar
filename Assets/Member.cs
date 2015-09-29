using UnityEngine;
using System.Collections.Generic;

public class Member {

    const float MARGIN = 1.0f;
    Transform transform;
    Quaternion defaultRotation;
    Quaternion initialRotation;
    Quaternion goalRotation;
    Vector3 axisA;
    Vector3 axisB;
    Vector3 axisC;
    float time;
    float elapsed;
    string key;
    bool done;

    public Member(string k, Transform t, Vector3 axisA, Vector3 axisB, Vector3 axisC) {
        key = k;
        transform = t;
        this.axisA = axisA;
        this.axisB = axisB;
        this.axisC = axisC;

        if (transform == null) {
            Debug.Log("WARNING: " + key + " member is null");
        }

        elapsed = 0;
        time = 5;
        defaultRotation = transform.localRotation;
        initialRotation = defaultRotation;
        goalRotation = defaultRotation;

        Debug.Log("Create member " + key + " with rotation " + defaultRotation.ToString());
    }

    public void Update() {
        if (done) return;
        elapsed += Time.deltaTime;
        transform.localRotation = Quaternion.Lerp(initialRotation, goalRotation, elapsed / time);
        if (Mathf.Abs(Quaternion.Angle(transform.localRotation, goalRotation)) < MARGIN) {
            //Debug.Log("Member " + key + " finished rotation.");
            done = true;
        }
    }

    public void Reset() {
        transform.localRotation = defaultRotation;
        initialRotation = defaultRotation;
        //Debug.Log("Reset member " + key + " from " + transform.rotation.ToString() + " to default rotation.");
    }

    public void SavePosition() {
        initialRotation = transform.localRotation;
    }

    public void ResetStage() {
        transform.localRotation = initialRotation;
        //Debug.Log("Reset member " + key + " from " + transform.rotation.ToString() + " to animation stage start.");
    }

    public void SetRotationEuler(float[] rotation) {
        Reset();

        if (axisA != Vector3.zero) {
            transform.localRotation = transform.localRotation * Quaternion.AngleAxis(rotation[0], axisA);
        }
        else if (rotation[0] != 0.0f) {
            Debug.LogWarning("Attempting to rotate member " + key + " around impossible axis!");
        }
        if (axisB != Vector3.zero) {
            transform.localRotation = transform.localRotation * Quaternion.AngleAxis(rotation[1], axisB);
        }
        else if (rotation[1] != 0.0f) {
            Debug.LogWarning("Attempting to rotate member " + key + " around impossible axis!");
        }
        if (axisC != Vector3.zero) {
            transform.localRotation = transform.localRotation * Quaternion.AngleAxis(rotation[2], axisC);
        }
        else if (rotation[2] != 0.0f) {
            Debug.LogWarning("Attempting to rotate member " + key + " around impossible axis!");
        }
        
        SavePosition();
        goalRotation = transform.localRotation; // Prevent invalid goals
        //Debug.Log("Force rotation of member " + key + " by " + rotation.ToString());
    }

    public void SetRotationQuaternion(float[] rotation) {
        Reset();

        transform.localRotation = transform.localRotation * new Quaternion(rotation[0], rotation[1], rotation[2], rotation[3]);

        SavePosition();
        goalRotation = transform.localRotation; // Prevent invalid goals
        //Debug.Log("Force rotation of member " + key + " by " + rotation.ToString());
    }

    public bool IsDone() { return done; }

    public void SetNewGoalEuler(float[] rotation) {
        done = false;
        elapsed = 0;
        goalRotation = transform.localRotation;

        time = rotation[3];
        if (axisA != Vector3.zero) {
            goalRotation = goalRotation * Quaternion.AngleAxis(rotation[0], axisA);
        }
        else if (rotation[0] != 0.0f) {
            Debug.LogWarning("Attempting to rotate member " + key + " around impossible axis!");
        }
        if (axisB != Vector3.zero) {
            goalRotation = goalRotation * Quaternion.AngleAxis(rotation[1], axisB);
        }
        else if (rotation[1] != 0.0f) {
            Debug.LogWarning("Attempting to rotate member " + key + " around impossible axis!");
        }
        if (axisC != Vector3.zero) {
            goalRotation = goalRotation * Quaternion.AngleAxis(rotation[2], axisC);
        }
        else if (rotation[2] != 0.0f) {
            Debug.LogWarning("Attempting to rotate member " + key + " around impossible axis!");
        }
        SavePosition();
        //Debug.Log("Member " + key + " will rotate towards " + goal.ToString());

    }

    public void SetNewGoalQuaternion(float[] rotation) {
        done = false;
        elapsed = 0;
        goalRotation = transform.localRotation;

        time = rotation[4];
        goalRotation = goalRotation * new Quaternion(rotation[0], rotation[1], rotation[2], rotation[3]);

        SavePosition();
        //Debug.Log("Member " + key + " will rotate towards " + goal.ToString());
    }
}