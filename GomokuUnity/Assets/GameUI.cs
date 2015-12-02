using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

public class GameUI : MonoBehaviour {

	public static GameUI instance;
	public Image winImage,redImage, blueImage, titleName, bg;
	public Sprite winRedLib, winBlueLib;
	public Text undoText;

	const float WIN_POSY = 110;
	const float WIN_POSXIN = 0;
	const float WIN_POSXOUT = 800;

	const float CHAR_POSXOUT = 570;
	const float CHAR_POSXIN = 370;
	const float CHAR_POSYIN = 136;
	const float CHAR_POSYOUT = 105;

	void Awake () {
		instance = this;
	}

	void Start () {
		winImage.enabled = false;
		redImage.enabled = true;
		blueImage.enabled = true;
	}

	public static void SetUndo(bool state){
		if (instance!=null){
			instance.undoText.gameObject.SetActive(state);
		}
	}

	public static void TitleScreenExit(TweenCallback callback){
		if (instance!=null){
			GameManager.isAnimating = true;
			instance.bg.enabled = true;
			instance.bg.DOColor(new Color(0,0,0,0.7f),1.5f).OnComplete(()=>{
				SFXManager.PlayOneShot(SFXManager.SFX.ping,1,0.5f);
				CharExit();
				CameraEffect.Shake(0.5f);
				instance.bg.color = new Color(1,1,1,1);
				instance.bg.DOColor(new Color(0,0,0,0),0.5f).OnComplete(()=>instance.bg.enabled=false);
			});
			instance.titleName.rectTransform.anchoredPosition = new Vector2(0,0);
			instance.titleName.rectTransform.localScale = new Vector3(1,1,1);
			DOTween.To(()=>instance.titleName.rectTransform.anchoredPosition,
			           (Vector2 anchorPos)=>{instance.titleName.rectTransform.anchoredPosition = anchorPos;},
			new Vector2(0,250),0.5f).SetDelay(1.5f).SetEase(Ease.OutBack).OnComplete(()=>GameManager.isAnimating=false);
			instance.titleName.rectTransform.DOScale(0.5f,0.5f).SetDelay(1.5f).SetEase(Ease.OutBack).OnComplete(callback) ;
		}
	}

	public static void TitleNameEnter(){
		if (instance!=null){
			instance.titleName.DOColor(new Color(1,1,1,1),0.5f);
			DOTween.To(()=>instance.titleName.rectTransform.anchoredPosition,
			           (Vector2 anchorPos)=>{instance.titleName.rectTransform.anchoredPosition = anchorPos;},
			new Vector2(0,250),0.5f).SetEase(Ease.OutBack);
		}
	}

	public static void TitleNameExit(){
		if (instance!=null){
			instance.titleName.DOColor(new Color(1,1,1,0),0.5f);
			DOTween.To(()=>instance.titleName.rectTransform.anchoredPosition,
			           (Vector2 anchorPos)=>{instance.titleName.rectTransform.anchoredPosition = anchorPos;},
			new Vector2(0,450),0.5f).SetEase(Ease.InBack);
		}
	}

	public static void WinEntrance(bool isRed) {
		if (instance!=null){
			instance.winImage.enabled = true;
			instance.winImage.sprite = isRed ? instance.winRedLib : instance.winBlueLib;

			instance.winImage.color = new Color(1,1,1,0);
			instance.winImage.DOColor(new Color(1,1,1,1),0.5f);
			
			instance.winImage.rectTransform.anchoredPosition = new Vector2( isRed ? -WIN_POSXOUT : WIN_POSXOUT,WIN_POSY);
			DOTween.To(()=>instance.winImage.rectTransform.anchoredPosition,
			           (Vector2 anchorPos)=>{instance.winImage.rectTransform.anchoredPosition = anchorPos;},
			new Vector2(WIN_POSXIN,WIN_POSY),0.5f).SetEase(Ease.OutBack) ;
		}
	}

	public static void WinExit(){
		if (instance!=null){
			instance.winImage.DOColor(new Color(1,1,1,0),0.5f).OnComplete(()=>instance.winImage.enabled = false);
		}
	}

	public static void CharEntrance(){
		if (instance!=null){
			instance.redImage.enabled = true;
			instance.blueImage.enabled = true;
			instance.redImage.color = new Color(1,1,1,0);
			instance.redImage.DOColor(new Color(1,1,1,1),0.5f);
			instance.redImage.rectTransform.anchoredPosition = new Vector2(-CHAR_POSXOUT,CHAR_POSYIN);
			DOTween.To(()=>instance.redImage.rectTransform.anchoredPosition,
			           (Vector2 anchorPos)=>{instance.redImage.rectTransform.anchoredPosition = anchorPos;},
			new Vector2(-CHAR_POSXIN,CHAR_POSYIN),0.5f);
			
			instance.blueImage.color = new Color(1,1,1,0);
			instance.blueImage.DOColor(new Color(1,1,1,1),0.5f);
			instance.blueImage.rectTransform.anchoredPosition = new Vector2(CHAR_POSXOUT,CHAR_POSYIN);
			DOTween.To(()=>instance.blueImage.rectTransform.anchoredPosition,
			           (Vector2 anchorPos)=>{instance.blueImage.rectTransform.anchoredPosition = anchorPos;},
			new Vector2(CHAR_POSXIN,CHAR_POSYIN),0.5f).OnComplete(RedFocus);
			
		}
	}
	
	public static void CharExit(){
		if (instance!=null){
			instance.redImage.DOColor(new Color(1,1,1,0),0.5f);
			DOTween.To(()=>instance.redImage.rectTransform.anchoredPosition,
			           (Vector2 anchorPos)=>{instance.redImage.rectTransform.anchoredPosition = anchorPos;},
			new Vector2(-CHAR_POSXOUT,CHAR_POSYIN),0.5f).OnComplete(()=>instance.redImage.enabled = false);
			
			instance.blueImage.DOColor(new Color(1,1,1,0),0.5f);
			DOTween.To(()=>instance.blueImage.rectTransform.anchoredPosition,
			           (Vector2 anchorPos)=>{instance.blueImage.rectTransform.anchoredPosition = anchorPos;},
			new Vector2(CHAR_POSXOUT,CHAR_POSYIN),0.5f).OnComplete(()=>instance.blueImage.enabled = false);
			
		}
	}
	
	public static void RedFocus(){
		instance.redImage.DOColor(new Color(1,1,1,1),0.5f);
		DOTween.To(()=>instance.redImage.rectTransform.anchoredPosition,
		           (Vector2 anchorPos)=>{instance.redImage.rectTransform.anchoredPosition = anchorPos;},
		new Vector2(-CHAR_POSXIN,CHAR_POSYIN),0.5f);
		
		instance.blueImage.DOColor(new Color(0.4f,0.4f,0.4f,0.6f),0.5f);
		DOTween.To(()=>instance.blueImage.rectTransform.anchoredPosition,
		           (Vector2 anchorPos)=>{instance.blueImage.rectTransform.anchoredPosition = anchorPos;},
		new Vector2(CHAR_POSXIN,CHAR_POSYOUT),0.5f);
	}
	
	public static void BlueFocus(){
		instance.redImage.DOColor(new Color(0.4f,0.4f,0.4f,0.6f),0.5f);
		DOTween.To(()=>instance.redImage.rectTransform.anchoredPosition,
		           (Vector2 anchorPos)=>{instance.redImage.rectTransform.anchoredPosition = anchorPos;},
		new Vector2(-CHAR_POSXIN,CHAR_POSYOUT),0.5f);
		
		instance.blueImage.DOColor(new Color(1,1,1,1),0.5f);
		DOTween.To(()=>instance.blueImage.rectTransform.anchoredPosition,
		           (Vector2 anchorPos)=>{instance.blueImage.rectTransform.anchoredPosition = anchorPos;},
		new Vector2(CHAR_POSXIN,CHAR_POSYIN),0.5f);
	}
}
