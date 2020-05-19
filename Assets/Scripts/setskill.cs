using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class setskill : MonoBehaviour
{
	public bool rush;
	public int set;
    // Start is called before the first frame update
	public void doit()
	{
		if (rush) {
			hero._h.rushnum = set;
		} else hero._h.atknum = set;
	}
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
