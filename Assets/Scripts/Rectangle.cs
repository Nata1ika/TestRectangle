using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Rectangle : MonoBehaviour 
{
	public void Init()
	{
		_image.color = new Color(UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0.5f, 1f));
	}

	public Transform transformOverride
	{
		get
		{
			if (_transform == null)
			{
				_transform = gameObject.transform;
			}
			return _transform;
		}
	}

	[SerializeField] Transform		_transform;
	[SerializeField] Button			_button;
	[SerializeField] Image			_image;
}
