using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hitbox : MonoBehaviour
{
	public bool enemy;
	public GameObject owner;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }
	private void OnTriggerEnter2D(Collider2D coll)
	{
			//Debug.Log(coll.gameObject.name);
			if (enemy) {
				if (coll.gameObject.tag == "plhit") {
					owner.GetComponent<ultimateenemy>().takedamage(coll.gameObject.GetComponent<dmghpstun>().stats, coll.gameObject.transform.position);
				}
			} else {
				if (coll.gameObject.tag == "enhit") {
					owner.GetComponent<hero>().takedamage();
				}
			}
	}
}
