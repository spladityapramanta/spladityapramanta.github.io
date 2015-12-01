using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TileMenuButton {

	public delegate void Callback();

	public Rect rect;
	public Callback callback;
	public Tile pivot;

	public static Rect RECT_HUMAN = new Rect(1,12,5,1);
	public static Rect RECT_COMPUTER = new Rect(9,12,5,1);
	public static Rect RECT_RED = new Rect(1,9,5,1);
	public static Rect RECT_BLUE = new Rect(9,9,5,1);
	public static Rect RECT_IDIOT = new Rect(1,6,5,1);
	public static Rect RECT_STUPID = new Rect(9,6,5,1);
	public static Rect RECT_GO = new Rect(5,2,5,2);
	public static Rect RECT_RESTART = new Rect(5,4,5,1);
	public static Rect RECT_MAINMENU = new Rect(5,1,5,1);

	//public static Rect RECT_RESTART = new Rect(1,4,7,1);
	public static Rect QUIT = new Rect(10,4,7,1);

	public TileMenuButton(Rect _rect, Tile _pivot, Callback _callback){
		rect = _rect;
		callback = _callback;
		pivot = _pivot;
	}

	//public List<KeyValuePair<Rect,Callback>> TileMenus;
	//public List<KeyValuePair<Rect,Callback>> TileGameOverMenu;

//	public void MenuButtonInit () {
//		TileMenus = new List<KeyValuePair<Rect, Callback>>();
//		TileMenus.Clear ();
//		TileSMenu.Add (new Rect (1.0f,12.0f,5.0f,1.0f));
//		TileSMenu.Add (new Rect (1.0f, 9.0f, 5.0f, 1.0f));
//		TileSMenu.Add (new Rect (1.0f, 6.0f, 5.0f, 1.0f));
//		TileSMenu.Add (new Rect (9.0f, 12.0f, 5.0f, 1.0f));
//		TileSMenu.Add (new Rect (9.0f, 9.0f, 5.0f, 1.0f));
//		TileSMenu.Add (new Rect (6.0f, 6.0f, 5.0f, 1.0f));
//		TileSMenu.Add (new Rect (6.0f, 3.0f, 5.0f, 1.0f));
//		TileGameOverMenu.Add (new Rect (1.0f, 4.0f, 7.0f, 1.0f));
//		TileGameOverMenu.Add (new Rect (10.0f, 4.0f, 4.0f, 1.0f));
//	}
	

	//public void StartButtonUpdate (float x, float y, float width, float height, Callback callback) {
	//	TileSMenu.Add(new KeyValuePair<Rect,Callback>(new Rect(x, y, width, height), callback));
	//}
	//
	//public void GameOVerButtonUpdate (float x, float y, float width, float height, Callback callback) {
	//	TileGameOverMenu.Add(new KeyValuePair<Rect,Callback>(new Rect(x, y, width, height), callback));
	//}
}
