using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankItemView : MonoBehaviour {
	public int rank;
	//public BigNumber score;
	public string name;

	public Image rankIcon;
	public Text rankText;
	public Text nameText;
	public Text assertText;
	public Image icon;


	public void SetName(string ne) {
		if(!string.IsNullOrEmpty(ne))
			nameText.text = ne;
	}

	public void SetIcon(Texture2D tex) {
		if (tex != null) {
			Rect rct = Rect.zero;
			if (tex != null && tex.width >= 96) {
				rct = new Rect (0, 0, 96, 96);
				Sprite sprite = Sprite.Create (tex, rct, Vector2.zero);
				icon.sprite = sprite;
			} else if(tex != null){
				rct = new Rect (0, 0, tex.width, tex.height);
				Sprite sprite = Sprite.Create(tex, rct, Vector2.zero);
				icon.sprite = sprite;
			}
		}
	}

	public void SetRankUnkonw(string rk) {
		rankText.gameObject.SetActive (true);
		rankIcon.gameObject.SetActive (false);
		rankText.text = rk;
	}

	public void InitView(int rk, string ne, BigNumber sc) {
		if (rk <= 3 && rk > 0) {
			rankText.gameObject.SetActive (false);
			rankIcon.gameObject.SetActive (true);
			rankIcon.transform.GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Sprites/medal" + rk);
		} else {
			rankIcon.gameObject.SetActive (false);
		} 

		rankText.text = rk.ToString();
		nameText.text = ne;
		assertText.text = sc.ToString ();

		if (rk < 0) {
			SetRankUnkonw("--");
		}
		//Debug.Log (string.Format ("rank:{0} name:{1} assert{2}", rk.ToString (), ne, sc.ToString ()));
		
	}
}
