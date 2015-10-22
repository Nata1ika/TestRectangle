using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class Rectangle : MonoBehaviour 
{
	public Action<Rectangle> 			RemoveEvent;
	public Action<Vector3>				MoveEvent;
	public Action<Rectangle, Rectangle>	LinkEvent;

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
		if (_countClick >= 2 && UnityEngine.Mathf.Abs(_timeFirstClick - time) <= _timeDeltaClick) //двойной клик - удаляем прямоугольник
		{
			Remove();
		}
		else //прямоугольник подвинулся
		{
			int index = _game.CanAdd(transformOverride.position, this);
			//-2 прямоугольник вылез за допустимую область. его нужно подвинуть в первоначальное место
			// >= 0 движение привело к наложению
			if (index == -2 || index >= 0) 
			{
				transformOverride.position = _posBeforeDrag;
				if (MoveEvent != null)
				{
					MoveEvent(_posBeforeDrag);
				}
				if (index >= 0 && LinkEvent != null)
				{
					LinkEvent(this, _game.GetRectangle(index));
				}

			}
			//else if (index == -1) {} //прямоугольник подвинули правильно ничего не делаем
		}
	}

	public void OnDrag()
	{
		Debug.Log("Drag");

		Vector3 pos = _game.cameraUI.ScreenToWorldPoint(Input.mousePosition);
		pos.z = 0f;

		transformOverride.position = pos;

		if (MoveEvent != null)
		{
			MoveEvent(pos);
		}
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
