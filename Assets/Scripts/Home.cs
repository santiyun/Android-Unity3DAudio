using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class Home : MonoBehaviour {
	public static HelloUnity app = null;
	public static string ChannelName = "0";

	private Text MessageText = null;
	private InputField ChannelInputField = null;
	private Button JoinButton = null;
	private Button LeaveButton = null;

	private bool IsPausingPoll = false;
	private bool IsJoiningChannel = false;

	void Start () {
		if (ReferenceEquals(app, null)) {
			app = new HelloUnity ();
			app.loadEngine ();

			app.RtcEngineAudio.OnError = onError;
			app.RtcEngineAudio.OnJoinChannelSuccess = onJoinChannelSuccess;
		}

		if (name.Equals ("JoinButton")) {
			MessageText = GameObject.Find ("messageText").GetComponent<Text> ();
			ChannelInputField = GameObject.Find ("channelName").GetComponent<InputField> ();
			JoinButton = GameObject.Find ("JoinButton").GetComponent<Button> ();

			MessageText.gameObject.SetActive (false);
			if (int.Parse (ChannelName) == 0) {
				ChannelName = Random.Range (1, 1000000).ToString ();
			}
			ChannelInputField.text = ChannelName;
			JoinButton.enabled = true;

			IsJoiningChannel = false;
		} else {
			LeaveButton = GameObject.Find ("LeaveButton").GetComponent<Button> (); 
			LeaveButton.enabled = true;
		}
	}

	void Update () {
		if (ReferenceEquals(app.RtcEngineAudio, null))
			return;

		while (!IsPausingPoll && app.RtcEngineAudio.GetMessageCount () > 0) {
			string callbackName = app.RtcEngineAudio.Poll ();
			Debug.LogFormat ("TTTDebug: GameObject is {0}, CallbackName is {1}", name, callbackName);

			if (callbackName.Equals("onJoinChannelSuccess")) {
				IsPausingPoll = true;
			}
		}
	}

	public void onJoinButtonClicked() {
		if (!IsJoiningChannel) {
			IsJoiningChannel = true;

			MessageText.text = "正在进入房间，请稍候 . . .";
			MessageText.color = Color.cyan;
			MessageText.gameObject.SetActive (true);
			//joinButton.enabled = false;

			app.joinChannel (ChannelInputField.text);
		}
	}

	public void onLeaveButtonClicked() {
		LeaveButton.GetComponent<Home> ().enabled = false;

		//if (app != null) {
		if (!ReferenceEquals(app, null)) {
			app.leaveChannel ();
			app.unloadEngine ();
			app = null;
			SceneManager.LoadScene ("HomeScene", LoadSceneMode.Single);
		}
	}

	public void onError (TTTRtcEngine.ERROR_CODE error, string msg) {
		if (name.Equals ("JoinButton")) {
			IsJoiningChannel = false;

			MessageText.color = Color.red;
			MessageText.text = msg;
		}
	}

	public void onJoinChannelSuccess (string channelName, uint uid, int elapsed) {
		JoinButton.GetComponent<Home> ().enabled = false;

		ChannelName = channelName;
		SceneManager.sceneLoaded += OnSceneLoaded;
		SceneManager.LoadScene ("HelloUnityScene", LoadSceneMode.Single);
	}

	public void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
		if (scene.name.Equals("HelloUnityScene")) {
			IsJoiningChannel = false;

			if (!ReferenceEquals (app, null)) {
				app.onHelloUnitySceneLoaded ();
			}

			SceneManager.sceneLoaded -= OnSceneLoaded;
		}
	}
}
