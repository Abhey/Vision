using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IBM.Watson.DeveloperCloud.Logging;
using System.Net;
using System.IO;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.UI;
using System;
using System.Globalization;
using IBM.Watson.DeveloperCloud.Services.VisualRecognition.v3;
using IBM.Watson.DeveloperCloud.Utilities;
using SimpleJSON;
using IBM.Watson.DeveloperCloud.Connection;
using IBM.Watson.DeveloperCloud.Services.LanguageTranslator.v2;

public class botController : MonoBehaviour {

	public Text listObj;
	public Text debugText; 
	public Text gpsText;
	public float lat;
	public float lon;
	public string s1;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
		
	void getNews( ) {
		string url = "https://newsapi.org/v2/top-headlines?country=us&apiKey=142ba11e03d74ba38f859c785eee017f";
		WWW request = new WWW (url);
		StartCoroutine (OnResponse (request));
	}

	private IEnumerator OnResponse( WWW req ) {
		yield return req;
		Debug.Log (req.text);
	}
	/*public void save( string s ) {
		//Add the string to the to-do list
		toDoList res ;
		FileStream file;
		BinaryFormatter bf;
		if (File.Exists (Application.persistentDataPath + "/toDoList.dat")) {
			bf = new BinaryFormatter ();
			file = File.Open (Application.persistentDataPath + "/toDoList.dat", FileMode.Open);
			res = (toDoList)bf.Deserialize (file);
			file.Close ();
		} 
		else {
			bf = new BinaryFormatter();
			file = File.Create (Application.persistentDataPath + "/toDoList.dat" ); 
			res = new toDoList () ;
			res.val = "To-Do List\n";
			res.number = 0;
			bf.Serialize (file, res);
			file.Close ();
		}
		bf = new BinaryFormatter();
		file = File.Create (Application.persistentDataPath + "/toDoList.dat" ); 
		int number = res.number + 1 ;
		res.val = res.val + number.ToString () + ". " + s + "\n" ;
		res.number = number;
		listObj.text = res.val;
		bf.Serialize (file, res);
		file.Close ();
	}*/

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
		res.val = res.val + s1 + "~" ;
		res.number = res.number + 1;
		bf.Serialize (file, res);
		file.Close ();
	}
	IEnumerator display()
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
		string ans = "To-Do at this location\n" ;
		if (File.Exists (Application.persistentDataPath + "/newToDoList.dat")) {
			FileStream file = File.Open (Application.persistentDataPath + "/newToDoList.dat", FileMode.Open);
			BinaryFormatter bf = new BinaryFormatter ();
			toDoList res = (toDoList)bf.Deserialize (file);
			file.Close ();
			string[] lats = res.lat.Split ('~');
			string[] lons = res.lon.Split ('~');
			string[] vals = res.val.Split ('~');
			int i;
			for (i = 0; i < res.number; i++) {
				float lat2 = float.Parse( lats[i] , CultureInfo.InvariantCulture.NumberFormat);
				float lon2 = float.Parse( lons[i] , CultureInfo.InvariantCulture.NumberFormat);
				float dis = Calc( lat , lon , lat2 , lon2 ) ;
				ans = ans + lats [i] + " " + lons [i] + " " + vals [i] + " " + dis.ToString() + "\n" ;
			}

		}
		debugText.text = ans;
	}

	//Taken code for distance calculation
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
	//Language Translation code
	private string _username = "a325c2ef-f7b6-4afe-baf9-8ccfdcf8b6f5";
	private string _password = "4m2KEKPBrrDE";
	private string _url = "https://gateway.watsonplatform.net/language-translator/api";
	private LanguageTranslator _languageTranslator;
	public Text TranslatedText;
	public void translate( )
	{
		LogSystem.InstallDefaultReactors();
		Credentials credentials = new Credentials(_username, _password, _url);
		_languageTranslator = new LanguageTranslator(credentials);
		//_forcedGlossaryFilePath = Application.dataPath + "/Watson/Examples/ServiceExamples/TestData/glossary.tmx";
		StartCoroutine ("trans");
	}
	private void OnGetTranslation(Translations translation, Dictionary<string, object> customData)
	{
		Log.Debug("ExampleLanguageTranslator.OnGetTranslation()", "Langauge Translator - Translate Response: {0}", customData["json"].ToString());
		string res = customData ["json"].ToString() ;
		var N = JSON.Parse (res);
		string ans = N ["translations"] [0] ["translation"];
		TranslatedText.text = ans ;
	}

	private IEnumerator trans()
	{
		if (!_languageTranslator.GetTranslation(OnGetTranslation, OnFail, ObjectText.text  , "en", "es"))
			Log.Debug("ExampleLanguageTranslator.GetTranslation()", "Failed to translate.");
		yield return null;
	}
	//Language trans code end

	//Image recognition code
	byte[] imageByteArray;
	public Text ObjectText;
	public GameObject g1;
	public GameObject g2;
	private VisualRecognition _visualRecognition;
	public void check()
	{
		string _apikey = "c5df3c97d2876236f153bc90b911298420ffbfbe";
		string _url = "https://gateway-a.watsonplatform.net/visual-recognition/api";
		string _visualRecognitionVersionDate = "2016-05-20";

		LogSystem.InstallDefaultReactors();
		Credentials credentials = new Credentials(_apikey, _url);
		_visualRecognition = new VisualRecognition(credentials);
		_visualRecognition.VersionDate = _visualRecognitionVersionDate;
		StartCoroutine ("TakePhoto");
	}

	private void OnClassifyGet(ClassifyTopLevelMultiple classify, Dictionary<string, object> customData)
	{
		Log.Debug("ExampleVisualRecognition.OnClassifyGet()", "{0}", customData["json"].ToString());
		string res = customData ["json"].ToString() ;
		var N = JSON.Parse (res);
		var classes = N ["images"] [0] ["classifiers"] [0] ["classes"];
		string ans = classes [0] ["class"];
		ObjectText.text = ans ;
		translate ();
	}

	private void OnFail(RESTConnector.Error error, Dictionary<string, object> customData)
	{
		Log.Error("ExampleRetrieveAndRank.OnFail()", "Error received: {0}", error.ToString());
	}

	public IEnumerator TakePhoto()
	{
		g1.SetActive (false);
		g2.SetActive (false);
		string filePath;
		if (Application.isMobilePlatform) {

			filePath = Application.persistentDataPath + "/image.png";
			ScreenCapture.CaptureScreenshot ("/image.png");
			yield return new WaitForSeconds(1.5f);
			imageByteArray = File.ReadAllBytes(filePath);

		} else {

			filePath = Application.dataPath + "/StreamingAssets/" + "image.png";
			ScreenCapture.CaptureScreenshot (filePath);
			yield return new WaitForSeconds(1.5f);
			imageByteArray = File.ReadAllBytes(filePath);
		}
		g1.SetActive (true);
		g2.SetActive (true);

		Log.Debug("ExampleVisualRecognition.Examples()", "Attempting to get classify by webcam.");
		if (!_visualRecognition.Classify(OnClassifyGet, OnFail , imageByteArray , "image/png"))
			Log.Debug("ExampleVisualRecognition", "Classify image failed!");

	}
	//Image recog code end

	public void controller( string s ) {
		//File.Delete (Application.persistentDataPath + "/newToDoList.dat");
		//Ye ExampleStreaming se string leta hai aur uske upar kaam karta hai
		if ( s.Contains ("list")) {
			StartCoroutine ("display");
		} 
		else if (s.Contains ("add")) {
			s1 = s;
			StartCoroutine ("save");
			debugText.text = "Added to to-do list" ; 
		}
		else if ( s.Contains("news") ) {
			getNews( ) ;
		} 
		else if( s.Contains("check") )
		{
			check ();
		}
		else {
			Debug.Log (s);
		}
	}
}

[Serializable]
class toDoList
{
	public int number;
	public string lat;
	public string lon;
	public string val;
}