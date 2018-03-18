using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class buttonController : MonoBehaviour {

	// Use this for initialization
	public GameObject b1;
	public GameObject b2;
	public GameObject b3;
	public GameObject b4;
	public GameObject b5;
	public GameObject b6;
	public GameObject b7;
	public Text t1;
	public Text t2;
	public Text t3;
	public Text t4;
	public Text t5;
	public GameObject I;
	void Start () {
		b3.SetActive (false);
		b4.SetActive (false);
		b5.SetActive (false);
		b7.SetActive (false);
		t1.text = "";
		t2.text = "";
		t3.text = "";
		t4.text = "";
		t5.text = "";
		I.SetActive (false);
	}

	public void f1()
	{
		b4.SetActive (true);
		b3.SetActive (true);
		I.SetActive (true);
		b1.SetActive (false);
		b2.SetActive (false);
		b6.SetActive (false);
		b7.SetActive (true);
	}
	public void f2()
	{
		b5.SetActive (true);
		I.SetActive (true);
		b1.SetActive (false);
		b2.SetActive (false);
		b6.SetActive (false);
		b7.SetActive (true);
	}
	public void f3()
	{
		b1.SetActive (false);
		b2.SetActive (false);
		b6.SetActive (false);
		b7.SetActive (true);
	}
	public void back()
	{
		b1.SetActive (true);
		b2.SetActive (true);
		b6.SetActive (true);
		b3.SetActive (false);
		b4.SetActive (false);
		b5.SetActive (false);
		t1.text = "";
		t2.text = "";
		t3.text = "";
		t4.text = "";
		t5.text = "";
		I.SetActive (false);		
	}
}
