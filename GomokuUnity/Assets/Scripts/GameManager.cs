using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
	static GameManager instance;

	public Tile tileLib;
	public GameObject pieceLib;
	public Material player1Mat, player2Mat;
	Tile[,] tiles;

	bool isPlayer1Turn = true;
	bool isAnimating = false;
	const float WOBLEDELAY = 0.05f; 

	void Awake () {
		instance = this;
	}
	void Start () {
		tiles = new Tile[15,15];
		for (int x=0;x<15;x++){
			for (int y=0;y<15;y++){
				tiles[x,y] = Instantiate<Tile>(tileLib) as Tile;
				tiles[x,y].SetTile(x,y);
			}
		}
	}

	public static void TileClicked(Tile tile){
		if (instance!=null && !instance.isAnimating){
			GameObject tempPiece = Instantiate(instance.pieceLib) as GameObject;
			tempPiece.SetActive(true);
			tile.AttachPiece(tempPiece);
			//tempPiece.transform.parent = tile.GetComponentInChildren<MeshRenderer>().transform;
			//tempPiece.transform.localPosition = new Vector3(0,0.53f,0);
			tempPiece.GetComponent<Piece>().SetMaterial(instance.isPlayer1Turn ? instance.player1Mat : instance.player2Mat);
			//tempPiece.GetComponentInChildren<MeshRenderer>().material = instance.isPlayer1Turn ? instance.player1Mat : instance.player2Mat;
			tile.state = instance.isPlayer1Turn ? Tile.TileState.p1 : Tile.TileState.p2;
			instance.StartCoroutine(instance.PutAnimation(tile));
			if (instance.CheckWinFromTile(tile)) Debug.Log((instance.isPlayer1Turn?"player 1":"player 2")+" win!");
			instance.isPlayer1Turn = !instance.isPlayer1Turn;
		}
	}

	bool CheckWinFromTile(Tile tile){
		Tile[] lastSameTile = new Tile[8];
		bool[] isEndCheck = new bool[8];
		for (int i=0;i<8;i++){
			isEndCheck[i] = false;
			lastSameTile[i]=tile;
		}
		for (int i=1;i<=5;i++){
			for (int d=0;d<8;d++){
				if (!isEndCheck[d]){
					int checkX = tile.idX;
					int checkY = tile.idY;
					switch(d){
					case 0://check UP
						checkX = tile.idX;
						checkY = tile.idY + i;
						break;
					case 1://check UP-RIGHT
						checkX = tile.idX + i;
						checkY = tile.idY + i;
						break;
					case 2://check RIGHT
						checkX = tile.idX + i;
						checkY = tile.idY;
						break;
					case 3://check DOWN-RIGHT
						checkX = tile.idX + i;
						checkY = tile.idY - i;
						break;
					case 4://check DOWN
						checkX = tile.idX;
						checkY = tile.idY - i;
						break;
					case 5://check DOWN-LEFT
						checkX = tile.idX - i;
						checkY = tile.idY - i;
						break;
					case 6://check LEFT
						checkX = tile.idX - i;
						checkY = tile.idY;
						break;
					case 7://check UP-LEFT
						checkX = tile.idX - i;
						checkY = tile.idY + i;
						break;

					}
					if (checkX>=0 && checkX<15 && checkY>=0 && checkY<15 && tiles[checkX,checkY].state == tile.state){
						lastSameTile[d] = tiles[checkX,checkY];
					} else {
						isEndCheck[d] = true;
					}
				}
			}
		}

		int maxRow = 0;
		for (int i=0;i<4;i++){
			maxRow = Mathf.Max(maxRow, Mathf.Abs(lastSameTile[i].idY-lastSameTile[i+4].idY)+1, Mathf.Abs(lastSameTile[i].idX-lastSameTile[i+4].idX)+1 );
		}
		return (maxRow >=5);
	}

	IEnumerator ChainGlowAnimation(Tile centerTile){
		float t = 0;
		centerTile.Blink(3);
		SFXManager.PlayOneShot(SFXManager.SFX.ping,0.3f,1);
		for (int distance=1;distance<7;distance++){
			while (t<WOBLEDELAY * distance){
				t+=Time.deltaTime;
				yield return 0;
			}
			SFXManager.PlayOneShot(SFXManager.SFX.ping,0.3f-0.02f*distance,1+0.15f*distance);
			for (int dir=0;dir<8;dir++){
				int checkX=centerTile.idX;
				int checkY=centerTile.idY;
				switch(dir){
				case 0:
					checkY=centerTile.idY+distance;
					break;
				case 1:
					checkX=centerTile.idX+distance;
					checkY=centerTile.idY+distance;
					break;
				case 2:
					checkX=centerTile.idX+distance;
					break;
				case 3:
					checkX=centerTile.idX+distance;
					checkY=centerTile.idY-distance;
					break;
				case 4:
					checkY=centerTile.idY-distance;
					break;
				case 5:
					checkX=centerTile.idX-distance;
					checkY=centerTile.idY-distance;
					break;
				case 6:
					checkX=centerTile.idX-distance;
					break;
				case 7:
					checkX=centerTile.idX-distance;
					checkY=centerTile.idY+distance;
					break;
				}
				if (checkX>=0 && checkX<15 && checkY>=0 && checkY<15){
					tiles[checkX,checkY].Blink(3-0.4f*distance);
				}
			}
		}
		yield return 0;
	}

	IEnumerator PutAnimation(Tile centerTile){
		
		isAnimating = true;
		float t=0;
		yield return new WaitForSeconds(0.28f-WOBLEDELAY);
		centerTile.Woble(1);
		SFXManager.PlayOneShot(SFXManager.SFX.vibrato);
		StartCoroutine(ChainGlowAnimation(centerTile));
		for (int distance=1;distance<5;distance++){
			while (t<WOBLEDELAY * distance){
				t+=Time.deltaTime;
				yield return 0;
			}
			for (int y=distance;y>=-distance;y--){
				int checkY = centerTile.idY+y;
				if (y==distance || y==-distance){
					int checkX = centerTile.idX;
					if (checkX>=0 && checkX<15 && checkY>=0 && checkY<15){
						tiles[checkX,checkY].Woble(1-0.2f*distance);
					}
				} else {
					int checkXRight = centerTile.idX+(distance-Mathf.Abs(y));
					int checkXLeft = centerTile.idX-(distance-Mathf.Abs(y));
					if (checkY>=0 && checkY<15){
						if (checkXRight>=0 && checkXRight<15){
							tiles[checkXRight,checkY].Woble(1-0.2f*distance);
						}
						if (checkXLeft>=0 && checkXLeft<15){
							tiles[checkXLeft,checkY].Woble(1-0.2f*distance);
						}
					}
				}
			}
		}
		yield return new WaitForSeconds(0.5f);
		isAnimating = false;
		yield return 0;
	}
}
