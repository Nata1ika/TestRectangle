using UnityEngine;
using System.Collections;

public class LoadPrefab : MonoBehaviour 
{
	public static bool LoadUIPrefab<T> (ref T screenPartInstance, GameObject prefab, System.Action<T> onInit, Transform parent) where T: MonoBehaviour
	{
		if (prefab != null)
		{
			var spGO = (GameObject) GameObject.Instantiate(prefab);
			var screenPartComponent = spGO.GetComponent<T>();
			
			if (screenPartComponent != null)
			{
				screenPartInstance = screenPartComponent;
				spGO.transform.SetParent(parent, false);
				
				if (onInit != null)
				{
					onInit(screenPartComponent);
				}
				
				return true;
			}
			else
			{
				Debug.LogError (string.Format ("Prefab from {0} doesn't contain {1} component", prefab.name, typeof(T).Name));
			}
		}
		else
		{
			Debug.LogError (string.Format ("Can't load UIScreenPart prefab at path: {0}", prefab.name));
		}
		return false;
	}
}
