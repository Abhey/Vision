using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animationController : MonoBehaviour {

	Animator anim ;
	void Start () {
		anim = this.GetComponent<Animator> ();
	}
	
	void Update () {
		float vertical = Input.GetAxis ("Vertical");

		if (vertical > 0)
			anim.SetInteger ("state", 1);
		else if(vertical < 0)
			anim.SetInteger ("state", 2);		
	}
}
