using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using System;
using SimpleJSON;
using UnityEngine.UI;

public class geoShow : MonoBehaviour {

	public UnityEngine.UI.Button b;
	public Text ans;
	float lat1, lon1;
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

	public void check()
	{
		StartCoroutine ("list");
	}

	IEnumerator list()
	{
		//Code to get location
		Input.location.Start ();
		int maxWait = 20;
		while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0) {
			yield return new WaitForSeconds (1);
			maxWait--;
		}
		lat1 = Input.location.lastData.latitude;
		lon1 = Input.location.lastData.longitude;
		gpsText.text = lat1.ToString () + " " + lon1.ToString (); 
		Input.location.Stop();
		//Get location code end

		FirebaseDatabase.DefaultInstance
			.GetReference("Rows")
			.GetValueAsync().ContinueWith(task => {
				if (task.IsFaulted) {
					Debug.Log("Error in fetching database");
				}
				else if (task.IsCompleted) {
					DataSnapshot snapshot = task.Result;
					//Debug.Log( snapshot.GetRawJsonValue().ToString() ) ; 
					string res = snapshot.GetRawJsonValue().ToString() ;
					var N = JSON.Parse (res);
					var rows = N ["Rows"].Children ;
					string finalAns = "Global Reviews.\n" ;
					foreach( var row in rows )
					{
						//Debug.Log( row ) ;
						float lat2 = row["lat"] , lon2 = row["lon"] ;
						double dis = Calc( lat1 , lon1 , lat2 , lon2 ) ;
						finalAns = finalAns + row["lat"] + " " + row["lon"] + " " + row["data"] + " " + dis.ToString() + "\n" ;
					}
					ans.text = finalAns ;
					//Debug.Log(snapshot);
				}
			});
	}

	public float Calc(float lat1, float lon1, float lat2, float lon2)
	{

		var R = 6378.137; // Radius of earth in KM
		var dLat = lat2 * Mathf.PI / 180 - lat1 * Mathf.PI / 180;
		var dLon = lon2 * Mathf.PI / 180 - lon1 * Mathf.PI / 180;
		float a = Mathf.Sin(dLat / 2) * Mathf.Sin(dLat / 2) +
			Mathf.Cos(lat1 * Mathf.PI / 180) * Mathf.Cos(lat2 * Mathf.PI / 180) *
			Mathf.Sin(dLon / 2) * Mathf.Sin(dLon / 2);
		var c = 2 * Mathf.Atan2(Mathf.Sqrt(a), Mathf.Sqrt(1 - a));
		Double distance = R * c;
		distance = distance * 1000f; // meters
		float distanceFloat = (float)distance;
		return distanceFloat;
	}

}
