using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SliceDetails.Data
{
	internal class NoteInfo
	{
		public NoteData noteData;
		public NoteCutInfo cutInfo;
		public Score score;
		public Vector3 notePosition;
		public Vector2 noteGridPosition;
		public int noteIndex;

		public NoteInfo() { 
			
		}

		public NoteInfo(NoteData noteData, NoteCutInfo cutInfo, Vector3 notePosition, Vector2 noteGridPosition) {
			this.noteData = noteData;
			this.cutInfo = cutInfo;
			this.notePosition = notePosition;
			this.noteGridPosition = noteGridPosition;
		}
	}
}
