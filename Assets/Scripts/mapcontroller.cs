using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mapcontroller : MonoBehaviour
{
	public static mapcontroller _mp;
	[System.Serializable]
	public class halfcircle
	{
		public bool filled;
		public Vector3 center;
		public Vector3 firstpoint;
	}
	[System.Serializable]
	public class line
	{
		public Vector3 p1;
		public Vector3 p2;
	}
	[System.Serializable]
	public class square
	{
		public bool filled;
		public Vector3 pru;
		public Vector3 pld;
	}
	public List<halfcircle> circles;
	public List<line> lines;
	public List<square> squares;
	// Start is called before the first frame update
	void Start()
    {
		_mp = this;
    }
	public bool check(Vector3 v)
	{
		for(int i = 0; i < lines.Count; i++) {
			Vector3 v2 = lines[i].p2 - lines[i].p1;
			Vector3 v1 = v- lines[i].p1;
			if (v2.x * v1.y - v2.y * v1.x > 0) {
				return false;
			}

		}
		for (int i = 0; i < squares.Count; i++) {
			if (v.x < squares[i].pru.x && v.x > squares[i].pld.x && v.y < squares[i].pru.y && v.y > squares[i].pld.y) {
				if (squares[i].filled)
					return false;
			} else if (!squares[i].filled) return false;

		}
		for (int i = 0; i < circles.Count; i++) {
			float r = Vector3.Distance(circles[i].center, circles[i].firstpoint);
			Debug.Log(r);
			if (Vector3.Distance(circles[i].center, v) > r) {
				Debug.Log("out");
				if (circles[i].filled) {
					continue;
				}
			} else {
				Debug.Log("in");
				if (!circles[i].filled) continue;
			}

			Vector3 v2 = circles[i].center - circles[i].firstpoint;
			v2 *= 2;
			Vector3 v1 = v - circles[i].firstpoint;
			if (v2.x * v1.y - v2.y * v1.x > 0) {
				return false;
			}

		}
		return true;
	}
    // Update is called once per frame
    void Update()
    {
        
    }
}
