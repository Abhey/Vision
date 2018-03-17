using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySound : MonoBehaviour {

	public string url = "http://hd1.djring.com/320/479065/Let%20Me%20Love%20You%20-%20DJ%20Snake%20Ft%20Justin%20Bieber%20(DJJOhAL.Com).mp3";
	public AudioSource source = null ;

	// Use this for initialization
	IEnumerator Start () {
		source = GetComponent<AudioSource> ();
		using (var www = new WWW (url)) {
			yield return www;
			source.clip = www.GetAudioClip (true,true);
		}
	}
	
	// Update is called once per frame
	void Update () {
		if ( source != null && !source.isPlaying && source.clip.loadState == AudioDataLoadState.Loaded)
			source.Play ();
	}
}
