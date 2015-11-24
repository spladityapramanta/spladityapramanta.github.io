using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour {

	GameObject attachedPiece;

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
	public void Reset(){
		state = TileState.empty;
		if(attachedPiece!=null){
			Destroy( attachedPiece.gameObject);
		}
	}

	public void AttachPiece(GameObject piece){
		piece.transform.parent = GetComponentInChildren<MeshRenderer>().transform;
		piece.transform.localPosition = new Vector3(0,0.53f,0);
		attachedPiece = piece;
	}
}
