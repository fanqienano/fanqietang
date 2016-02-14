using UnityEngine;
using System.Collections;

public class ClickIgnore : MonoBehaviour, ICanvasRaycastFilter
{
	
	public bool IsRaycastLocationValid (Vector2 screenPoint, Camera eventCamera)
	{
		return false;
	}
	
}
