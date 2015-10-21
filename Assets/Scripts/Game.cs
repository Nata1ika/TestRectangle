using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class Game : MonoBehaviour 
{
	void Start()
	{
		RectTransform tr = this.gameObject.GetComponent<RectTransform>();
		Vector3 pos = tr.position / _canvas.localScale.x;
		float width = tr.rect.size.x / 2f;
		float height = tr.rect.size.y / 2f;

		_bgVertexPos = new Vector2[3];
		_bgVertexPos[0] = new Vector2(pos.x + width, pos.y + height);
		_bgVertexPos[1] = new Vector2(pos.x + width, pos.y - height);
		_bgVertexPos[2] = new Vector2(pos.x - width, pos.y + height);

		_rectangles = new List<Rectangle>();
	}

	public void OnClick()
	{
		Vector3 pos = _camera.ScreenToWorldPoint(Input.mousePosition);
		pos.z = 0f;
		//проверка что влезет
		if (! ContainsPoint(pos))
		{
			Add(pos);
		}
	}

	bool ContainsPoint(Vector3 pos)
	{
		if (_width <= 0 || _height <= 0)
		{
			RectTransform _tr = _rectanglePrefab.GetComponent<RectTransform>();
			if (_tr != null)
			{
				_width = _tr.rect.size.x / 2f;
				_height = _tr.rect.size.y / 2f;
			}
			else
			{
				return true;
			}
		}
		Vector3 [] vertex = new Vector3[4];
		pos /= _canvas.localScale.x;
		vertex[0] = new Vector3(pos.x + _width, pos.y + _height, 0);
		vertex[1] = new Vector3(pos.x + _width, pos.y - _height, 0);
		vertex[2] = new Vector3(pos.x - _width, pos.y + _height, 0);
		vertex[3] = new Vector3(pos.x - _width, pos.y - _height, 0);

		for (int i=0; i<vertex.Length; i++)
		{
			if (! ContainsPoint(vertex[i], _bgVertexPos))
			{
				return true;
			}
			if (_rectangles != null)
			{
				for (int j=0; j<_rectangles.Count; j++)
				{
					//получить вершины для _rectangles[j] и проверить что точка не принадлежит прямоугольнику
				}
			}
		}

		return false;
	}

	void Add(Vector3 pos)
	{
		Rectangle rectangle = null;
		if (LoadPrefab.LoadUIPrefab(ref rectangle, _rectanglePrefab, null, this.gameObject.transform))
		{
			rectangle.transform.position = pos;
			_rectangles.Add(rectangle);
		}
	}

	bool ContainsPoint(Vector3 point, Vector2[] vertex) //содержит ли прямоугольник точку
	{
		if (point.x <= vertex[0].x && point.x >= vertex[2].x && point.y <= vertex[0].y && point.y >= vertex[1].y)
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	[SerializeField] Button		_bg;
	[SerializeField] Button		_newGame;

	[SerializeField] GameObject	_rectanglePrefab;
	[SerializeField] Camera		_camera;
	[SerializeField] Transform	_canvas;

	List<Rectangle>				_rectangles;
	float						_width = -1f;
	float						_height = -1f;

	Vector2[]					_bgVertexPos;
}
