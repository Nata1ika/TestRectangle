using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class Game : MonoBehaviour 
{
	public Camera				cameraUI;
	public Transform			canvas;
	public Transform			parentRectangle;
	public Transform			parentLine;

	void Start()
	{
		//определяем координаты вершин базовой области для создания прмоугольников
		RectTransform tr = this.gameObject.GetComponent<RectTransform>();
		Vector3 pos = tr.position / canvas.localScale.x;
		float width = tr.rect.size.x / 2f;
		float height = tr.rect.size.y / 2f;

		_bgVertexPos = new Vector2[3];
		_bgVertexPos[0] = new Vector2(pos.x + width, pos.y + height);
		_bgVertexPos[1] = new Vector2(pos.x + width, pos.y - height);
		_bgVertexPos[2] = new Vector2(pos.x - width, pos.y + height);

		//тут будет хрранить все прямоугольники
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

	//нажатие на пустое место базовой области
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

	//удалить все прямоугольники и связи
	public void NewGame()
	{
		for (int i=_rectangles.Count-1; i>=0; i--)
		{
			_rectangles[i].Remove();
		}
	}

	//удалить все связи
	public void ClearLine()
	{
		for (int i=0; i<_rectangles.Count; i++)
		{
			_rectangles[i].lineController.RemoveAll(_rectangles[i]);
		}
	}

	//определяем положение точки относительно других прямоугольников
	//exclusion - исключение, этот прямоуголь ник проверять не нужно, так как точка является его вершиной
	//-2 точка вылезла за область экрана; -1 нет совпадений с другими прямоугольниками; (0 - _rectangles.Count-1) индекс прямоугольника
	public int CanAdd(Vector3 pos, Rectangle exclusion)
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

	//добавить новый прямоугольник в заданную точку
	void Add(Vector3 pos)
	{
		Rectangle rectangle = null;
		if (LoadPrefab.LoadUIPrefab(ref rectangle, _rectanglePrefab, null, parentRectangle))
		{
			rectangle.transform.position = pos;
			rectangle.Init(this);
			rectangle.RemoveEvent += Remove;
			_rectangles.Add(rectangle);
		}
	}

	//удален прямоугольник
	void Remove(Rectangle rectangle)
	{
		rectangle.RemoveEvent -= Remove;
		_rectangles.Remove(rectangle);
	}

	//содержит ли прямоугольник точку
	bool ContainsPoint(Vector3 point, Vector2[] vertex) 
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

	[SerializeField] Button				_bg; //базовая область
	[SerializeField] GameObject			_rectanglePrefab; //префаб прямоугольника

	List<Rectangle>						_rectangles; //список всех прямоугольниколв на сцене

	float								_width = -1f; //ширина прямоугольников
	float								_height = -1f; //высота прямоугольников

	Vector2[]							_bgVertexPos; //координаты вершин базовой области
}
