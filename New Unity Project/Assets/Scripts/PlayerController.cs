using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public enum State {
	PLAYING,
	WON,
	FALLING
}

public class GameState {
	public State state;
	public GameObject floor;
	public int count;
	public int totalCubes;

	public GameState() {
		state = State.PLAYING;
		floor = GameObject.Find ("Level1");
		count = 0;
		totalCubes = 0;
	}

	public void AddCubes(int cubesInLevel) {
		totalCubes += cubesInLevel;
	}

	public void ChangeState(State state) {
		this.state = state;
	}

	public void EatCube() {
		count += 1;
		if (count >= totalCubes) {
			ChangeState(State.WON);
		}
	}
}

public class PlayerController : MonoBehaviour {

	public float speed;
	public Text countText;
	public Text winText;
	public const int winAmount = 12;

	GameState currentState;

	void Start()
	{
		currentState = new GameState ();
		currentState.AddCubes (winAmount);
		SetCountText ();
		winText.text = "";
	}

	bool IsFalling() {
		return GetComponent<Rigidbody>().velocity.y < -0.1;
	}

	string GetNextLevel() {
		// TODO
		return "Level2";
	}

	void FixedUpdate ()
	{
		if (currentState == null) {
			winText.text = "Loading.";
			return;
		}

		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");

		Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);

		Rigidbody body = GetComponent<Rigidbody> ();
		body.AddForce (movement * speed);

		if (currentState.state == State.WON) {
			winText.text = "You Win... FOR NOW!";
			currentState.floor.GetComponent<Rigidbody>().isKinematic = false;
			currentState.floor.GetComponent<Rigidbody>().useGravity = true;
			currentState.ChangeState (State.FALLING);
		} else if (currentState.state == State.FALLING && !IsFalling()){
			currentState.ChangeState (State.PLAYING);
			currentState.floor = GameObject.Find (GetNextLevel());
			winText.text = "";
		}
	}
	void OnTriggerEnter(Collider other) 
	{
		if (other.gameObject.CompareTag ("Pickup"))
		{
			other.gameObject.SetActive (false);
			currentState.EatCube();
			SetCountText ();
		}
	}
	void SetCountText ()
	{
		countText.text = "Count: " + currentState.count.ToString ();
	}
}
