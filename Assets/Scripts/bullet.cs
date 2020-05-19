using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
	public bool destroyan;
	public bool animated;
	public bool rotaterl;
	public bool fly;
	public bool teleport;
	public bool destroycoll;
	public bool facingright;
	public bool parashot;
	public bool destroydone;
	public Transform leg;
	public Animation anim;
	float curtimeout=0;
	public float speed;
	public float grad = 1;
	Vector3 tsel;
	public float angle1;
	public float para,parb,parc;
	bool godown = false;
	public void launch()
	{
		if (teleport) {
			gameObject.transform.position = hero._h.legpoint.position + new Vector3(((float)rnd._r.Next(200) - 100) / 100, ((float)rnd._r.Next(200) - 100) / 100, 0);
		}
	}
	public void st()
	{
		Destroy(gameObject, 30);
		if (parashot) {
			tsel= hero._h.gameObject.transform.position + new Vector3(((float)rnd._r.Next(200) - 100) / 100, ((float)rnd._r.Next(200) - 100) / 100, 0);
			float d = 1 / Mathf.Cos(angle1 * Mathf.Deg2Rad);
			d = d * d - 1;
			d = Mathf.Sqrt(d);
			if (angle1 < 0)
				d = -d;
			Vector3 v;
			if (facingright)
				v = new Vector3(1, d);
			else
				v = new Vector3(-1, d);
			v.Normalize();
			v *= 0.01f;
			v += leg.position;
			float x1 = leg.position.x, y1 = leg.position.y, x2 = v.x, y2 = v.y, x3 = tsel.x, y3 = tsel.y;
			para = (y3 - (x3 * (y2 - y1) + x2 * y1 - x1 * y2) / (x2 - x1)) / (x3 * (x3 - x1 - x2) + x1 * x2);
			parb = (y2 - y1) / (x2 - x1) - para * (x1 + x2);
			parc = (x2 * y1 - x1 * y2) / (x2 - x1) + para * x1 * x2;
		}
		if (animated) {
			anim = gameObject.GetComponent<Animation>();
			anim.Play();
		}
	}
	// Start is called before the first frame update
	void Start()
    {
	}

    // Update is called once per frame
    void Update()
    {
		if (fly) {
			curtimeout += Time.deltaTime;
			if (curtimeout >= 0.01) {
				curtimeout = 0;
				Vector3 v = transform.right * speed;
				if (mapcontroller._mp.check(leg.position + v)) {
					gameObject.transform.position += v;
				} else if (destroycoll) {
					Destroy(gameObject);
				}
			}
		} else if (parashot) {
			curtimeout += Time.deltaTime;
			if (curtimeout >= 0.01) {
				curtimeout = 0;
				if (!godown) {
					float yy = leg.position.y + speed;
					float D = parb * parb - 4 * para * (parc - yy);
					if (D < 0) {
						godown = true;
					} else {
						float xx;
						if (facingright)
							xx = (-parb + Mathf.Sqrt(D)) / (2 * para);
						else xx = (-parb - Mathf.Sqrt(D)) / (2 * para);
						Vector3 v = new Vector3(xx, yy) - leg.position;
						v.Normalize();
						v *= speed;
						gameObject.transform.position += v;
					}
				} else {
					float yy = leg.position.y - speed;
					float D = parb * parb - 4 * para * (parc - yy);
					float xx;
					if (!facingright)
						xx = (-parb + Mathf.Sqrt(D)) / (2 * para);
					else xx = (-parb - Mathf.Sqrt(D)) / (2 * para);
					Vector3 v = new Vector3(xx, yy) - leg.position;
					v.Normalize();
					v *= speed;
					gameObject.transform.position += v;
				}
			}
		}
		if (animated && destroyan && !anim.isPlaying) {
			Destroy(gameObject);
		}
    }
}
