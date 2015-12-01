using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

public class GameFinishUI : MonoBehaviour {

	public static GameFinishUI instance;
	public Image winImage;
	public Sprite winRedLib, winBlueLib;

	const float POSY = 80;
	const float POSXIN = 0;
	const float POSXOUT = 800;

	void Awake () {
		instance = this;
	}

	void Start () {
		winImage.enabled = false;
	}

	public static void Entrance(bool isRed) {
		if (instance!=null){
			instance.winImage.enabled = true;
			instance.winImage.sprite = isRed ? instance.winRedLib : instance.winBlueLib;

			instance.winImage.color = new Color(1,1,1,0);
			instance.winImage.DOColor(new Color(1,1,1,1),0.5f);
			
			instance.winImage.rectTransform.anchoredPosition = new Vector2( isRed ? -POSXOUT : POSXOUT,POSY);
			DOTween.To(()=>instance.winImage.rectTransform.anchoredPosition,
			           (Vector2 anchorPos)=>{instance.winImage.rectTransform.anchoredPosition = anchorPos;},
			new Vector2(POSXIN,POSY),0.5f).SetEase(Ease.OutBack) ;
		}
	}

	public static void Exit(){
		if (instance!=null){
			instance.winImage.DOColor(new Color(1,1,1,0),0.5f).OnComplete(()=>instance.winImage.enabled = false);
		}
	}

}
