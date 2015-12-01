using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MenuText : MonoBehaviour {
	public Text[] texts;

	public void MenuMainHuman(){
		texts[0].gameObject.SetActive (true); //human
		texts[1].gameObject.SetActive (true); //AI
		texts[2].gameObject.SetActive (false); //red
		texts[3].gameObject.SetActive (false); //blue
		texts[4].gameObject.SetActive (false); //idiot
		texts[5].gameObject.SetActive (false); //stupid
		texts[6].gameObject.SetActive (true); //go
		texts[7].gameObject.SetActive (false); //restart
		texts[8].gameObject.SetActive (false); //mainmenu
	}

	public void MenuMainComputer(){
		texts[0].gameObject.SetActive (true); //human
		texts[1].gameObject.SetActive (true); //AI
		texts[2].gameObject.SetActive (true); //red
		texts[3].gameObject.SetActive (true); //blue
		texts[4].gameObject.SetActive (true); //idiot
		texts[5].gameObject.SetActive (true); //stupid
		texts[6].gameObject.SetActive (true); //go
		texts[7].gameObject.SetActive (false); //restart
		texts[8].gameObject.SetActive (false); //mainmenu
	}

	public void MenuGameFinish(){
		texts[0].gameObject.SetActive (false); //human
		texts[1].gameObject.SetActive (false); //AI
		texts[2].gameObject.SetActive (false); //red
		texts[3].gameObject.SetActive (false); //blue
		texts[4].gameObject.SetActive (false); //idiot
		texts[5].gameObject.SetActive (false); //stupid
		texts[6].gameObject.SetActive (false); //go
		texts[7].gameObject.SetActive (true); //restart
		texts[8].gameObject.SetActive (true); //mainmenu
	}
}
