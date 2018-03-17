using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class geo : MonoBehaviour {

	public UnityEngine.UI.Button b;
	// Use this for initialization
	void Start () {
		UnityEngine.UI.Button btn = b.GetComponent<UnityEngine.UI.Button> ();
		//btn.onClick.AddListener();
	}

}
