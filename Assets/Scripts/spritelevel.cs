using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spritelevel : MonoBehaviour
{
	SpriteRenderer main;
    // Start is called before the first frame update
    void Start()
    {
		main = gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
		main.sortingOrder = -(int)((transform.position.y + 5.29f) * 100);
    }
}
