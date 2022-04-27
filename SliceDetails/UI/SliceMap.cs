using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SliceDetails.UI
{
	class SliceMap
	{
		private readonly GameObject SliceMapGO;

		public SliceMap()
		{
			SliceMapGO = new();
			SliceMapGO.transform.SetPositionAndRotation(Plugin.Settings.SliceMapPosition, Quaternion.Euler(Plugin.Settings.SliceMapRotation));
		}

		void NoteOutline(Vector3 position, float lineThickness)
		{
			GameObject note = new();

		}
	}
}








		//public Transform _sliceContainer;
		//public List<GameObject> _slices = new();

		//public void DrawSlice(int index, int notetype, Vector3 cutPosition, float scaleFactor, string shader, float missDistance, Vector3 cutNormal)
		//{
		//	float p = Mathf.Atan2(-1f * cutNormal.x, cutNormal.y) * Mathf.Rad2Deg;
		//	Quaternion dotRotation = Quaternion.Euler(90f, 0f, p);
		//	Vector3 dotScale = new Vector3(missDistance, scaleFactor, scaleFactor);
		//	_slices.Add(GameObject.CreatePrimitive)
		//	_slices.Insert(index, GameObject.CreatePrimitive(PrimitiveType.Cube));
		//	_slices[index].SetActive(true);
		//	_slices[index].transform.SetParent(_sliceContainer);
		//	_slices[index].transform.position = new Vector3(cutPosition.x, -.5f, cutPosition.y + 1f);
		//	_slices[index].transform.rotation = dotRotation;
		//	_slices[index].transform.localScale = dotScale;

		//	float r = notetype == 0 ? .75f : 0f;
		//	float b = notetype == 1 ? .75f : 0f;
		//	float a = 1 - Mathf.Clamp01(missDistance / 0.3f);
		//	Color gradient = new Color(r, 0f, b, 1f);
		//	Renderer renderer = _slices[index].GetComponent<Renderer>();
		//	renderer.material = SetMaterial(gradient, shader);
		//float p = Mathf.Atan2(-1f * cutNormal.x, cutNormal.y) * Mathf.Rad2Deg;
		//Quaternion dotRotation = Quaternion.Euler(90f, 0f, p);
		//Vector3 dotScale = new Vector3(missDistance, scaleFactor, scaleFactor);
		//float r = notetype == 0 ? .75f : 0f;
		//float b = notetype == 1 ? .75f : 0f;
		//float a = 1 - Mathf.Clamp01(missDistance / 0.3f);
		//Color gradient = new Color(r, 0f, b, 1f);
		//Renderer renderer = _slices[index].GetComponent<Renderer>();
		//renderer.material = SetMaterial(gradient, shader);

		//_slices.Add(GameObject.CreatePrimitive(PrimitiveType.Cube));
		//_slices.Insert(index, GameObject.CreatePrimitive(PrimitiveType.Cube));
		//_slices[index].SetActive(true);
		//_slices[index].transform.SetParent(_sliceContainer);
		//_slices[index].transform.position = new Vector3(cutPosition.x, -.5f, cutPosition.y + 1f);
		//_slices[index].transform.rotation = dotRotation;
		//_slices[index].transform.localScale = dotScale;
		//}
//	}
//}
