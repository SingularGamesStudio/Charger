using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class menu : MonoBehaviour
{
	public static menu _mn;
	public Camera mc;
	public GameObject gameui;
	[System.Serializable]
	public class lvl {
		public GameObject fon;
		public Color camcolor;
		public List<int> enemies;
		public GameObject spawner;
		public bool open;
	}
	[System.Serializable]
	public class chapter
	{
		public List<lvl> levels;
		public bool open;
	}
	public List<chapter> chapters;
	public List<GameObject> enemies;
	public List<Sprite> intsp;
	public Sprite locked;
	public Sprite nullsp;
	public Sprite nosp;
	public List<GameObject> heroes;
	public List<bool> heroesopen;
	public Image storymode;
	public Image infinitemode;
	public int chapternow;
	public int lvlnow;
	public int heronow;
	public List<Image> lvlsprites;
	public List<Image> chaptersprites;
	public List<Image> herosprites;
	public List<GameObject> rushbtn;
	public List<GameObject> atkbtn;
	public Animation marker;
	public bool maxlevel = true;
	// Start is called before the first frame update
	void Start()
    {
		PlayerPrefs.DeleteAll();
		if (PlayerPrefs.HasKey("level")) {
			lvlnow = PlayerPrefs.GetInt("level")%10;
			chapternow = PlayerPrefs.GetInt("level") / 10;
		}
		if (PlayerPrefs.HasKey("hero")) {
			heronow = PlayerPrefs.GetInt("hero");
		}
		PlayerPrefs.SetInt("level", chapternow * 10 + lvlnow);
		PlayerPrefs.SetInt("hero", heronow);
		PlayerPrefs.Save();
		for(int i = 0; i<=chapternow; i++) {
			chapters[i].open = true;
			for(int j = 0; j < 10; j++) {
				chapters[i].levels[j].open = true;
			}
		}
		for (int i = chapternow+1; i < 10; i++) {
			chapters[i].open = false;
		}
		for (int j = lvlnow+1; j < 10; j++) {
			chapters[chapternow].levels[j].open = false;
		}
		updall();
	}
	public void exitgame(){
		Application.Quit();
	}
	public void addlevel(int add)
	{
		if (lvlnow + add >= 0 && lvlnow + add < chapters[chapternow].levels.Count && chapters[chapternow].levels[lvlnow + add].open) {
			chapters[chapternow].levels[lvlnow].fon.SetActive(false);
			lvlnow += add;
			updall();
		}
	}
	public void addhero(int add)
	{
		if (heronow + add >= 0 && heronow + add < heroes.Count && heroesopen[heronow + add]) {
			heronow += add;
			PlayerPrefs.SetInt("hero", heronow);
			updall();
		}
	}
	public void addchapter(int add)
	{
		if (chapternow + add >= 0 && chapternow + add < chapters.Count && chapters[chapternow + add].open) {
			chapters[chapternow].levels[lvlnow].fon.SetActive(false);
			chapternow += add;
			lvlnow = 0;
			updall();
		}
	}
	public void startgame()
	{
		if(chapternow*10+lvlnow== PlayerPrefs.GetInt("level")) {
			maxlevel = true;
		} else {
			maxlevel = false;
		}
		if (heroes[heronow].GetComponent<hero>().atkanim.Length == 1) {
			atkbtn[1].SetActive(false);
			atkbtn[0].SetActive(false);
		}
		if (heroes[heronow].GetComponent<hero>().rushanim.Length < 3) {
			rushbtn[2].SetActive(false);
		}
		if (heroes[heronow].GetComponent<hero>().rushanim.Length == 1) {
			rushbtn[1].SetActive(false);
			rushbtn[0].SetActive(false);
		}
		for (int  i = 0; i<heroes[heronow].GetComponent<hero>().hp; i++) {
			rnd._r.hpsp[i].gameObject.SetActive(true);
			rnd._r.hpsp[i].sprite = heroes[heronow].GetComponent<hero>().hpfull;
		}
		for (int i = heroes[heronow].GetComponent<hero>().hp; i < 10; i++) {
			rnd._r.hpsp[i].gameObject.SetActive(false);
		}
		GameObject h;
		GameObject h1 = Instantiate(chapters[chapternow].levels[lvlnow].spawner);
		for (int i = 0; i < chapters[chapternow].levels[lvlnow].enemies.Count; i++) {
			h = Instantiate(enemies[chapters[chapternow].levels[lvlnow].enemies[i]]);
			h.transform.position = h1.transform.GetChild(i).position+(h.transform.position-h.GetComponent<ultimateenemy>().legpoint.position);
		}
		float hh = (chapters[chapternow].levels[lvlnow].fon.transform.lossyScale.x / 2) * chapters[chapternow].levels[lvlnow].fon.transform.GetComponent<SpriteRenderer>().sprite.texture.height / 100;
		float ww = (chapters[chapternow].levels[lvlnow].fon.transform.lossyScale.y / 2) * chapters[chapternow].levels[lvlnow].fon.transform.GetComponent<SpriteRenderer>().sprite.texture.width / 100;
		if (ww / Screen.width * Screen.height < hh) {
			Debug.Log("a");
			mc.orthographicSize = hh;
		} else mc.orthographicSize = ww / Screen.width * Screen.height;
		
		h = Instantiate(heroes[heronow]);
		h.transform.position = Vector3.zero;
		h.GetComponent<hero>().mark = marker;
		gameui.SetActive(true);
		gameObject.SetActive(false);
	}
	void updall()
	{
		chapters[chapternow].levels[lvlnow].fon.SetActive(true);
		mc.backgroundColor = chapters[chapternow].levels[lvlnow].camcolor;
		if (chapternow == 0) {
			chaptersprites[0].sprite = nullsp;
		} else if (chapters[chapternow - 1].open) {
			chaptersprites[0].sprite = intsp[chapternow - 1];
		} else chaptersprites[0].sprite = locked;

		chaptersprites[1].sprite = intsp[chapternow];

		if (chapternow == chapters.Count-1) {
			chaptersprites[2].sprite = nullsp;
		} else if (chapters[chapternow + 1].open) {
			chaptersprites[2].sprite = intsp[chapternow + 1];
		} else chaptersprites[2].sprite = locked;

		if (lvlnow == 0) {
			lvlsprites[0].sprite = nullsp;
		} else if (chapters[chapternow].levels[lvlnow - 1].open) {
			lvlsprites[0].sprite = intsp[lvlnow - 1];
		} else lvlsprites[0].sprite = locked;

		lvlsprites[1].sprite = intsp[lvlnow];

		if (lvlnow == chapters[chapternow].levels.Count - 1) {
			lvlsprites[2].sprite = nullsp;
		} else if (chapters[chapternow].levels[lvlnow+1].open) {
			lvlsprites[2].sprite = intsp[lvlnow + 1];
		} else lvlsprites[2].sprite = locked;


		if (heronow == 0) {
			herosprites[0].sprite = nosp;
		} else if (heroesopen[heronow-1]) {
			herosprites[0].sprite = heroes[heronow - 1].transform.GetChild(0).GetComponent<SpriteRenderer>().sprite;
		} else herosprites[0].sprite = nosp;

		herosprites[1].sprite = heroes[heronow].transform.GetChild(0).GetComponent<SpriteRenderer>().sprite;

		if (heronow == heroes.Count-1) {
			herosprites[2].sprite = nosp;
		} else if (heroesopen[heronow + 1]) {
			herosprites[2].sprite = heroes[heronow + 1].transform.GetChild(0).GetComponent<SpriteRenderer>().sprite;
		} else herosprites[2].sprite = nosp;
	}
	// Update is called once per frame
	void Update()
    {
        
    }
}
