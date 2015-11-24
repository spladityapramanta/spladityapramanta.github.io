using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour {
	public enum TileState{
		empty, p1, p2
	}
	public int idX, idY;
	public TileState state;

	public void SetTile(int _x, int _y){
		state = TileState.empty;
		idX = _x;
		idY = _y;
		gameObject.SetActive(true);
		transform.position = new Vector3(-7+_x,0,-7+_y);
	}
	public void OnMouseDown(){
		if (state==TileState.empty){
			GameManager.TileClicked(this);
		}
	}
}
