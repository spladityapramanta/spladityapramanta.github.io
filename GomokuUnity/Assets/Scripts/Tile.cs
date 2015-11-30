using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour {

	Piece attachedPiece;

	public enum TileState{
		empty, p1, p2
	}
	public int idX, idY;
	public TileState state;
	float brightness = 1;
	public bool clickable = true;

	public void SetTile(int _x, int _y){
		state = TileState.empty;
		idX = _x;
		idY = _y;
		gameObject.SetActive(true);
		transform.position = new Vector3(-7+_x,0,-7+_y);
		
		int color = 0;
		if (_x%2==0) color++;
		if (_y%2==0) color++;
		brightness = 1-0.3f*color;
		GetComponentInChildren<Renderer>().material.SetColor("_Color",new Color(brightness,brightness,brightness));
	}
	public void OnMouseDown(){
		if (state==TileState.empty && clickable){
			GameManager.TileClicked(this);
		}
	}

	public void OnMouseEnter(){
		SelectionTile.HoverOnTile(this);
	}

	public void Reset(){
		state = TileState.empty;
		if(attachedPiece!=null){
			attachedPiece.Fall(attachedPiece.transform.position.normalized * 4 + new Vector3(0,8,0));
		}
	}

	public void AttachPiece(Piece piece){
		piece.transform.parent = GetComponentInChildren<MeshRenderer>().transform;
		piece.transform.localPosition = new Vector3(0,0.5f,0);
		attachedPiece = piece;
	}

	public void DetachPiece(){
		if (attachedPiece!=null){
			attachedPiece.transform.parent = null;
			attachedPiece = null;
			state = TileState.empty;
		}
	}

	public void Woble(float power){
		GetComponent<Animator>().SetFloat("Power",power);
		GetComponent<Animator>().SetTrigger("Woble");
	}

	public void Blink(float power){
		GetComponentInChildren<Renderer>().material.SetColor("_Color",new Color(brightness+power,brightness+power,brightness+power));
		DG.Tweening.DOTween.To(
			()=>GetComponentInChildren<Renderer>().material.GetColor("_Color").r,
			(float x)=>(GetComponentInChildren<Renderer>().material.SetColor("_Color",new Color(x,x,brightness+(x-brightness)*0.8f,0.7f))),
			brightness, 0.9f);
	}
}
