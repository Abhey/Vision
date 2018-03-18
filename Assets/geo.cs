using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using System;
using Firebase.Database;
using Firebase.Unity.Editor;
using UnityEngine.UI;

class RowEntry{
	public double lat;
	public double lon;
	public string data;

	public RowEntry(double _lat, double _lon, string _data){
		lat = _lat;
		lon = _lon;
		data = _data;
	}
	public Dictionary<string, System.Object> ToDictionary() {
		Dictionary<string, System.Object> result = new Dictionary<string, System.Object>();
		result["lat"] = lat;
		result["lon"] = lon;
		result ["data"] = data;
		return result;
	}
}

public class geo : MonoBehaviour {

	public UnityEngine.UI.Button b;
	public InputField I;
	public Text gpsText;
	DependencyStatus dependencyStatus = DependencyStatus.UnavailableOther;

	// Use this for initialization
	void Start () {
		UnityEngine.UI.Button btn = b.GetComponent<UnityEngine.UI.Button> ();
		btn.onClick.AddListener(check);

		FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
			dependencyStatus = task.Result;
			if (dependencyStatus == DependencyStatus.Available) {
				InitializeFirebase();
			} else {
				Debug.LogError("Could not resolve all Firebase dependencies: " + dependencyStatus);
			}
		});
	}

	protected virtual void InitializeFirebase() {
		FirebaseApp app = FirebaseApp.DefaultInstance;
		app.SetEditorDatabaseUrl("https://vision-avatar.firebaseio.com/");
		if (app.Options.DatabaseUrl != null) app.SetEditorDatabaseUrl(app.Options.DatabaseUrl);
	}

	public void AddRow(double _lat , double _lon, string _data){

    string key = FirebaseDatabase.DefaultInstance.GetReference("Rows").Push().Key;
    RowEntry e = new RowEntry(_lat, _lon, _data);
	Dictionary<string, System.Object> eVal = e.ToDictionary();
	Dictionary<string, System.Object> childUpdates = new Dictionary<string, System.Object>();
    childUpdates["/Rows/" + key] = eVal ;
    FirebaseDatabase.DefaultInstance.GetReference("Rows").UpdateChildrenAsync(childUpdates);

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
		float lat = Input.location.lastData.latitude;
		float lon = Input.location.lastData.longitude;
		gpsText.text = lat.ToString () + " " + lon.ToString (); 
		Input.location.Stop ();
		string val = I.text;
		AddRow (lat, lon, val);
	}
}
