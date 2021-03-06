﻿using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	delegate void Callback();

	static GameManager instance;

	public Tile tileLib;
	public List<TileMenuButton> menuButtons;
	public Piece pieceLib;
	public GameObject startMenu;
	//public GameObject gameoverMenu;
	public GameObject undoMenu;
	public Material player1Mat, player2Mat;
	Tile[,] tiles;
	public enum gameState{
		start, ingame, gameover
	}

	public gameState State;
	bool isPlayer1Turn = true;
	public static bool isAnimating = false;
	[HideInInspector] public bool isVSHuman = false;
	public bool isPlayAsRed = true;
	int AILevel = 1;

	const float WOBLEDELAY = 0.05f;

	private List<KeyValuePair<int,int>> lastClicked;
	private int lastClickedNum;
	int playerLastPosX, playerLastPosY;


	void Awake () {
		instance = this;
		State = gameState.start;
		//gameoverMenu.SetActive (false);
	}
	void Start () {
		menuButtons = new List<TileMenuButton>();
		lastClicked = new List<KeyValuePair<int,int>> ();
		CameraEffect.MenuPerpective();
		tiles = new Tile[15,15];
		for (int x=0;x<15;x++){
			for (int y=0;y<15;y++){
				tiles[x,y] = Instantiate<Tile>(tileLib) as Tile;
				tiles[x,y].SetTile(x,y);
			}
		}
		//SettingVsAI();
		isAnimating = true;
		GameUI.TitleScreenExit(()=>{
			isAnimating = false;
			MenuMainComputer();
			SettingVsAI();
		});
	}
	void Update(){

		if (!isAnimating && State == gameState.ingame){
			GameUI.SetUndo(lastClicked.Count>0);
		}

		if (Input.GetKeyDown(KeyCode.U) && !isAnimating && State == gameState.ingame){
			Debug.Log("undo");
			if (isVSHuman){
				if(Undo()){
					if (isPlayer1Turn){
						GameUI.RedFocus();
					} else {
						GameUI.BlueFocus();
					}
				}
			} else {
				Debug.Log("Undo");
				Undo();
				Debug.Log(isPlayer1Turn);
				Undo();
				Debug.Log(isPlayer1Turn);
			}
		}

		if (!isAnimating && State == gameState.ingame && !isVSHuman && ((!isPlayer1Turn && isPlayAsRed) || (isPlayer1Turn && !isPlayAsRed)) ) {
			int posX = 0;
			int posY = 0;
			switch(AILevel){
			case 1: //idiot
				while(tiles[posX,posY].state != Tile.TileState.empty){
					posX = (int)(Random.value * 15);
					posY = (int)(Random.value * 15);
				}
				break;
			case 2: //very stupid
				int random = (int)(Random.value*8);
				Debug.Log (random);
				switch(random){
					case 1:
						if(tiles[lastClicked[lastClickedNum - 1].Key - 1, lastClicked[lastClickedNum - 1].Value + 1].state == Tile.TileState.empty){
							posX = lastClicked[lastClickedNum - 1].Key - 1;
							posY = lastClicked[lastClickedNum - 1].Value  + 1;
						}
					break;

					case 2:
						if(tiles[lastClicked[lastClickedNum - 1].Key, lastClicked[lastClickedNum - 1].Value + 1].state == Tile.TileState.empty){
							posX = lastClicked[lastClickedNum - 1].Key;
							posY = lastClicked[lastClickedNum - 1].Value + 1;
						}
					break;

					case 3:
						if(tiles[lastClicked[lastClickedNum - 1].Key + 1, lastClicked[lastClickedNum - 1].Value + 1].state == Tile.TileState.empty){
							posX = lastClicked[lastClickedNum - 1].Key + 1;
							posY = lastClicked[lastClickedNum - 1].Value  + 1;
						}
					break;

					case 4:
						if(tiles[lastClicked[lastClickedNum - 1].Key + 1, lastClicked[lastClickedNum - 1].Value].state == Tile.TileState.empty){
							posX = lastClicked[lastClickedNum - 1].Key + 1;
							posY = lastClicked[lastClickedNum - 1].Value;
						}
					break;

					case 5:
						if(tiles[lastClicked[lastClickedNum - 1].Key + 1, lastClicked[lastClickedNum - 1].Value - 1].state == Tile.TileState.empty){
							posX = lastClicked[lastClickedNum - 1].Key + 1;
							posY = lastClicked[lastClickedNum - 1].Value  - 1;
						}
					break;

					case 6:
						if(tiles[lastClicked[lastClickedNum - 1].Key, lastClicked[lastClickedNum - 1].Value - 1].state == Tile.TileState.empty){
							posX = lastClicked[lastClickedNum - 1].Key;
							posY = lastClicked[lastClickedNum - 1].Value - 1;
						}
					break;

					case 7:
						if(tiles[lastClicked[lastClickedNum - 1].Key - 1, lastClicked[lastClickedNum - 1].Value - 1].state == Tile.TileState.empty){
							posX = lastClicked[lastClickedNum - 1].Key - 1;
							posY = lastClicked[lastClickedNum - 1].Value  - 1;
						}
					break;

					case 8:
					if(tiles[lastClicked[lastClickedNum - 1].Key - 1, lastClicked[lastClickedNum - 1].Value].state == Tile.TileState.empty){
							posX = lastClicked[lastClickedNum - 1].Key - 1;
							posY = lastClicked[lastClickedNum - 1].Value;
						}
					break;
					}
				break;
            }
			TileClicked(tiles[posX,posY]);
		    
	        }
			if (lastClickedNum > 0)
				undoMenu.SetActive (true);
			else
				undoMenu.SetActive (false);
		}


	public void CreateStageHeight(){
		for (int x=0;x<15;x++){
			for (int y=0;y<15;y++){
				tiles[x,y].ColorDefault();
				tiles[x,y].clickable = false;
				tiles[x,y].transform.DOMove(
					new Vector3(tiles[x,y].transform.position.x,
				            0f,
				            tiles[x,y].transform.position.z),
					0.5f);
			}
		}

		foreach(TileMenuButton t in menuButtons){
			int xMin = (int)t.rect.x;
			int xMax = (int)t.rect.x + (int)t.rect.width;
			int yMin = (int)t.rect.y;
			int yMax = (int)t.rect.y + (int)t.rect.height;
			for (int x=xMin; x<xMax; x++){
				for (int y=yMin; y<yMax; y++){
					tiles[x,y].ColorHighlight();
					tiles[x,y].clickable = true;
					tiles[x,y].transform.DOMove(
						new Vector3(tiles[x,y].transform.position.x,
					            1f,
					            tiles[x,y].transform.position.z),
						0.5f);
				}
			}
		}
	}

	public static TileMenuButton isOnMenuButton(int x, int y){
		if (instance!=null){
			foreach (TileMenuButton t in instance.menuButtons){
				int xMin = (int)t.rect.x;
				int xMax = (int)t.rect.x + (int)t.rect.width;
				int yMin = (int)t.rect.y;
				int yMax = (int)t.rect.y + (int)t.rect.height;
				if (x>= xMin && x<xMax && y>=yMin && y<yMax){
					return t;
				}
			}
			return null;
		}
		return null;
	}

	public void MenuClear(){
		startMenu.SetActive(false);
		menuButtons.Clear();
		CreateStageHeight();
	}

	public void MenuMainHuman()
	{
		menuButtons.Clear();

		startMenu.SetActive(true);
		startMenu.GetComponent<MenuText>().MenuMainHuman();

		menuButtons.Add(new TileMenuButton(TileMenuButton.RECT_HUMAN,
		                                   tiles[(int)TileMenuButton.RECT_HUMAN.x, (int)TileMenuButton.RECT_HUMAN.y] ,
		                                   SettingVsHuman));
		menuButtons.Add(new TileMenuButton(TileMenuButton.RECT_COMPUTER,
		                                   tiles[(int)TileMenuButton.RECT_COMPUTER.x, (int)TileMenuButton.RECT_COMPUTER.y] ,
		                                   SettingVsAI));
		menuButtons.Add(new TileMenuButton(TileMenuButton.RECT_GO,
		                                   tiles[(int)TileMenuButton.RECT_GO.x, (int)TileMenuButton.RECT_GO.y] ,
		                                   StartGame));
		CreateStageHeight();
	}

	public void MenuMainComputer()
	{
		menuButtons.Clear();

		startMenu.SetActive(true);
		startMenu.GetComponent<MenuText>().MenuMainComputer();

		menuButtons.Add(new TileMenuButton(TileMenuButton.RECT_HUMAN,
		                                   tiles[(int)TileMenuButton.RECT_HUMAN.x, (int)TileMenuButton.RECT_HUMAN.y] ,
		                                   SettingVsHuman));
		menuButtons.Add(new TileMenuButton(TileMenuButton.RECT_COMPUTER,
		                                   tiles[(int)TileMenuButton.RECT_COMPUTER.x, (int)TileMenuButton.RECT_COMPUTER.y] ,
		                                   SettingVsAI));
		menuButtons.Add(new TileMenuButton(TileMenuButton.RECT_RED,
		                                   tiles[(int)TileMenuButton.RECT_RED.x, (int)TileMenuButton.RECT_RED.y] ,
		                                   SettingPlayAsRed));
		menuButtons.Add(new TileMenuButton(TileMenuButton.RECT_BLUE,
		                                   tiles[(int)TileMenuButton.RECT_BLUE.x, (int)TileMenuButton.RECT_BLUE.y] ,
		                                   SettingPlayAsBlue));
		menuButtons.Add(new TileMenuButton(TileMenuButton.RECT_IDIOT,
		                                   tiles[(int)TileMenuButton.RECT_IDIOT.x, (int)TileMenuButton.RECT_IDIOT.y] ,
		                                   SettingAIIdiot));
		menuButtons.Add(new TileMenuButton(TileMenuButton.RECT_STUPID,
		                                   tiles[(int)TileMenuButton.RECT_STUPID.x, (int)TileMenuButton.RECT_STUPID.y] ,
		                                   SettingAIStupid));
		menuButtons.Add(new TileMenuButton(TileMenuButton.RECT_GO,
		                                   tiles[(int)TileMenuButton.RECT_GO.x, (int)TileMenuButton.RECT_GO.y] ,
		                                   StartGame));
		CreateStageHeight();
	}

	public void MenuGameFinish(){
		menuButtons.Clear();
		startMenu.SetActive(true);
		startMenu.GetComponent<MenuText>().MenuGameFinish();
		menuButtons.Add(new TileMenuButton(TileMenuButton.RECT_RESTART,
		                                   tiles[(int)TileMenuButton.RECT_HUMAN.x, (int)TileMenuButton.RECT_HUMAN.y] ,
		                                   ()=>{
			GameUI.WinExit();
			StartGame();
		}));
		menuButtons.Add(new TileMenuButton(TileMenuButton.RECT_MAINMENU,
		                                   tiles[(int)TileMenuButton.RECT_COMPUTER.x, (int)TileMenuButton.RECT_COMPUTER.y] ,
		                                   ()=>{
			MenuMainComputer();
			SettingVsAI();
			GameUI.TitleNameEnter();
			GameUI.WinExit();
		}));
		CreateStageHeight();
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
	public void StartGame(){
		lastClicked.Clear();
		lastClickedNum = 0;
		SFXManager.PlayOneShot(SFXManager.SFX.ping,0.7f,0.9f);
		GameUI.CharEntrance();
		GameUI.TitleNameExit();
		CameraEffect.GameplayPerspective();
		MenuClear();
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

	public void SettingVsHuman(){
		Debug.Log ("Human");
		SFXManager.PlayOneShot(SFXManager.SFX.ping,0.7f,0.9f);
		for (int i=0;i<menuButtons.Count;i++){
			if (i!=0) 
			{
				menuButtons[i].pivot.Reset();
			} else {
				menuButtons[i].pivot.OnMouseDown();
			}
		}

		isVSHuman = true;
		MenuMainHuman();
	}
	
	public void SettingVsAI(){
		MenuMainComputer();
		SFXManager.PlayOneShot(SFXManager.SFX.ping,0.7f,0.86f);
		Debug.Log ("Computer");
		tiles [1, 12].Reset ();
		//startMenu.transform.GetChild (2).gameObject.SetActive (true);
		//startMenu.transform.GetChild (3).gameObject.SetActive (true);
		isVSHuman = false;

		menuButtons[0].pivot.Reset();
		menuButtons[1].pivot.OnMouseDown();
		isAnimating = false;
		if (isPlayAsRed){
			menuButtons[2].pivot.OnMouseDown();
			menuButtons[3].pivot.Reset();
		} else {
			menuButtons[2].pivot.Reset();
			menuButtons[3].pivot.OnMouseDown();
		}
		isAnimating = false;
		if (AILevel == 1){
			menuButtons[4].pivot.OnMouseDown();
			menuButtons[5].pivot.Reset();
		} else {
			menuButtons[4].pivot.Reset();
			menuButtons[5].pivot.OnMouseDown();
		}
	}

	public void SettingPlayAsRed(){
		Debug.Log ("RED");
		SFXManager.PlayOneShot(SFXManager.SFX.ping,0.7f,0.83f);
		isPlayAsRed = true;
		menuButtons[2].pivot.OnMouseDown();
		menuButtons[3].pivot.Reset();
		for (int i=0;i<menuButtons.Count;i++){
			Piece p = menuButtons[i].pivot.GetComponentInChildren<Piece>();
			if (p!=null){
				p.SetMaterial(player1Mat);
			}
		}
	}

	public void SettingPlayAsBlue(){
		Debug.Log ("BLUE");
		SFXManager.PlayOneShot(SFXManager.SFX.ping,0.7f,0.8f);
		isPlayAsRed = false;
		menuButtons[2].pivot.Reset();
		menuButtons[3].pivot.OnMouseDown();
		for (int i=0;i<menuButtons.Count;i++){
			Piece p = menuButtons[i].pivot.GetComponentInChildren<Piece>();
			if (p!=null){
				p.SetMaterial(player2Mat);
			}
		}
	}

	public void SettingAIIdiot(){
		Debug.Log ("IDIOT");
		SFXManager.PlayOneShot(SFXManager.SFX.ping,0.7f,0.76f);
		AILevel = 1;
		menuButtons[4].pivot.OnMouseDown();
		menuButtons[5].pivot.Reset();
	}
	
	public void SettingAIStupid(){
		Debug.Log ("STUPID");
		SFXManager.PlayOneShot(SFXManager.SFX.ping,0.7f,0.73f);
		AILevel = 2;
		menuButtons[4].pivot.Reset();
		menuButtons[5].pivot.OnMouseDown();
	}

	public void SettingAINormal(){
		Debug.Log ("NORMAL");
		SFXManager.PlayOneShot(SFXManager.SFX.ping,0.7f,0.9f);
		AILevel = 3;
	}
// ================================================================================= // 
// ================================================================================= //
	// GAME OVER MENU METHODS
	public void restart(){
		Debug.Log("RESTART");
		SFXManager.PlayOneShot(SFXManager.SFX.ping,0.7f,0.8f);
		for (int x=0;x<15;x++){
			for (int y=0;y<15;y++){
				tiles[x,y].Reset();
			}
		}
		bool isPlayer1Turn = true;
		//gameoverMenu.SetActive (false);
		State = gameState.start;
		startMenu.SetActive (true);
		lastClicked.Clear ();
	}

	public void quit(){
		Debug.Log("QUIT");
		Application.Quit ();
	}

	public static void TileClicked(Tile tile){
		if (instance!=null && !isAnimating){
			Callback afterClick = null;

			switch (instance.State){
			case gameState.start :
				break;
			case gameState.ingame :
				instance.lastClicked.Add(new KeyValuePair<int,int>(tile.idX, tile.idY));
				instance.lastClickedNum = instance.lastClicked.Count;
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
				if(instance.State == gameState.ingame){
					instance.SwitchTurn();
				}
			}));
			//Debug.Log ("lastclick num :" + instance.lastClickedNum);
		}
	}

	public bool Undo(){
		if (lastClicked.Count>0){

			int lastIdx = lastClickedNum - 1;
			//Debug.Log ("last idx num:" + lastIdx);
			int idX = lastClicked [lastIdx].Key;
			int idY = lastClicked [lastIdx].Value;
			tiles [idX, idY].Reset ();
			lastClicked.RemoveAt (lastIdx);
			//Debug.Log ("lastclick num :" + instance.lastClickedNum);
			Debug.Log(lastClicked.Count);
			isPlayer1Turn = !isPlayer1Turn;
			lastClickedNum = lastClicked.Count;
			return true;
		}
		return false;
	}

	public void SwitchTurn(){
		instance.isPlayer1Turn = !instance.isPlayer1Turn;
		if (instance.isPlayer1Turn){
			GameUI.RedFocus();
		} else {
			GameUI.BlueFocus();
		}
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
		return (maxRow >=5);
	}

	IEnumerator GameFinishAnimation(Piece[] winningPiece){
		isAnimating = true;
		State = gameState.gameover;
		Tile center = winningPiece[2].GetComponentInParent<Tile>();

		GameUI.CharExit();
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
		for (float t=0;t<2;t+=Time.deltaTime){
			yield return 0;
		}
		CameraEffect.MenuPerpective();
		GameUI.WinEntrance(isPlayer1Turn);
		MenuGameFinish();
		//gameoverMenu.SetActive (true);
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

	public static void BigSlamWoble(int _x, int _y){
		if (instance!=null){
			for (int x = 0;x<15;x++){
				for (int y = 0; y<15; y++){
					float distance = Mathf.Sqrt((_x-x)*(_x-x)+(_y-y)*(_y-y));
					//instance.tiles[x,y].GetComponentInChildren<Renderer>().material.DOFade(1,distance/25f).OnComplete(()=>{
						instance.tiles[x,y].Woble(Mathf.Max( 2-distance/7,0));
					//});
				}
			}
		}
	}

	IEnumerator PutAnimation(Tile centerTile, Callback callback = null){
		isAnimating = true;
		SelectionTile.SetClickable(false);
		float t=0;
		yield return new WaitForSeconds(0.28f-WOBLEDELAY);
		SFXManager.PlayOneShot(SFXManager.SFX.vibrato);
		if (State == gameState.ingame){
			centerTile.Woble(1);
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
		}
		yield return new WaitForSeconds(0.5f);
		isAnimating = false;
		SelectionTile.SetClickable(true);
		if (callback!=null) callback();
		yield return 0;
	}
}
