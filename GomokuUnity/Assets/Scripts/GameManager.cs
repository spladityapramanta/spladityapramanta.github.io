using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	delegate void Callback();

	static GameManager instance;

	public Tile tileLib;
	public Piece pieceLib;
	public GameObject startMenu;
	public GameObject gameoverMenu;
	public GameObject undoMenu;
	public Material player1Mat, player2Mat;
	Tile[,] tiles;
	public enum gameState{
		start, ingame, gameover
	}

	public gameState State;
	bool isPlayer1Turn = true;
	bool isAnimating = false;
	[HideInInspector] public bool isVSHuman = false;
	int AILevel = 1;
	int checkNumberOfStraightRow = 0;
	int playerLastPosX = 0;
	int playerLastPosY = 0;
	const float WOBLEDELAY = 0.05f;

	private List<KeyValuePair<int,int>> lastClicked;
	private int lastClickedNum;


	void Awake () {
		instance = this;
		State = gameState.start;
		gameoverMenu.SetActive (false);
	}
	void Start () {
		lastClicked = new List<KeyValuePair<int,int>> ();
		CameraEffect.MenuPerpective();
		tiles = new Tile[15,15];
		for (int x=0;x<15;x++){
			for (int y=0;y<15;y++){
				tiles[x,y] = Instantiate<Tile>(tileLib) as Tile;
				tiles[x,y].SetTile(x,y);
				if((x==1 && y==12)||(x==1 && y==9)||(x==1 && y==6)||(x==6 && y==12)||(x==6 && y==9)||(x==6 && y==6)||(x==6 && y==3))
				{
					tiles[x,y].clickable = true;
				}
				else
				{
					tiles[x,y].clickable = false;
				}
			}
		}

	}
	void Update(){
		//Debug.Log (isPlayer1Turn);
	 	if (!isPlayer1Turn && !isAnimating && State == gameState.ingame) {
			if (!isVSHuman) {
				int posX = 0;
				int posY = 0;
				switch(AILevel){
				case 1: //idiot
					while(tiles[posX,posY].state != Tile.TileState.empty){
						posX = (int)(Random.value * 15);
						posY = (int)(Random.value * 15);
					}
					break;
				//case 2: //very stupid
//					if(checkNumberOfStraightRow ==4)
//					{
//						posX = 
//					}
				}

				TileClicked(tiles[posX,posY]);
			}
		}
		if (lastClickedNum > 0)
			undoMenu.SetActive (true);
		else
			undoMenu.SetActive (false);

		//Debug.Log (lastClickedNum);
	}

	bool AIStupidWarned(){
		int tempVal = 1;
		Tile[] tempTiles = new Tile[4];
		foreach (Tile tile in tiles) {
			if(tile.state == Tile.TileState.p1){
				tempVal +=1;
				tempTiles[tempVal - 2] = tile;
				if(tempVal >= 3)
				{
					playerLastPosX = tile.idX;
					playerLastPosY = tile.idY;
					return true;
				}
			}
			else
			{
				tempVal -= 1;
				//tempTiles = [];

			}
		}
		return false;
	}
	
// ================================================================================= //
	// SHELL STARTING MENU METHODS 
	public void GOClicked(){
		Debug.Log ("Gas");
		CameraEffect.GameplayPerspective();
		State = gameState.ingame;
		startMenu.SetActive (false);
		isPlayer1Turn = true;
		for (int x=0;x<15;x++){
			for (int y=0;y<15;y++){
				tiles[x,y].Reset();
				tiles[x,y].clickable = true;
			}
		}

	}

	public void humanPlay(){
		Debug.Log ("Human");
		tiles [6, 12].Reset ();
		startMenu.transform.GetChild (2).gameObject.SetActive (false); //red
		startMenu.transform.GetChild (3).gameObject.SetActive (false); //blue
		startMenu.transform.GetChild (4).gameObject.SetActive (false); //idiot
		startMenu.transform.GetChild (5).gameObject.SetActive (false); //stupid
		foreach (Tile tile in tiles) {
			if(tile.state != Tile.TileState.empty)
				tile.Reset();
			if((tile.idX == 1 && tile.idY == 12)||(tile.idX == 6 && tile.idY == 12)||(tile.idX == 6 && tile.idY == 3))
				tile.clickable = true;
			else
				tile.clickable = false;
		}
		isVSHuman = true;
	}
	
	public void compPlay(){
		Debug.Log ("Computer");
		tiles [1, 12].Reset ();
		//startMenu.transform.GetChild (2).gameObject.SetActive (true);
		//startMenu.transform.GetChild (3).gameObject.SetActive (true);
		startMenu.transform.GetChild (2).gameObject.SetActive (true); //red
		startMenu.transform.GetChild (3).gameObject.SetActive (true); //blue
		startMenu.transform.GetChild (4).gameObject.SetActive (true); //idiot
		startMenu.transform.GetChild (5).gameObject.SetActive (true); //stupid
		isVSHuman = false;
		foreach (Tile tile in tiles) {
			if((tile.idX==1 && tile.idY==12)||(tile.idX==1 && tile.idY==9)||(tile.idX==1 && tile.idY==6)||(tile.idX==6 && tile.idY==12)||(tile.idX==6 && tile.idY==9)||(tile.idX==6 && tile.idY==6)||(tile.idX==6 && tile.idY==3))
			{
				tile.clickable = true;
			}
			else
			{
				tile.clickable = false;
			}
		}
	}

	public void isPlayerOneRed(){
		tiles [6, 9].Reset ();
		Debug.Log ("RED");
		isPlayer1Turn = true;
		foreach (Tile tile in tiles) {
			if(tile.state != Tile.TileState.empty)
			{
				tile.Reset();
				tile.state = Tile.TileState.p1;
				TileClicked(tile);
			}
		}
	}

	public void isPlayerOneBlue(){
		tiles [1, 9].Reset ();
		Debug.Log ("BLUE");
		isPlayer1Turn = false;
		foreach (Tile tile in tiles) {
			if(tile.state != Tile.TileState.empty)
			{
				tile.Reset();
				tile.state = Tile.TileState.p2;
				TileClicked(tile);
			}
		}
	}

	public void isAIIdiot(){
		Debug.Log ("IDIOT");
		AILevel = 1;
	}
	
	public void isAIStupid(){
		Debug.Log ("STUPID");
		AILevel = 2;
	}

	public void isAINormal(){
		Debug.Log ("NORMAL");
		AILevel = 3;
	}
// ================================================================================= // 
// ================================================================================= //
	// GAME OVER MENU METHODS
	public void restart(){
		Debug.Log("RESTART");
		for (int x=0;x<15;x++){
			for (int y=0;y<15;y++){
				tiles[x,y].Reset();
			}
		}
		bool isPlayer1Turn = true;
		gameoverMenu.SetActive (false);
		State = gameState.start;
		startMenu.SetActive (true);
		lastClicked.Clear ();
	}

	public void quit(){
		Debug.Log("QUIT");
		Application.Quit ();
	}

// ================================================================================= //		
	
//	public static void TileClicked(Tile tile){
//		if (instance!=null && instance.State == gameState.ingame && !instance.isAnimating){
//			GameObject tempPiece = Instantiate(instance.pieceLib) as GameObject;
//			tempPiece.SetActive(true);
//			tile.AttachPiece(tempPiece);
//			//tempPiece.transform.parent = tile.GetComponentInChildren<MeshRenderer>().transform;
//			//tempPiece.transform.localPosition = new Vector3(0,0.53f,0);
//			tempPiece.GetComponent<Piece>().SetMaterial(instance.isPlayer1Turn ? instance.player1Mat : instance.player2Mat);
//			//tempPiece.GetComponentInChildren<MeshRenderer>().material = instance.isPlayer1Turn ? instance.player1Mat : instance.player2Mat;
//			tile.state = instance.isPlayer1Turn ? Tile.TileState.p1 : Tile.TileState.p2;
//			instance.StartCoroutine(instance.PutAnimation(tile));
//			if (instance.CheckWinFromTile(tile)) Debug.Log((instance.isPlayer1Turn?"player 1":"player 2")+" win!");
//			instance.isPlayer1Turn = !instance.isPlayer1Turn;
//		}
//	}

	public static void TileClicked(Tile tile){
		if (instance!=null && !instance.isAnimating){
			Callback afterClick = null;

			switch (instance.State){
			case gameState.start :
				int x = tile.idX;
				int y = tile.idY;
				if(x==1&&y==12)instance.humanPlay();
				if(x==6&&y==12)instance.compPlay();
				if(x==1&&y==9)instance.isPlayerOneRed();
				if(x==6&&y==9)instance.isPlayerOneBlue();
				if(x==1&&y==6)instance.isAIIdiot();
				if(x==6&&y==3)instance.GOClicked();
				break;
			case gameState.ingame :
				afterClick = ()=>{
					if (instance.CheckWinFromTile(tile)) Debug.Log((instance.isPlayer1Turn?"player 1":"player 2")+" win!");
				};
				break;
			case gameState.gameover :
				break;

			}
			Piece tempPiece = Instantiate<Piece>(instance.pieceLib) as Piece;
			tempPiece.gameObject.SetActive(true);
			tile.AttachPiece(tempPiece);
			tempPiece.GetComponent<Piece>().SetMaterial(instance.isPlayer1Turn ? instance.player1Mat : instance.player2Mat);
			tile.state = instance.isPlayer1Turn ? Tile.TileState.p1 : Tile.TileState.p2;
			instance.StartCoroutine(instance.PutAnimation(tile,()=>{
				if (afterClick!=null) afterClick();
			}));
			if(instance.State == gameState.ingame)instance.isPlayer1Turn = !instance.isPlayer1Turn;
			instance.lastClicked.Add(new KeyValuePair<int,int>(tile.idX, tile.idY));
			instance.lastClickedNum = instance.lastClicked.Count;
			//Debug.Log ("lastclick num :" + instance.lastClickedNum);
		}
	}

	public void Undo(){
		int lastIdx = lastClickedNum - 1;
		Debug.Log ("last idx num:" + lastIdx);
		int idX = lastClicked [lastIdx].Key;
		int idY = lastClicked [lastIdx].Value;
		tiles [idX, idY].Reset ();
		lastClicked.RemoveAt (lastIdx);
		Debug.Log ("lastclick num :" + instance.lastClickedNum);
		isPlayer1Turn = !isPlayer1Turn;
		lastClickedNum = lastClicked.Count;
	}

	public static void ResetBoard(){
		if (instance!=null){
			for (int x = 0;x<15;x++){
				for (int y=0;y<15;y++){
					instance.tiles[x,y].Reset();
				}
			}
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
			if (maxRow >= 5) {
				Piece[] winningPiece = new Piece[5];
				int winX = lastSameTile[i].idX;
				int winY = lastSameTile[i].idY;
				for (int winIdx=0;winIdx<5;winIdx++){

					winningPiece[winIdx]=tiles[winX,winY].GetComponentInChildren<Piece>();
					if (winX<lastSameTile[i+4].idX){
						winX++;
					} else if (winX>lastSameTile[i+4].idX){
						winX--;
					}
					if (winY<lastSameTile[i+4].idY){
						winY++;
					} else if (winY>lastSameTile[i+4].idY){
						winY--;
					}
				}
				StartCoroutine(GameFinishAnimation(winningPiece));
				break;
			}
		}
		if (maxRow < 5) {
			checkNumberOfStraightRow = maxRow;
			playerLastPosX = lastSameTile[lastSameTile.Length - 1].idX;
			playerLastPosY = lastSameTile[lastSameTile.Length - 1].idY;
		}
		return (maxRow >=5);
	}

	IEnumerator GameFinishAnimation(Piece[] winningPiece){
		isAnimating = true;
		Vector3[] initialPosition = new Vector3[5];
		SFXManager.PlayOneShot(SFXManager.SFX.powerUp);
		for (int i=0;i<5;i++){
			winningPiece[i].Blink(10);
			winningPiece[i].GetComponent<Animator>().SetBool("isHyped",true);
			initialPosition[i] = winningPiece[i].transform.position;
			winningPiece[i].GetComponentInParent<Tile>().DetachPiece();
		}
		for (float t=0;t<0.5f;t+=Time.deltaTime){
			yield return 0;
		}
		for (int i=0;i<5;i++){
			winningPiece[i].Blink(10);
		}
		for (float t=0;t<2;t+=Time.deltaTime*3){
			float height = 1-(1-t)*(1-t);
			float scale = Mathf.Min(2-t,1);
			for (int i=0;i<5;i++){
				winningPiece[i].transform.position = new Vector3(winningPiece[i].transform.position.x,0,winningPiece[i].transform.position.z);
				winningPiece[i].transform.position = Vector3.Lerp(initialPosition[i],initialPosition[2],t/2f);
				winningPiece[i].transform.position = new Vector3(winningPiece[i].transform.position.x,height,winningPiece[i].transform.position.z);
				winningPiece[i].transform.localScale = new Vector3(scale,scale,scale);
			}
			yield return 0;
		}
		for (int i=0;i<4;i++){
			Destroy(winningPiece[i].gameObject);
		}
		SFXManager.PlayOneShot(SFXManager.SFX.jump);
		winningPiece[4].transform.localScale = new Vector3(10,10,10);
		winningPiece[4].GetComponent<Animator>().SetBool("isGiant",true);
		//for (float t=-0.5f;t<1;t+=Time.deltaTime){
		//	winningPiece[4].transform.position = Vector3.Lerp(initialPosition[2],Vector3.zero,Mathf.Max(0,t));
		//}
		State = gameState.gameover;
		for (float t=0;t<2;t+=Time.deltaTime){
			yield return 0;
		}
		gameoverMenu.SetActive (true);
		isAnimating = false;
		SFXManager.PlayOneShot(SFXManager.SFX.win);
		yield return 0;
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

	IEnumerator PutAnimation(Tile centerTile, Callback callback = null){
		isAnimating = true;
		SelectionTile.SetClickable(false);
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
		SelectionTile.SetClickable(true);
		if (callback!=null) callback();
		yield return 0;
	}
}
