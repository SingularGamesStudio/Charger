using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class rnd : MonoBehaviour
{
	public static rnd _r;
	public System.Random r = new System.Random();
	public List<Image> hpsp;
	public bool check = false;
	public menu mm;
	// Start is called before the first frame update
	void Start()
    {
		_r = this;
    }
	public int Next(int a)
	{
		return r.Next(a);
	}
	public void endgame(bool win, bool f)
	{
		if (win) {
			int lvl = PlayerPrefs.GetInt("level");
			if (lvl == 89) {
				if (f)
					if(mm.maxlevel)
						lvl++;
			} else {
				if (mm.maxlevel)
					lvl++;
			}
			PlayerPrefs.SetInt("level", lvl);
			PlayerPrefs.SetInt("win", 1);
		} else {
			PlayerPrefs.SetInt("win", 0);
		}
		PlayerPrefs.Save();
		Application.LoadLevel(Application.loadedLevel);
	}
    // Update is called once per frame
    void Update()
    {
		if (check) {
			check = false;
			if (GameObject.FindGameObjectsWithTag("enemy").Length == 0) {
				rnd._r.endgame(true, false);
			}
		}
    }
}
