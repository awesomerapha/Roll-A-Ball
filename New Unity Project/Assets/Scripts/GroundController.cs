using UnityEngine;
using System.Collections;

public class GroundController : MonoBehaviour {
	public static GroundController floor1;

	// Use this for initialization
	void Start () {
		GroundController.floor1 = this;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
