using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TileStartMenu : MonoBehaviour {

	public List<Rect> TileSMenu;
	public List<Rect> TileGameOverMenu;

	// Use this for initialization
	void Awake () {
		TileSMenu = new List<Rect> ();
		TileGameOverMenu = new List<Rect> ();
	}

	public void MenuButton () {
		TileSMenu.Clear ();
		TileSMenu.Add (new Rect (1.0f,12.0f,5.0f,1.0f));
		TileSMenu.Add (new Rect (1.0f, 9.0f, 5.0f, 1.0f));
		TileSMenu.Add (new Rect (1.0f, 6.0f, 5.0f, 1.0f));
		TileSMenu.Add (new Rect (9.0f, 12.0f, 5.0f, 1.0f));
		TileSMenu.Add (new Rect (9.0f, 9.0f, 5.0f, 1.0f));
		TileSMenu.Add (new Rect (6.0f, 6.0f, 5.0f, 1.0f));
		TileSMenu.Add (new Rect (6.0f, 3.0f, 5.0f, 1.0f));
		TileGameOverMenu.Add (new Rect (1.0f, 4.0f, 7.0f, 1.0f));
		TileGameOverMenu.Add (new Rect (10.0f, 4.0f, 4.0f, 1.0f));
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
