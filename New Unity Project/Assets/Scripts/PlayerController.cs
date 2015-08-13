using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public enum State {
	PLAYING,
	WON,
	FALLING
}
public class Level
{
	public GameObject floor;
	public int Numcubes;
	public Level(string name , int numbcubes)
	{
		Numcubes = numbcubes;
		floor = GameObject.Find (name);
	}
}
public class GameState {
	public State state;
	public Level[] Levels;
	public int count , score , CurrentLevel;

	public GameState() {
		state = State.PLAYING;
		Levels = new Level[]{
			new Level ("Levels/Level 1/Floor", 12),
			new Level ("Levels/Level 2/Floor", 10),
			new Level ("Levels/Level 3/Floor", 18)
		};
		count = 0;
		score = 0;
		CurrentLevel = 0;
	}

    
	public void ChangeState(State state) {
		this.state = state;
	}
	public Level GetLevel(){
		return Levels [CurrentLevel];
	}
	public void EatCube() {

		score += 1;
		count += 1;
		if (count >= GetLevel().Numcubes) {
			ChangeState(State.WON);
		
		

		}
	}
	public void NextLevel(){
		count = 0;
		if (CurrentLevel + 1 < Levels.Length) {
			CurrentLevel += 1;
		}
	}
}

public class PlayerController : MonoBehaviour {
	
	public Text countText;
	public Text winText;

	GameState currentState;

	void Start()
	{
		currentState = new GameState ();
		SetCountText ();
	}

	bool IsFalling() {
		return GetComponent<Rigidbody>().velocity.y < -0.1;
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
		for (var i = 0; i < Input.touchCount; ++i) {
			float dx = Input.GetTouch(i).deltaPosition.x;
			float dz = Input.GetTouch (i).deltaPosition.y;
			movement.x += dx;
			movement.z += dz;
		}
		movement.Normalize ();
		movement *= 10;

		Rigidbody body = GetComponent<Rigidbody> ();
		body.AddForce (movement);

		if (currentState.state == State.WON) {
			winText.text = "You Win...FOR NOW!";
			currentState.GetLevel().floor.GetComponent<Rigidbody>().isKinematic = false;
			currentState.GetLevel().floor.GetComponent<Rigidbody>().useGravity = true;
			currentState.ChangeState (State.FALLING);
		} else if (currentState.state == State.FALLING && !IsFalling()){
			currentState.ChangeState (State.PLAYING);
			currentState.NextLevel ();
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
		countText.text = "Score: " + currentState.score.ToString ();
	}
}
