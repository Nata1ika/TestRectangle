﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class Game : MonoBehaviour 
{
	public Camera				cameraUI;
	public Transform			canvas;

	void Start()
	{
		RectTransform tr = this.gameObject.GetComponent<RectTransform>();
		Vector3 pos = tr.position / canvas.localScale.x;
		float width = tr.rect.size.x / 2f;
		float height = tr.rect.size.y / 2f;

		_bgVertexPos = new Vector2[3];
		_bgVertexPos[0] = new Vector2(pos.x + width, pos.y + height);
		_bgVertexPos[1] = new Vector2(pos.x + width, pos.y - height);
		_bgVertexPos[2] = new Vector2(pos.x - width, pos.y + height);

		_rectangles = new List<Rectangle>();
	}

	public Rectangle GetRectangle(int index)
	{
		if (_rectangles == null || _rectangles.Count <= index)
		{
			return null;
		}
		else
		{
			return _rectangles[index];
		}
	}

	public void OnClick()
	{
		Vector3 pos = cameraUI.ScreenToWorldPoint(Input.mousePosition);
		pos.z = 0f;
		//проверка что влезет
		if (CanAdd(pos, null) == -1)
		{
			Add(pos);
		}
	}

	public int CanAdd(Vector3 pos, Rectangle exclusion) //-2 точка вылезла за область экрана; -1 нет совпадений с другими прямоугольниками; (0 - _rectangles.Count-1) индекс прямоугольника
	{
		if (_width <= 0 || _height <= 0)
		{
			RectTransform _tr = _rectanglePrefab.GetComponent<RectTransform>();
			_width = _tr.rect.size.x / 2f;
			_height = _tr.rect.size.y / 2f;
		}

		//вершины для нового прямоугольника
		Vector3 [] vertex = new Vector3[4];
		pos /= canvas.localScale.x;
		vertex[0] = new Vector3(pos.x + _width, pos.y + _height, 0);
		vertex[1] = new Vector3(pos.x + _width, pos.y - _height, 0);
		vertex[2] = new Vector3(pos.x - _width, pos.y + _height, 0);
		vertex[3] = new Vector3(pos.x - _width, pos.y - _height, 0);

		for (int i=0; i<vertex.Length; i++)
		{
			//проверка что влазит в область экрана
			if (! ContainsPoint(vertex[i], _bgVertexPos))
			{
				return -2;
			}
			//проверка что не задевает другие 
			if (_rectangles != null)
			{
				Vector2[] vertexBase = new Vector2[3];
				Vector3 posBase;
				for (int j=0; j<_rectangles.Count; j++)
				{
					if (exclusion != _rectangles[j])
					{
						//получить вершины для _rectangles[j] и проверить что точка не принадлежит прямоугольнику
						posBase = _rectangles[j].transformOverride.position / canvas.localScale.x;
						vertexBase[0] = new Vector2(posBase.x + _width, posBase.y + _height);
						vertexBase[1] = new Vector2(posBase.x + _width, posBase.y - _height);
						vertexBase[2] = new Vector2(posBase.x - _width, posBase.y + _height);

						if (ContainsPoint(vertex[i], vertexBase))
						{
							return j;
						}
					}
				}
			}
		}

		return -1;
	}

	void Add(Vector3 pos)
	{
		Rectangle rectangle = null;
		if (LoadPrefab.LoadUIPrefab(ref rectangle, _rectanglePrefab, null, this.gameObject.transform))
		{
			rectangle.transform.position = pos;
			rectangle.Init(this);
			rectangle.RemoveEvent += Remove;
			_rectangles.Add(rectangle);
		}
	}

	void Remove(Rectangle rectangle)
	{
		rectangle.RemoveEvent -= Remove;
		_rectangles.Remove(rectangle);
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

	[SerializeField] Button				_bg;
	[SerializeField] Button				_newGame;

	[SerializeField] GameObject			_rectanglePrefab;

	List<Rectangle>				_rectangles;
	float						_width = -1f;
	float						_height = -1f;

	Vector2[]					_bgVertexPos;
}
