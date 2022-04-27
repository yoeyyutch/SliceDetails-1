using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SliceDetails.Data
{
	internal class SliceInfo
	{
		public NoteData noteData;
		public NoteCutInfo cutInfo;
		public Score score;
		public Vector3 notePosition;
		public Vector2 noteGridPosition;
		public int noteIndex;

		public SliceInfo() { 
			
		}

		public SliceInfo(NoteCutInfo cutInfo) {
			this.cutInfo = cutInfo;
			this.noteData = cutInfo.noteData;
			this.notePosition = cutInfo.notePosition;
			this.noteGridPosition = new(cutInfo.noteData.lineIndex, (int)cutInfo.noteData.noteLineLayer);
			this.noteIndex = (int)(noteGridPosition.y * 4 + noteGridPosition.x);
		}
	}
}
