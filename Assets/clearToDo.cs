using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class clearToDo : MonoBehaviour {
	public Button b;
	void Start () {
		UnityEngine.UI.Button btn = b.GetComponent<UnityEngine.UI.Button> ();
		btn.onClick.AddListener(check);
	}

	void check(){
		File.Delete (Application.persistentDataPath + "/newToDoList.dat");
	}
}
