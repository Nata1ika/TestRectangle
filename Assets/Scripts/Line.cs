using UnityEngine;
using System.Collections;

public class Line : MonoBehaviour
{
	public void SetPosition(Vector3 pos1, Vector3 pos2, float scale)
	{
		overrideTransform.position = (pos1 + pos2) / 2f;
		overrideTransform.sizeDelta = new Vector2((pos2 - pos1).magnitude / scale, height);
		overrideTransform.rotation = Quaternion.FromToRotation(Vector3.right, (pos2 - pos1));
	}

	public void Remove()
	{
		GameObject.Destroy(gameObject);
	}

	RectTransform overrideTransform
	{
		get
		{
			if (_transform == null)
			{
				_transform = gameObject.GetComponent<RectTransform>();
			}
			return _transform;
		}
	}

	float height
	{
		get
		{
			if (_height < 0)
			{
				_height = overrideTransform.rect.height;
			}
			return _height;
		}
	}

	RectTransform	_transform;
	float			_height = -1f;
}
