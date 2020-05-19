using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class hero: MonoBehaviour
{
	public static hero _h;
	[Header("Attack")]
	public int hp;
	public Sprite hpfull;
	public Sprite hpempty;
	[Header("Move")]
	Vector3 move = Vector3.zero;
	public float speed;
	float curtimeout = 0;
	Vector3 rushvector = Vector3.zero;
	public bool facingright = true;
	public Animation mark;
	public bool rushfinding = false;
	public Transform legpoint;
	
	[Header("UI")]
	public string state = "free";
	public int rushnum = 0;
	public int atknum = 0;

	[Header("Animation")]
	public Animation anim;
	public AnimationClip stay;
	public AnimationClip[] rushanim = new AnimationClip[3];
	public float[] rushlen = new float[3];
	public AnimationClip[] atkanim = new AnimationClip[3];
	bool startslow = false;

	public void startrush()
	{
		move = rushvector * speed;
	}
	public void portal()
	{
		Vector3 v =  rushvector * speed * rushlen[rushnum];
		if (mapcontroller._mp.check(legpoint.transform.position + v))
			gameObject.transform.position += v;
		else for(float i = 0.99f; i > 0; i -= 0.02f) {
				v = v / (i + 0.02f) * i;
				if (mapcontroller._mp.check(legpoint.transform.position + v)) {
					//Debug.Log(i);
					gameObject.transform.position += v;
					break;
				}
			}
	}
	public void stoprush()
	{
		move = Vector3.zero;
	}
	public void takedamage()
	{
		hp--;
		CameraShake.Shake(0.3f, 0.2f);
		rnd._r.hpsp[hp].sprite = hpempty;
		if (hp <= 0)
			rnd._r.endgame(false, false);
	}
	public void timeslow()
	{
		if (state == "free") {
			startslow = false;
			//Time.timeScale = 0;
			mark.gameObject.SetActive(true);
			mark.Play();
			rushfinding = true;
		} else {
			startslow = true;
		}
	}
	public void rush(Vector3 dir)
	{
		startslow = false;
		Time.timeScale = 1;
		rushfinding = false;
		mark.gameObject.SetActive(false);
		if (state == "free") {
			state = "rush";
			if (dir.x >= 0 && !facingright) {
				facingright = !facingright;
				gameObject.transform.rotation *= Quaternion.AngleAxis(180, Vector3.up);
			} else if (dir.x < 0 && facingright) {
				facingright = !facingright;
				gameObject.transform.rotation *= Quaternion.AngleAxis(180, Vector3.up);
			}
			anim.Play(rushanim[rushnum].name, PlayMode.StopAll);
			//if (facingright)
				rushvector = dir;
			//else rushvector = new Vector3(-dir.x, dir.y);
		}
	}
	public void attack(Vector3 dir)
	{
		if (state == "free" && !rushfinding) {
			state = "atk";
			if (dir.x >= 0 && !facingright) {
				facingright = !facingright;
				gameObject.transform.rotation *= Quaternion.AngleAxis(180, Vector3.up);
			} else if (dir.x < 0 && facingright) {
				facingright = !facingright;
				gameObject.transform.rotation *= Quaternion.AngleAxis(180, Vector3.up);
			}
			anim.Play(atkanim[atknum].name, PlayMode.StopAll);
		} else {
			if (rushfinding) {
				startslow = true;
				Time.timeScale = 1;
				rushfinding = false;
				mark.gameObject.SetActive(false);
				state = "atk";
				if (dir.x >= 0 && !facingright) {
					facingright = !facingright;
					gameObject.transform.rotation *= Quaternion.AngleAxis(180, Vector3.up);
				} else if (dir.x < 0 && facingright) {
					facingright = !facingright;
					gameObject.transform.rotation *= Quaternion.AngleAxis(180, Vector3.up);
				}
				anim.Play(atkanim[atknum].name, PlayMode.StopAll);
			}
		}
	}
	// Start is called before the first frame update
	void Start()
	{
		_h = this;
	}

	// Update is called once per frame
	void Update()
	{
		curtimeout += Time.deltaTime;
		if (curtimeout >= 0.01) {
			if(mapcontroller._mp.check(legpoint.transform.position + move))
				gameObject.transform.position+=move;
			curtimeout = 0;
		}
		if(rushfinding && !mark.isPlaying) {
			Time.timeScale = 0;
		}
		if (!anim.isPlaying) {
			state = "free";
			anim.Play(stay.name);
			if (startslow) {
				timeslow();
			}
		}
	}
}
