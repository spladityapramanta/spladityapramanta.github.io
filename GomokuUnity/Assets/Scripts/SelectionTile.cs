using UnityEngine;
using System.Collections;

public class SelectionTile : MonoBehaviour {

	public static SelectionTile instance;
	Tile currentTile;
	public Material wallMaterial;
	public ParticleSystem[] wall;
	public ParticleSystem shockWave;
	bool isClickable = true;

	void Awake(){
		instance = this;
	}
	public static void SetClickable(bool isActive){
		if (instance!=null){
			instance.isClickable = isActive;
			for (int i =0;i<instance.wall.Length;i++){
				instance.wall[i].gameObject.SetActive(isActive);
			}
		}
	}
	public static void HoverOnTile(Tile tile){
		if (instance!=null){
			instance.currentTile = tile;
			instance.transform.parent = tile.GetComponentInChildren<MeshRenderer>().transform;
			instance.transform.localPosition = new Vector3(0,0.5f,0);
			if (tile.state != Tile.TileState.empty){
				instance.wallMaterial.SetColor("_TintColor",new Color(1,0.2f,0.2f,0.35f));
			} else {
				instance.wallMaterial.SetColor("_TintColor",new Color(1,1,0.4f,0.35f));
			}
		}
	}

	public void OnMouseDown(){
		if (currentTile!=null) 
		{
			if (isClickable){
				if (currentTile.state == Tile.TileState.empty){
					shockWave.Emit(1);
				} else {
					CameraEffect.Shake(0.5f,1);
				}
				currentTile.OnMouseDown();
			}
		}
	}
}
