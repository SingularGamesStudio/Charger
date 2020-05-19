using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ultimateenemy: MonoBehaviour
{
	public bool teleports;
	public bool pursues;
	public bool shots;
	public bool shots2;
	public bool attacks;
	public bool rotates;
	public bool gorandomly;
	public bool gosamey;
	public bool building;
	[Header("Animation")]
	public Animation anim;
	public AnimationClip walk;
	public AnimationClip teleport;
	public AnimationClip attack;
	public AnimationClip shotanim;
	public AnimationClip shotanim2;
	public AnimationClip die;
	[Header("Pursue")]
	public float speed;
	float curtimeout = 0;
	public bool facingright = false;
	public float maxlen;
	public Transform legpoint;
	string state;
	[Header("Attack")]
	public int hp;
	int stun = 0;
	int push = 0;
	bool st = false;
	Vector3 stunmove = Vector3.zero;
	public List<GameObject> allanim;
	public GameObject basesp;
	public GameObject stunsp;
	[HideInInspector]
	public int dead = 0;
	public GameObject dielastsp;
	public GameObject missle;
	public GameObject missle1;
	[Header("Teleport")]
	public int telechance;
	public int shotchance;
	public int shotchance2;
	public int gochance;
	[HideInInspector]
	public Vector3 teletsel;
	[HideInInspector]
	public Vector3 gotsel;
	// Start is called before the first frame update
	void Start()
	{
		state = "free";
	}
	void stopanim()
	{
		anim.Stop();
		for (int i = 0; i < allanim.Count; i++) {
			allanim[i].SetActive(false);
		}
		basesp.SetActive(true);
	}
	public void shot()
	{
		GameObject h = Instantiate(missle);
		h.transform.position = gameObject.transform.position;
		if (h.GetComponent<bullet>() != null) {
			
			if (h.GetComponent<bullet>().rotaterl) {
				if (h.GetComponent<bullet>().facingright != facingright) {
					h.transform.rotation *= Quaternion.AngleAxis(180, Vector3.up);
					h.GetComponent<bullet>().facingright = !h.GetComponent<bullet>().facingright;
				}
			}
			h.GetComponent<bullet>().st();
		}
	}
	public void portal()
	{
		gameObject.transform.position = teletsel;
	}
	public void shot1()
	{
		GameObject h = Instantiate(missle1);
		h.transform.position = gameObject.transform.position;
		if (h.GetComponent<bullet>() != null) {
			h.GetComponent<bullet>().st();
			if (h.GetComponent<bullet>().rotaterl) {
				if (h.GetComponent<bullet>().facingright != facingright) {
					h.transform.rotation *= Quaternion.AngleAxis(180, Vector3.up);
					h.GetComponent<bullet>().facingright = !h.GetComponent<bullet>().facingright;
				}
			}
		}
	}
	public void takedamage(atkcontainer aaa, Vector3 from)
	{
		if (dead < 1) {
			stunmove = gameObject.transform.position - from;
			stunmove.Normalize();
			stunmove = new Vector3(stunmove.x, stunmove.y, 0);
			stunmove *= 2f;
			hp -= aaa.dmg;
			stun = aaa.stun;
			push = aaa.push;
			if (hp <= 0) {
				dead = 1;
				if (rotates) {
					if (stunmove.x >= 0) {
						if (facingright)
							gameObject.transform.rotation *= Quaternion.AngleAxis(180, Vector3.up);
						facingright = false;
					}
					if (stunmove.x < 0) {
						if (!facingright)
							gameObject.transform.rotation *= Quaternion.AngleAxis(180, Vector3.up);
						facingright = true;
					}
				}
				stopanim();
				basesp.SetActive(false);
				stunsp.SetActive(true);
			} else if (aaa.breakanim && !building) {
				if (rotates) {
					if (stunmove.x >= 0) {
						if (facingright)
							gameObject.transform.rotation *= Quaternion.AngleAxis(180, Vector3.up);
						facingright = false;
					}
					if (stunmove.x < 0) {
						if (!facingright)
							gameObject.transform.rotation *= Quaternion.AngleAxis(180, Vector3.up);
						facingright = true;
					}
				}
				stopanim();
				basesp.SetActive(false);
				stunsp.SetActive(true);
			}
		}
	}
	// Update is called once per frame
	void Update()
	{
		curtimeout += Time.deltaTime;
		
		if (curtimeout >= 0.01) {
			curtimeout = 0;
			Vector3 V = hero._h.legpoint.position - legpoint.position;
			if (push > 0) {
				push--;
				if (push <= 0) {
					st = true;
					push = stun;
					stun = 0;
				}
				if (!st) {
					if (mapcontroller._mp.check(legpoint.transform.position + stunmove * speed))
						gameObject.transform.position += stunmove * speed;
				}
			} else if (state!="attack" && attacks && V.magnitude <= maxlen) {
				if (rotates) {
					if (V.x >= 0 && !facingright) {
						facingright = true;
						gameObject.transform.rotation *= Quaternion.AngleAxis(180, Vector3.up);
					}
					if (V.x < 0 && facingright) {
						facingright = false;
						gameObject.transform.rotation *= Quaternion.AngleAxis(180, Vector3.up);
					}
				}
				state = "attack";
				anim.Stop();
				anim.Play(attack.name);
			} else
			if (state == "walk") {
				if (st) {
					stunsp.SetActive(false);
					st = false;
					if (dead == 1) {
						anim.Play(die.name);
						dead = 2;
					}
				}
				if (dead != 2) {
					if (gosamey && Mathf.Abs(gotsel.y - hero._h.legpoint.position.y) > maxlen) {
						stopanim();
						state = "free";
					} else {
						Vector3 v = gotsel - legpoint.transform.position;
						if (v.magnitude < maxlen / 2) {
							stopanim();
							state = "free";
						} else {
							v.Normalize();
							v *= speed;
							if (mapcontroller._mp.check(legpoint.transform.position + v)) {
								gameObject.transform.position += v;
							} else {
								stopanim();
								state = "free";
							}
						}
					}
				}
			} else {
				if (st) {
					stunsp.SetActive(false);
					st = false;
					if (dead == 1) {
						anim.Play(die.name);
						dead = 2;
					}
				}
				if (state == "free" && dead < 1) {
					if (V.magnitude <= maxlen && attacks) {
						if (rotates) {
							if (V.x >= 0 && !facingright) {
								facingright = true;
								gameObject.transform.rotation *= Quaternion.AngleAxis(180, Vector3.up);
							}
							if (V.x < 0 && facingright) {
								facingright = false;
								gameObject.transform.rotation *= Quaternion.AngleAxis(180, Vector3.up);
							}
						}
						state = "attack";
						anim.Play(attack.name);
					} else
					if (pursues) {
						if (V.magnitude > maxlen) {
							if (rotates) {
								if (V.x >= 0 && !facingright) {
									facingright = true;
									gameObject.transform.rotation *= Quaternion.AngleAxis(180, Vector3.up);
								}
								if (V.x < 0 && facingright) {
									facingright = false;
									gameObject.transform.rotation *= Quaternion.AngleAxis(180, Vector3.up);
								}
							}
							V.Normalize();
							V = new Vector3(V.x, V.y, 0);
							if (mapcontroller._mp.check(legpoint.transform.position + V * speed))
								gameObject.transform.position += V * speed;
						}

					}
				}
			}
		}
		if (!anim.isPlaying && push <= 0) {
			//state = "free";
			if (dead == 2) {
				rnd._r.check = true;
				dielastsp.transform.SetParent(null);
				Destroy(gameObject);
			}
			int chnc = rnd._r.Next(100) + 1;
			Vector3 V = hero._h.legpoint.position - legpoint.position;
			if (state == "walk") {
				anim.Play(walk.name);
			} else {
				if (rotates) {
					if (V.x >= 0 && !facingright) {
						facingright = true;
						gameObject.transform.rotation *= Quaternion.AngleAxis(180, Vector3.up);
					}
					if (V.x < 0 && facingright) {
						facingright = false;
						gameObject.transform.rotation *= Quaternion.AngleAxis(180, Vector3.up);
					}
				}

				if (attacks && V.magnitude <= maxlen) {
					if (rotates) {
						if (V.x >= 0 && !facingright) {
							facingright = true;
							gameObject.transform.rotation *= Quaternion.AngleAxis(180, Vector3.up);
						}
						if (V.x < 0 && facingright) {
							facingright = false;
							gameObject.transform.rotation *= Quaternion.AngleAxis(180, Vector3.up);
						}
					}
					state = "attack";
					anim.Play(attack.name);
				} else {
					if (gosamey && Mathf.Abs(legpoint.transform.position.y - hero._h.legpoint.position.y) > maxlen) {
						while (true) {
							Vector3 v = new Vector3(legpoint.position.x + (rnd._r.Next(600) - 300) / 100, hero._h.legpoint.position.y, 0);
							if (mapcontroller._mp.check(v)) {
								gotsel = v;
								v = gotsel - legpoint.position;
								if (rotates) {
									if (v.x >= 0 && !facingright) {
										facingright = true;
										gameObject.transform.rotation *= Quaternion.AngleAxis(180, Vector3.up);
									}
									if (v.x < 0 && facingright) {
										facingright = false;
										gameObject.transform.rotation *= Quaternion.AngleAxis(180, Vector3.up);
									}
								}
								state = "walk";
								anim.Play(walk.name);
								break;
							}
						}
					} else if (teleports && chnc <= telechance) {
						while (true) {
							float xx = (rnd._r.Next(2000) - 1000) / 100;
							float yy = (rnd._r.Next(2000) - 1000) / 100;
							Vector3 vv = new Vector3(xx, yy, 0);
							if (mapcontroller._mp.check(legpoint.transform.position + vv)) {
								teletsel = gameObject.transform.position + vv;
								anim.Play(teleport.name);
								break;
							}
						}
						state = "portal";
					} else
					if (shots && chnc <= shotchance) {
						anim.Play(shotanim.name);
						state = "shot1";
					} else
					if (shots2 && chnc <= shotchance2) {
						anim.Play(shotanim2.name);
						state = "shot2";
					} else if(gorandomly && chnc <= gochance) {
						while (true) {
							float xx = (rnd._r.Next(2000) - 1000) / 100;
							float yy = (rnd._r.Next(2000) - 1000) / 100;
							Vector3 v = new Vector3(xx, yy, 0);
							gotsel = v + legpoint.position;
							if (mapcontroller._mp.check(gotsel)) {
								anim.Play(walk.name);
								if (rotates) {
									if (v.x >= 0 && !facingright) {
										facingright = true;
										gameObject.transform.rotation *= Quaternion.AngleAxis(180, Vector3.up);
									}
									if (v.x < 0 && facingright) {
										facingright = false;
										gameObject.transform.rotation *= Quaternion.AngleAxis(180, Vector3.up);
									}
								}
								break;
							}
						}
						state = "walk";
					}
				}
				if (pursues && !anim.isPlaying) {
					state = "free";
					anim.Play(walk.name);
				}
			}
		}
	}
}
