using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SliceDetails.Data
{
	internal class Tile
	{
		public List<SliceInfo>[] sliceList = new List<SliceInfo>[18];
		public float[] angleAverages = new float[18];
		public float[] offsetAverages = new float[18];
		public Score[] scoreAverages = new Score[18];
		public int[] tileNoteCounts = new int[18];
		public float scoreAverage = 0.0f;
		public bool atLeastOneNote = false;

		public void CalculateAverages() {
			//angleAverages = new float[18];
			//offsetAverages = new float[18];
			//scoreAverages = new Score[18];
			//noteCounts = new int[18];
			//scoreAverage = 0.0f;
			//atLeastOneNote = false;

			for (int i = 0; i < 18; i++) {
				scoreAverages[i] = new Score(0.0f, 0.0f, 0.0f);
			}

			int noteCount = 0;
			for (int i = 0; i < sliceList.Length; i++) {
				if (sliceList[i].Count > 0) {
					foreach (SliceInfo slice in sliceList[i]) {
						atLeastOneNote = true;

						angleAverages[i] += slice.cutInfo.cutDirDeviation;

						offsetAverages[i] += Vector3.Dot(slice.cutInfo.cutNormal, slice.cutInfo.cutPoint - slice.notePosition) < 0 ? slice.cutInfo.cutDistanceToCenter:-slice.cutInfo.cutDistanceToCenter;

						scoreAverages[i] += slice.score;
						scoreAverage += slice.score.TotalScore;
						noteCount++;
					}
					//angleXYAverages.x /= tileNoteInfos[i].Count;
					//angleXYAverages.y /= tileNoteInfos[i].Count;
					//angleAverages[i] = Mathf.Atan2(angleXYAverages.y, angleXYAverages.x) * 180f / Mathf.PI;
					tileNoteCounts[i] = sliceList[i].Count;
					angleAverages[i] /= sliceList[i].Count;
					offsetAverages[i] /= sliceList[i].Count;
					scoreAverages[i] /= sliceList[i].Count;
				}
			}
			scoreAverage /= noteCount;
		}
	}
}
