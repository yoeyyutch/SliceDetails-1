using SliceDetails.UI;
using SliceDetails.Data;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace SliceDetails
{
	internal class SliceProcessor
	{
		public Tile[] tiles;
		public GameObject SliceMap;
		public List<GameObject> Slices = new();
		public List<GameObject> Notes = new();
		//public List<Transform> Cutlist = new();
		public Material[] SliceMaterial;
		public bool Ready { get; private set; } = false;

		public void ResetProcessor()
		{
			Ready = false;

			tiles = new Tile[12];

			// Create "tiles", basically allocate information about each position in the 4x3 note grid.
			for (int i = 0; i < 12; i++)
			{
				tiles[i] = new Tile();
				for (int j = 0; j < 18; j++)
				{
					tiles[i].sliceList[j] = new List<SliceInfo>();
				}
			}
		}

		public void CreateSliceMap()
		{
			SliceMap = new();
			SliceMap.SetActive(Plugin.Settings.SliceMappingEnabled);
			SliceMap.transform.position = Plugin.Settings.SliceMapPosition;
			SliceMap.transform.rotation = Quaternion.Euler(Plugin.Settings.SliceMapRotation);
			SliceMap.transform.localScale = Plugin.Settings.SliceMapScale * Vector3.one;
			Slices.Clear();
			SliceMaterial = new Material[3]
			{
				Utils.ColorSchemeManager.SliceMaterial(ColorType.ColorA),
				Utils.ColorSchemeManager.SliceMaterial(ColorType.ColorB),
				new(Shader.Find("Sprites/Default"))
			};
			SliceMaterial[2].color = new Color(0.75f, 0.75f, 0.75f, Plugin.Settings.NoteTransparency);
			CreateGrid();
		}

		public void CreateGrid()
		{
			if (!SliceMap.activeSelf || SliceMap == null) return;

			float yOffset = Mathf.Clamp((HarmonyPatches.PlayerHeightGrabber.PlayerHeight - 1.8f) * 0.5f, -0.2f, 0.6f);
			float[] noteX = {-0.9f,-0.3f,0.3f,0.9f,};
			float[] noteY = { 0.85f + yOffset, 1.4f + yOffset, 1.9f + yOffset };
			Vector3 scale= Vector3.one * (0.5f * Plugin.Settings.NoteScale);

			for (int y = 0; y < 3; y++)
			{
				for (int x = 0, i = 0; x < 4; x++, i++)
				{
					GameObject note = GameObject.CreatePrimitive(PrimitiveType.Cube);
					note.SetActive(Plugin.Settings.ShowLiveView);
					note.transform.SetParent(SliceMap.transform);
					note.transform.SetPositionAndRotation(SliceMap.transform.position, SliceMap.transform.rotation);
					Vector3 notePos = new Vector3(noteX[x], noteY[y], 0.5f);
					note.transform.localPosition = notePos;
					note.transform.localScale = scale;
					note.GetComponent<Renderer>().sharedMaterial = SliceMaterial[2];
					Notes.Add(note);
				}
			}
		}

		public void CreateSlice(NoteCutInfo cut)
		{
			if (!SliceMap.activeSelf || SliceMap == null) return;

			float angle = cut.noteData.cutDirection.RotationAngle() + cut.cutDirDeviation + 90f;
			//float index = (int)(noteInfo.noteGridPosition.y * 4 + noteInfo.noteGridPosition.x -5.5f);

			GameObject slash = GameObject.CreatePrimitive(PrimitiveType.Cube);
			slash.SetActive(Plugin.Settings.ShowLiveView);
			slash.transform.SetParent(SliceMap.transform);
			slash.transform.SetPositionAndRotation(SliceMap.transform.position, SliceMap.transform.rotation);
			Vector3 cutPos = new Vector3(cut.cutPoint.x, cut.cutPoint.y, cut.cutPoint.z * .25f);
			slash.transform.SetLocalPositionAndRotation(cutPos, Quaternion.Euler(0, 0, angle));
			slash.transform.localScale = new(Plugin.Settings.SliceLength, Plugin.Settings.SliceWidth, Plugin.Settings.SliceWidth);
			slash.GetComponent<Renderer>().sharedMaterial = SliceMaterial[(int)cut.noteData.colorType];
			Slices.Add(slash);
		}

		public void ProcessSlices(List<SliceInfo> sliceInfo)
		{
			ResetProcessor();

			// Populate the tiles' note infos.  Each List<NoteInfo> in tileNoteInfos cooresponds to each direction/color combination (i.e. DownLeft/ColorA)
			// where elements 0-8 are ColorA notes and elements 9-17 are ColorB notes numbering from NoteCutDirection.Up (0) to NoteCutDirection.Any (8)
			foreach (SliceInfo slice in sliceInfo)
			{
				//Vector2 gridPos = new Vector2((int)slice.noteData.noteLineLayer,slice.noteData.lineIndex);
				int noteDirection = (int)Enum.Parse(typeof(OrderedNoteCutDirection), slice.noteData.cutDirection.ToString());
				int noteColor = (int)slice.noteData.colorType;
				int tileNoteDataIndex = noteColor * 9 + noteDirection;
				tiles[slice.noteIndex].sliceList[tileNoteDataIndex].Add(slice);
			}

			// Calculate average angles and offsets
			for (int i = 0; i < 12; i++)
			{
				tiles[i].CalculateAverages();
			}

			Ready = true;
		}

		public void DrawSlices(bool set)
		{
			if (Slices.Count == 0)
				return;
			foreach (GameObject slice in Slices)
			{
				slice.SetActive(set);
			}
			foreach (GameObject note in Notes)
			{
				note.SetActive(set);
			}
		}
	}
}
