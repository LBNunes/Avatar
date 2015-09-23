using UOS;
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(uOS))]
public class uOSApp : MonoBehaviour, UOSApplication 
{
    public UnityGateway gateway { get; private set; }

	// Use this for initialization
	void Start () {
        uOS.Init(this, null);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Init(IGateway gateway, uOSSettings settings) {
        this.gateway = (UnityGateway) gateway;
    }

    public void TearDown() {
        this.gateway = null;
    }
}
