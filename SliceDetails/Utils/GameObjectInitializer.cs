using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SliceDetails.Utils
{
	static public class GameObjectInitializer
	{
		static public GameObject Primative(Transform parent, Vector3 localPosition, Vector3 localRotation, Vector3 localScale, PrimitiveType type)
		{
			GameObject g = GameObject.CreatePrimitive(type);
			return g;
		}
	
	}
}
