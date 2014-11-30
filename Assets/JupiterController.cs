using UnityEngine;
using System.Collections;

public class JupiterController : MonoBehaviour {

    public float RotationSpeed;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 rot = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z);
        rot.y += RotationSpeed * Time.deltaTime;
        transform.eulerAngles = rot;
	}
}
