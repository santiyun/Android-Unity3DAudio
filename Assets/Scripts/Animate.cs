using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animate : MonoBehaviour {

	public int i = 0;
	public bool speak = false;
	public bool isOnLine = false;

	// Use this for initialization
	void Start () {
		Screen.sleepTimeout = SleepTimeout.NeverSleep;
	}
	
	// Update is called once per frame
	void Update () {
		Renderer theRenderer = GetComponent<Renderer> ();
		if (!isOnLine) {
			theRenderer.material.mainTexture = null;
		} else {
			if (!speak) {
				theRenderer.material.mainTexture = (Texture2D)Resources.Load ("textures/speak3");
				return;
			}

			i++;
			if (i == 1) {
				theRenderer.material.mainTexture = (Texture2D)Resources.Load ("textures/speak3");
			} else if (i == 2) {
				theRenderer.material.mainTexture = (Texture2D)Resources.Load ("textures/speak2");
			} else {
				i = 0;
				theRenderer.material.mainTexture = (Texture2D)Resources.Load ("textures/speak1");
			}
		}

	}
}

