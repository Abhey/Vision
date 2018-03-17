using UnityEngine;
using System.Collections;
using IBM.Watson.DeveloperCloud.Services.VisualRecognition.v3;
using IBM.Watson.DeveloperCloud.Logging;
using IBM.Watson.DeveloperCloud.Utilities;
using System.Collections.Generic;
using IBM.Watson.DeveloperCloud.Connection;
using System.Runtime.InteropServices;
using System.IO;

public class visualRecognition : MonoBehaviour
{
	byte[] imageByteArray;
	private string _apikey = "c5df3c97d2876236f153bc90b911298420ffbfbe";
	private string _url = "https://gateway-a.watsonplatform.net/visual-recognition/api";
	private VisualRecognition _visualRecognition;
	private string _visualRecognitionVersionDate = "2016-05-20";

	void Start()
	{
		LogSystem.InstallDefaultReactors();

		Credentials credentials = new Credentials(_apikey, _url);

		_visualRecognition = new VisualRecognition(credentials);
		_visualRecognition.VersionDate = _visualRecognitionVersionDate;

		StartCoroutine ("TakePhoto");
	}
		
	private void OnClassifyGet(ClassifyTopLevelMultiple classify, Dictionary<string, object> customData)
	{
		Log.Debug("ExampleVisualRecognition.OnClassifyGet()", "{0}", customData["json"].ToString());
	}
		
	private void OnFail(RESTConnector.Error error, Dictionary<string, object> customData)
	{
		Log.Error("ExampleRetrieveAndRank.OnFail()", "Error received: {0}", error.ToString());
	}

	public IEnumerator TakePhoto()
	{
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
		Log.Debug("ExampleVisualRecognition.Examples()", "Attempting to get classify by webcam.");
		if (!_visualRecognition.Classify(OnClassifyGet, OnFail , imageByteArray , "image/png"))
			Log.Debug("ExampleVisualRecognition", "Classify image failed!");

	}
}


