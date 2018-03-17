using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class todo : MonoBehaviour {

	public Text gpsText;
	public float lat;
	public float lon;
	public UnityEngine.UI.Button b;
	public InputField todoInput;
	// Use this for initialization
	void Start () {
		UnityEngine.UI.Button btn = b.GetComponent<UnityEngine.UI.Button> ();
		btn.onClick.AddListener(check);
	}

	public void check()
	{
		StartCoroutine ("save");
	}

	IEnumerator save()
	{
		//Code to get location
		Input.location.Start ();
		int maxWait = 20;
		while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0) {
			yield return new WaitForSeconds (1);
			maxWait--;
		}
		lat = Input.location.lastData.latitude;
		lon = Input.location.lastData.longitude;
		gpsText.text = lat.ToString () + " " + lon.ToString (); 
		Input.location.Stop();

		//Code to add to list
		toDoList res ;
		FileStream file;
		BinaryFormatter bf;
		if (File.Exists (Application.persistentDataPath + "/newToDoList.dat")) {
			bf = new BinaryFormatter ();
			file = File.Open (Application.persistentDataPath + "/newToDoList.dat", FileMode.Open);
			res = (toDoList)bf.Deserialize (file);
			file.Close ();
		} 
		else {
			bf = new BinaryFormatter();
			file = File.Create (Application.persistentDataPath + "/newToDoList.dat" ); 
			res = new toDoList () ;
			res.number = 0;
			res.lat = "";
			res.lon = "";
			res.val = "";
			file.Close ();
		}
		bf = new BinaryFormatter();
		file = File.Create (Application.persistentDataPath + "/newToDoList.dat" ); 
		res.lat = res.lat + lat.ToString() + "~" ;
		res.lon = res.lon + lon.ToString() + "~" ;
		res.val = res.val + todoInput.text + "~" ;
		res.number = res.number + 1;
		bf.Serialize (file, res);
		file.Close ();
	}
}
