using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LineController : MonoBehaviour 
{
	public class ConformityLine
	{
		public Rectangle 	rectangle;
		public Line			line;

		public ConformityLine(Rectangle setRectangle, Line setLine)
		{
			rectangle = setRectangle;
			line = setLine;
		}
	}

	public List<ConformityLine>	listLine;

	Rectangle thisRectangle
	{
		get
		{
			if (_thisRectangle == null)
			{
				_thisRectangle = gameObject.GetComponent<Rectangle>();
			}
			return _thisRectangle;
		}
	}

	void Start()
	{
		listLine = new List<ConformityLine>();

		thisRectangle.RemoveEvent += RemoveAll;
		thisRectangle.MoveEvent += Move;
		thisRectangle.LinkEvent += Link;
	}

	void OnDestroy()
	{
		thisRectangle.RemoveEvent -= RemoveAll;
		thisRectangle.MoveEvent -= Move;
		thisRectangle.LinkEvent -= Link;
	}

	public void RemoveAll(Rectangle rectangle) //удаляются все связи для текущего прямоугольника, в т.ч. физически
	{
		for (int i=0; i<listLine.Count; i++)
		{
			listLine[i].rectangle.lineController.Remove(thisRectangle);
			listLine[i].line.Remove();
		}

		listLine.Clear();
	}

	public void Remove(Rectangle rectangle) //удаляются связь этого и rectangle прямоугольников. удаление только из списка
	{
		for (int i=0; i<listLine.Count; i++)
		{
			if (rectangle == listLine[i].rectangle)
			{
				listLine.Remove(listLine[i]);
				return;
			}
		}
	}

	void Move(Vector3 pos)
	{
		for (int i=0; i<listLine.Count; i++)
		{
			listLine[i].line.SetPosition(pos, listLine[i].rectangle.transformOverride.position, thisRectangle.game.canvas.localScale.x);
		}
	}

	void Link(Rectangle rectangle)
	{
		//проверим что такой связи еще нет. если есть ее требуется удалить
		for (int i=0; i<listLine.Count; i++)
		{
			if (rectangle == listLine[i].rectangle)
			{
				listLine[i].line.Remove();
				listLine.Remove(listLine[i]);
				rectangle.lineController.Remove(thisRectangle);
				return;
			}
		}
		//создаем
		Line line = null;
		if (LoadPrefab.LoadUIPrefab(ref line, _linePrefab, null, thisRectangle.game.parentLine))
		{
			line.SetPosition(thisRectangle.transformOverride.position, rectangle.transformOverride.position, thisRectangle.game.canvas.localScale.x);
			listLine.Add(new ConformityLine(rectangle, line));
			rectangle.lineController.listLine.Add(new ConformityLine(thisRectangle, line));
		}

	}

	[SerializeField] GameObject		_linePrefab;
	Rectangle						_thisRectangle;
}
