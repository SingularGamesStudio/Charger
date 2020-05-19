using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buttonsizecontroller : MonoBehaviour
{
	public bool right = false;
	public bool up;
	public static class RectTransformExtensions
	{
		public static void SetDefaultScale(RectTransform trans)
		{
			trans.localScale = new Vector3(1, 1, 1);
		}
		public static void SetPivotAndAnchors(RectTransform trans, Vector2 aVec)
		{
			trans.pivot = aVec;
			trans.anchorMin = aVec;
			trans.anchorMax = aVec;
		}

		public static Vector2 GetSize(RectTransform trans)
		{
			return trans.rect.size;
		}
		public static float GetWidth(RectTransform trans)
		{
			return trans.rect.width;
		}
		public static float GetHeight(RectTransform trans)
		{
			return trans.rect.height;
		}

		public static void SetPositionOfPivot(RectTransform trans, Vector2 newPos)
		{
			trans.localPosition = new Vector3(newPos.x, newPos.y, trans.localPosition.z);
		}

		public static void SetLeftBottomPosition(RectTransform trans, Vector2 newPos)
		{
			trans.localPosition = new Vector3(newPos.x + (trans.pivot.x * trans.rect.width), newPos.y + (trans.pivot.y * trans.rect.height), trans.localPosition.z);
		}
		public static void SetLeftTopPosition(RectTransform trans, Vector2 newPos)
		{
			trans.localPosition = new Vector3(newPos.x + (trans.pivot.x * trans.rect.width), newPos.y - ((1f - trans.pivot.y) * trans.rect.height), trans.localPosition.z);
		}
		public static void SetRightBottomPosition(RectTransform trans, Vector2 newPos)
		{
			trans.localPosition = new Vector3(newPos.x - ((1f - trans.pivot.x) * trans.rect.width), newPos.y + (trans.pivot.y * trans.rect.height), trans.localPosition.z);
		}
		public static void SetRightTopPosition(RectTransform trans, Vector2 newPos)
		{
			trans.localPosition = new Vector3(newPos.x - ((1f - trans.pivot.x) * trans.rect.width), newPos.y - ((1f - trans.pivot.y) * trans.rect.height), trans.localPosition.z);
		}

		public static void SetSize(RectTransform trans, Vector2 newSize)
		{
			Vector2 oldSize = trans.rect.size;
			Vector2 deltaSize = newSize - oldSize;
			trans.offsetMin = trans.offsetMin - new Vector2(deltaSize.x * trans.pivot.x, deltaSize.y * trans.pivot.y);
			trans.offsetMax = trans.offsetMax + new Vector2(deltaSize.x * (1f - trans.pivot.x), deltaSize.y * (1f - trans.pivot.y));
		}
		public static void SetWidth(RectTransform trans, float newSize)
		{
			SetSize(trans, new Vector2(newSize, trans.rect.size.y));
		}
		public static void SetHeight(RectTransform trans, float newSize)
		{
			SetSize(trans, new Vector2(trans.rect.size.x, newSize));
		}
	}

	public List<RectTransform> buttons;
    // Start is called before the first frame update
    void Start()
    {
		if (!up) {
			float h = RectTransformExtensions.GetHeight(gameObject.GetComponent<RectTransform>());
			RectTransformExtensions.SetSize(gameObject.GetComponent<RectTransform>(), new Vector2(h / 5, h));
			Vector3 v = RectTransformExtensions.GetSize(gameObject.transform.parent.GetComponent<RectTransform>());
			if (!right)
				RectTransformExtensions.SetLeftTopPosition(gameObject.GetComponent<RectTransform>(), new Vector2(-v.x / 2, v.y / 2));
			else
				RectTransformExtensions.SetRightBottomPosition(gameObject.GetComponent<RectTransform>(), new Vector2(v.x / 2, -v.y / 2));

			for (int i = 0; i < buttons.Count; i++) {
				if (i == 1 || i == 3) {
					RectTransformExtensions.SetSize(buttons[i], new Vector2(h / 5 * 0.7f, h / 5 * 0.7f));
					RectTransformExtensions.SetLeftTopPosition(buttons[i], new Vector2(-h / 10 * 0.7f, -h / 5 * i + h / 2 - h / 5 * 0.15f));
				} else {
					RectTransformExtensions.SetSize(buttons[i], new Vector2(h / 5, h / 5));
					RectTransformExtensions.SetLeftTopPosition(buttons[i], new Vector2(-h / 10, -h / 5 * i + h / 2));
				}
			}
		} else {
			float h = RectTransformExtensions.GetHeight(gameObject.transform.parent.gameObject.GetComponent<RectTransform>());
			float d = RectTransformExtensions.GetWidth(gameObject.transform.parent.gameObject.GetComponent<RectTransform>());
			d -= 2 * (h / 5);
			d -= h / 10;
			RectTransformExtensions.SetSize(gameObject.GetComponent<RectTransform>(), new Vector2(d, d/5));
			RectTransformExtensions.SetLeftTopPosition(gameObject.GetComponent<RectTransform>(), new Vector2(-d/2, d / 10));
			for (int i = 0; i < buttons.Count; i++) {
				if (i == 1 || i == 3) {
					RectTransformExtensions.SetSize(buttons[i], new Vector2(d / 5 * 0.7f, d / 5 * 0.7f));
					RectTransformExtensions.SetLeftTopPosition(buttons[i], new Vector2( d / 5 * i - d / 2 + d / 5 * 0.15f,d / 10 * 0.7f));
				} else {
					RectTransformExtensions.SetSize(buttons[i], new Vector2(d / 5, d / 5));
					RectTransformExtensions.SetLeftTopPosition(buttons[i], new Vector2(d / 5 * i - d / 2,d / 10));
				}
			}
		}
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
