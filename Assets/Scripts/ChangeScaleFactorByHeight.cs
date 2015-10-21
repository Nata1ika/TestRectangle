using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

[RequireComponent (typeof(CanvasScaler))]
public class ChangeScaleFactorByHeight : MonoBehaviour 
{
	[Serializable]
	public class HeightToScale
	{
		public float minHeight;
		public float scaleFactor;	
	}

	public float defaultScale = 1f;
	public HeightToScale[] scaleFactors;

	void Awake()
	{
		var currentScaler = GetComponent <CanvasScaler>();

		currentScaler.scaleFactor = defaultScale;

		foreach (var scaleOption in scaleFactors)
		{
			if (Screen.height >= scaleOption.minHeight)
			{
				currentScaler.scaleFactor = scaleOption.scaleFactor;
			}
		}
	}
}
