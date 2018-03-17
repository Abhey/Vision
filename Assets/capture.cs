using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using IBM.Watson.DeveloperCloud.Services.VisualRecognition.v3;
using IBM.Watson.DeveloperCloud.Logging;
using IBM.Watson.DeveloperCloud.Utilities;
using SimpleJSON;
using IBM.Watson.DeveloperCloud.Services.LanguageTranslator.v2;
using UnityEngine.UI;
using IBM.Watson.DeveloperCloud.Connection;
using System.IO;

public class capture : MonoBehaviour {
	public UnityEngine.UI.Button b;
	// Use this for initialization
	void Start () {
		UnityEngine.UI.Button btn = b.GetComponent<UnityEngine.UI.Button> ();
		btn.onClick.AddListener(check);
	}
	
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

}
