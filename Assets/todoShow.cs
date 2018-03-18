using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Globalization;
using System;

public class todoShow : MonoBehaviour {

	public Text debugText; 
	public Text gpsText;
	public float lat;
	public float lon;
	public Button b;
	public textToSpeechVision tts;
	void Start () {
		UnityEngine.UI.Button btn = b.GetComponent<UnityEngine.UI.Button> ();
		btn.onClick.AddListener(check);
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
		lat = Input.location.lastData.latitude;
		lon = Input.location.lastData.longitude;
		gpsText.text = lat.ToString () + " " + lon.ToString (); 
		Input.location.Stop();

		//Display to-do here.
		string ans = "Your personal notes at this location\n" ;
		string toSpeak = "Here are some of your personal notes at this location\n";
		if (File.Exists (Application.persistentDataPath + "/newToDoList.dat")) {
			FileStream file = File.Open (Application.persistentDataPath + "/newToDoList.dat", FileMode.Open);
			BinaryFormatter bf = new BinaryFormatter ();
			toDoList res = (toDoList)bf.Deserialize (file);
			file.Close ();
			string[] lats = res.lat.Split ('~');
			string[] lons = res.lon.Split ('~');
			string[] vals = res.val.Split ('~');
			int i , j = 1 ;
			for (i = 0; i < res.number; i++) {
				if (j <= 3) {
					toSpeak = toSpeak + "Number " + j.ToString () + " " + vals [i] + "\n"; 
					j++;
				}
				float lat2 = float.Parse( lats[i] , CultureInfo.InvariantCulture.NumberFormat);
				float lon2 = float.Parse( lons[i] , CultureInfo.InvariantCulture.NumberFormat);
				float dis = Calc( lat , lon , lat2 , lon2 ) ;
				ans = ans + lats [i] + " " + lons [i] + " " + vals [i] + " " + dis.ToString() + "\n" ;
			}
		}
		debugText.text = ans;
		tts.convert (toSpeak);
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
