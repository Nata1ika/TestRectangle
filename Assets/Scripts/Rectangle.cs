using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Rectangle : MonoBehaviour 
{
	public System.Action<Rectangle> RemoveEvent;

	public void Init(Game game)
	{
		_image.color = new Color(UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0.5f, 1f));
		_game = game;
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

	public void OnPointerDown()
	{
		Debug.Log("Down");
		_posBeforeDrag = transformOverride.position;

		float time = Time.time;
		if (UnityEngine.Mathf.Abs(_timeFirstClick - time) <= _timeDeltaClick)
		{
			_countClick ++;
		}
		else
		{
			_countClick = 1;
		}

		_timeFirstClick = time;

	}

	public void OnPointerUp()
	{
		Debug.Log("Up");
		float time = Time.time;
		if (_countClick >= 2 && UnityEngine.Mathf.Abs(_timeFirstClick - time) <= _timeDeltaClick)
		{
			Remove();
		}
	}

	public void OnDrag()
	{
		Debug.Log("Drag");
		Motion();
	}


	void Motion()
	{
		Vector3 pos = _game.cameraUI.ScreenToWorldPoint(Input.mousePosition);
		pos.z = 0f;

		transformOverride.position = pos;
	}

	public void Remove()
	{
		if (RemoveEvent != null)
		{
			RemoveEvent(this);
		}

		GameObject.Destroy(this.gameObject);
	}

	[SerializeField] Transform		_transform;
	[SerializeField] Button			_button;
	[SerializeField] Image			_image;

	Game							_game;
	Vector3							_posBeforeDrag;
	float							_timeFirstClick;
	int								_countClick = 0;
	const float						_timeDeltaClick = 0.5f;

}
