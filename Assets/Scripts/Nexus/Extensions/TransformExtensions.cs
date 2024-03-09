using System;
using UnityEngine;
using System.Collections.Generic;

namespace Assets.Scripts.ClientUtilities.Extensions
{
	public static class TransformExtensions
	{	
		public static Transform FindDeep(this Transform a, string Name, bool IncludeDisables = true)
		{
			if ((a.gameObject.activeInHierarchy || IncludeDisables) && a.name == Name)
				return a;

			for (int i = 0; i < a.childCount; i++)
			{
				Transform result = a.GetChild(i).FindDeep(Name, IncludeDisables);

				if (result != null)
					return result;
			}

			return null;
		}

		
		public static Transform[] FindDeepAll(this Transform a, string Name)
		{
			List<Transform> resultsList = new List<Transform>();

			if (a.name == Name)
				resultsList.Add(a);

			for (int i = 0; i < a.childCount; i++)
			{
				Transform[] resultArray = a.GetChild(i).FindDeepAll(Name);
				if (resultArray != null)
					resultsList.AddRange(resultArray);
			}

			if (resultsList.Count != 0)
				return resultsList.ToArray();

			return null;
		}

	
		public static Transform GetFirstActiveChild(this Transform a)
		{
			for (int i = 0; i < a.childCount; ++i)
			{
				Transform activeTransform = a.transform.GetChild(i);
				if (activeTransform.gameObject.activeSelf)
				{
					return activeTransform;
				}
			}
			return null;
		}
	}
}