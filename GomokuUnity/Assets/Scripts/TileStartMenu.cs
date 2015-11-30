using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TileStartMenu : MonoBehaviour {

	public delegate void Callback();

	public List<KeyValuePair<Rect,Callback>> TileSMenu;
	public List<KeyValuePair<Rect,Callback>> TileGameOverMenu;

	// Use this for initialization
	void Awake () {
		TileSMenu = new List<KeyValuePair<Rect,Callback>> ();
		TileGameOverMenu = new List<KeyValuePair<Rect,Callback>> ();
	}

	public void MenuButtonInit () {
		TileSMenu.Clear ();
		TileGameOverMenu.Clear ();
//		TileSMenu.Add (new Rect (1.0f,12.0f,5.0f,1.0f));
//		TileSMenu.Add (new Rect (1.0f, 9.0f, 5.0f, 1.0f));
//		TileSMenu.Add (new Rect (1.0f, 6.0f, 5.0f, 1.0f));
//		TileSMenu.Add (new Rect (9.0f, 12.0f, 5.0f, 1.0f));
//		TileSMenu.Add (new Rect (9.0f, 9.0f, 5.0f, 1.0f));
//		TileSMenu.Add (new Rect (6.0f, 6.0f, 5.0f, 1.0f));
//		TileSMenu.Add (new Rect (6.0f, 3.0f, 5.0f, 1.0f));
//		TileGameOverMenu.Add (new Rect (1.0f, 4.0f, 7.0f, 1.0f));
//		TileGameOverMenu.Add (new Rect (10.0f, 4.0f, 4.0f, 1.0f));
	}
	

	public void StartButtonUpdate (float x, float y, float width, float height, Callback callback) {
		TileSMenu.Add(new KeyValuePair<Rect,Callback>(new Rect(x, y, width, height), callback));
	}

	public void GameOVerButtonUpdate (float x, float y, float width, float height, Callback callback) {
		TileGameOverMenu.Add(new KeyValuePair<Rect,Callback>(new Rect(x, y, width, height), callback));
	}
}
