using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {
	public int force = 1;
	public Text text;
	public GameObject winText;
	private Rigidbody rd;
	private int score = 0;

	// Use this for initialization
	void Start () {
		rd = GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetTouch (0).phase == TouchPhase.Began) {
			
			GetComponent<AudioSource>().Play ();
		}

	}

	void FixedUpdate() {
		float h = Input.GetAxis ("Horizontal");
		float v = Input.GetAxis ("Vertical");

		rd.AddForce (new Vector3 (h, 0, v) * force);
	}

	void OnCollisionEnter(Collision collision) {
	}

	void OnTriggerEnter(Collider collider) {
		if (collider.tag.Equals ("food")) {
			Destroy (collider.gameObject);
			score++;
			text.text = "分数：" + score.ToString ();
			if (score == 3) {
				winText.SetActive (true);
			}
		}
	}
}
