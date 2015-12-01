using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

public class CharacterUI : MonoBehaviour {
	public static CharacterUI instance;

	public Image redImage, blueImage;

	const float POSXOUT = 570;
	const float POSXIN = 370;
	const float POSYIN = 86;
	const float POSYOUT = 55;

	void Awake(){
		instance = this;
	}

	void Start () {
		redImage.enabled = false;
		blueImage.enabled = false;
	}

	public static void Entrance(){
		if (instance!=null){
			instance.redImage.enabled = true;
			instance.blueImage.enabled = true;
			instance.redImage.color = new Color(1,1,1,0);
			instance.redImage.DOColor(new Color(1,1,1,1),0.5f);
			instance.redImage.rectTransform.anchoredPosition = new Vector2(-POSXOUT,POSYIN);
			DOTween.To(()=>instance.redImage.rectTransform.anchoredPosition,
			           (Vector2 anchorPos)=>{instance.redImage.rectTransform.anchoredPosition = anchorPos;},
			new Vector2(-POSXIN,POSYIN),0.5f);

			instance.blueImage.color = new Color(1,1,1,0);
			instance.blueImage.DOColor(new Color(1,1,1,1),0.5f);
			instance.blueImage.rectTransform.anchoredPosition = new Vector2(POSXOUT,POSYIN);
			DOTween.To(()=>instance.blueImage.rectTransform.anchoredPosition,
			           (Vector2 anchorPos)=>{instance.blueImage.rectTransform.anchoredPosition = anchorPos;},
			new Vector2(POSXIN,POSYIN),0.5f).OnComplete(RedFocus);

		}
	}

	public static void Exit(){
		if (instance!=null){
			instance.redImage.DOColor(new Color(1,1,1,0),0.5f);
			DOTween.To(()=>instance.redImage.rectTransform.anchoredPosition,
			           (Vector2 anchorPos)=>{instance.redImage.rectTransform.anchoredPosition = anchorPos;},
			new Vector2(-POSXOUT,POSYIN),0.5f).OnComplete(()=>instance.redImage.enabled = false);

			instance.blueImage.DOColor(new Color(1,1,1,0),0.5f);
			DOTween.To(()=>instance.blueImage.rectTransform.anchoredPosition,
			           (Vector2 anchorPos)=>{instance.blueImage.rectTransform.anchoredPosition = anchorPos;},
			new Vector2(POSXOUT,POSYIN),0.5f).OnComplete(()=>instance.blueImage.enabled = false);
			
		}
	}

	public static void RedFocus(){
		instance.redImage.DOColor(new Color(1,1,1,1),0.5f);
		DOTween.To(()=>instance.redImage.rectTransform.anchoredPosition,
		           (Vector2 anchorPos)=>{instance.redImage.rectTransform.anchoredPosition = anchorPos;},
		new Vector2(-POSXIN,POSYIN),0.5f);
		
		instance.blueImage.DOColor(new Color(0.4f,0.4f,0.4f,0.6f),0.5f);
		DOTween.To(()=>instance.blueImage.rectTransform.anchoredPosition,
		           (Vector2 anchorPos)=>{instance.blueImage.rectTransform.anchoredPosition = anchorPos;},
		new Vector2(POSXIN,POSYOUT),0.5f);
	}

	public static void BlueFocus(){
		instance.redImage.DOColor(new Color(0.4f,0.4f,0.4f,0.6f),0.5f);
		DOTween.To(()=>instance.redImage.rectTransform.anchoredPosition,
		           (Vector2 anchorPos)=>{instance.redImage.rectTransform.anchoredPosition = anchorPos;},
		new Vector2(-POSXIN,POSYOUT),0.5f);
		
		instance.blueImage.DOColor(new Color(1,1,1,1),0.5f);
		DOTween.To(()=>instance.blueImage.rectTransform.anchoredPosition,
		           (Vector2 anchorPos)=>{instance.blueImage.rectTransform.anchoredPosition = anchorPos;},
		new Vector2(POSXIN,POSYIN),0.5f);
	}
}
