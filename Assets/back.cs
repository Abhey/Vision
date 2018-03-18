using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class back : MonoBehaviour {

	public GameObject g1;
	public GameObject g2;
	public UnityEngine.UI.Button b;
	// Use this for initialization
	void Start () {
		UnityEngine.UI.Button btn = b.GetComponent<UnityEngine.UI.Button> ();
		btn.onClick.AddListener(check);
	}
	void check()
	{
		g1.SetActive (false);
		g2.SetActive (true);
	}
}
