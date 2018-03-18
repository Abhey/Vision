using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IBM.Watson.DeveloperCloud.Services.TextToSpeech.v1;
using IBM.Watson.DeveloperCloud.Logging;
using IBM.Watson.DeveloperCloud.Utilities;
using IBM.Watson.DeveloperCloud.Connection;

public class textToSpeechVision : MonoBehaviour {

	private string _username = "d9682a83-f37d-4ede-b412-70f471195249";
	private string _password = "DhJmEEEjIBaP";
	private string _url = "https://stream.watsonplatform.net/text-to-speech/api";
	TextToSpeech _textToSpeech;

	void Start () {
		LogSystem.InstallDefaultReactors();
		Credentials credentials = new Credentials(_username, _password, _url);
		_textToSpeech = new TextToSpeech(credentials);
		convert ("Hi, this is Vision. Welcome to the world of Augmented Reality.\n");
	}
	
	public void convert( string s )
	{
		Log.Debug("ExampleTextToSpeech.Examples()", "Attempting synthesize.");
		Debug.Log ("Converting...");
		_textToSpeech.Voice = VoiceType.en_US_Allison;
		_textToSpeech.ToSpeech(HandleToSpeechCallback, OnFail, s, true);
	}

	void HandleToSpeechCallback(AudioClip clip, Dictionary<string, object> customData = null)
	{
		Debug.Log ("Audio received");
		PlayClip(clip);
	}

	private void PlayClip(AudioClip clip)
	{
		Debug.Log ("Playing Audio");
		if (Application.isPlaying && clip != null)
		{
			GameObject audioObject = new GameObject("AudioObject");
			AudioSource source = audioObject.AddComponent<AudioSource>();
			source.spatialBlend = 0.0f;
			source.loop = false;
			source.clip = clip;
			source.Play();
			Destroy(audioObject, clip.length);
		}
	}

	private void OnFail(RESTConnector.Error error, Dictionary<string, object> customData)
	{
		Log.Error("ExampleTextToSpeech.OnFail()", "Error received: {0}", error.ToString());
	}
}
