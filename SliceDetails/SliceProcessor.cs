using SliceDetails.UI;
using SliceDetails.Data;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace SliceDetails
{
	internal class SliceProcessor
	{
		public Tile[] tiles = new Tile[12];
		public bool Ready { get; private set; } = false;

		public void ResetProcessor() {
			Ready = false;

			tiles = new Tile[12];

			// Create "tiles", basically allocate information about each position in the 4x3 note grid.
			for (int i = 0; i < 12; i++) {
				tiles[i] = new Tile();
				for (int j = 0; j < 18; j++) {
					tiles[i].sliceList[j] = new List<SliceInfo>();
				}
			}
		}

		public void ProcessSlices(List<SliceInfo> sliceInfo) {
			ResetProcessor();

			// Populate the tiles' note infos.  Each List<NoteInfo> in tileNoteInfos cooresponds to each direction/color combination (i.e. DownLeft/ColorA)
			// where elements 0-8 are ColorA notes and elements 9-17 are ColorB notes numbering from NoteCutDirection.Up (0) to NoteCutDirection.Any (8)
			foreach (SliceInfo slice in sliceInfo) {
				Vector2 gridPos = new Vector2((int)slice.noteData.noteLineLayer,slice.noteData.lineIndex);
				int noteDirection = (int)Enum.Parse(typeof(OrderedNoteCutDirection), slice.noteData.cutDirection.ToString());
				int noteColor = (int)slice.noteData.colorType;
				int tileNoteDataIndex = noteColor * 9 + noteDirection;
				int index = (int)((int)slice.noteData.noteLineLayer * 4 + slice.noteData.lineIndex);
				tiles[index].sliceList[tileNoteDataIndex].Add(slice);
			}

			// Calculate average angles and offsets
			for (int i = 0; i < 12; i++) {
				tiles[i].CalculateAverages();
			}

			Ready = true;
		}
	}
}
