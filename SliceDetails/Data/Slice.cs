using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SliceDetails.Data
{
	public readonly struct Slice
	{
		public readonly NoteCutInfo sliceCutInfo;
		public readonly Transform sliceParent;
		public readonly GameObject[] sliceMesh;

		public Slice(NoteCutInfo noteCutInfo, Transform parent)
		{
			sliceCutInfo = noteCutInfo;
			sliceParent = parent;
			sliceMesh = new GameObject[2];
		}

		public GameObject SliceMesh()
		{
			GameObject g = new();
			return g;
		}
	}
}
