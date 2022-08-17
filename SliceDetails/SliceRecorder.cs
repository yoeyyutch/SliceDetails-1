using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using SliceDetails.Data;
using SliceDetails.UI;
using SiraUtil.Logging;
using System.Linq;

namespace SliceDetails
{
	internal class SliceRecorder : IInitializable, IDisposable
	{
		private readonly BeatmapObjectManager _beatmapObjectManager;
		private readonly ScoreController _scoreController;
		private readonly SliceProcessor _sliceProcessor;
		//public GameObject SliceMap;
		//public List<GameObject> Slices = new();
		//public List<Transform> Cutlist = new();
		//public Material[] SliceMaterial;

		//private static int sliceCount;


		private Dictionary<NoteData, SliceInfo> _sliceScore = new Dictionary<NoteData, SliceInfo>();

		private List<SliceInfo> _slices = new();

		//public static int SliceCount { get => sliceCount; set => sliceCount = value; }

		public SliceRecorder(BeatmapObjectManager beatmapObjectManager, ScoreController scoreController, SliceProcessor sliceProcessor)
		{
			_beatmapObjectManager = beatmapObjectManager;
			_scoreController = scoreController;
			_sliceProcessor = sliceProcessor;

		}

		public void Initialize()
		{
			_beatmapObjectManager.noteWasCutEvent += OnNoteWasCut;
			_scoreController.scoringForNoteFinishedEvent += ScoringForNoteFinishedHandler;
			_sliceProcessor.ResetProcessor();
			_sliceProcessor.CreateSliceMap();
			//SliceCount = 0;
			//Plugin.Log.Info(Plugin.Settings.PlayerHeight.ToString("0.###")+ " m");
		}

		public void Dispose()
		{
			_beatmapObjectManager.noteWasCutEvent -= OnNoteWasCut;
			_scoreController.scoringForNoteFinishedEvent -= ScoringForNoteFinishedHandler;
			// Process slices once the map ends
			ProcessSlices();
			GameObject.Destroy(_sliceProcessor.SliceMap);
		}

		public void ProcessSlices()
		{
			_sliceProcessor.ProcessSlices(_slices);
		}

		private void OnNoteWasCut(NoteController noteController, in NoteCutInfo noteCutInfo)
		{
			if (noteController == null
				|| noteCutInfo.noteData.colorType == ColorType.None
				|| !noteCutInfo.allIsOK
				// Verify note is on standard 4 x 3 grid
				|| !Enumerable.Range(0, 4).Contains(noteCutInfo.noteData.lineIndex)
				|| !Enumerable.Range(0, 3).Contains((int)noteCutInfo.noteData.noteLineLayer))
				return;
			ProcessNote(noteCutInfo);
		}

		private void ProcessNote(NoteCutInfo noteCutInfo)
		{
			SliceInfo s = new(noteCutInfo);
			_sliceScore.Add(s.noteData, s);
		}

		public void ScoringForNoteFinishedHandler(ScoringElement scoringElement)
		{
			SliceInfo slice;
			if (_sliceScore.TryGetValue(scoringElement.noteData, out slice))
			{

				GoodCutScoringElement goodCutScoringElement = (GoodCutScoringElement)scoringElement;
				IReadonlyCutScoreBuffer cutScoreBuffer = goodCutScoringElement.cutScoreBuffer;

				int preSwing = cutScoreBuffer.beforeCutScore;
				int postSwing = cutScoreBuffer.afterCutScore;
				int offset = cutScoreBuffer.centerDistanceCutScore;

				switch (goodCutScoringElement.noteData.scoringType)
				{
					case NoteData.ScoringType.Normal:
						slice.score = new(preSwing, postSwing, offset);
						_slices.Add(slice);
						break;

					case NoteData.ScoringType.SliderHead:
						slice.score = new(preSwing, null, offset);
						_slices.Add(slice);
						break;
					case NoteData.ScoringType.SliderTail:
						slice.score = new(null, postSwing, offset);
						_slices.Add(slice);
						break;
					case NoteData.ScoringType.BurstSliderHead:
						slice.score = new(preSwing, null, offset);
						_slices.Add(slice);
						break;
				}

				if (Plugin.Settings.SliceMappingEnabled)
				{
					_sliceProcessor.CreateSlice(slice.cutInfo);
				}

				_sliceScore.Remove(goodCutScoringElement.noteData);
			}

			else
			{
				// Bad cut, do nothing
			}

		}

		// Moved to SliceProcessor
		//
		//void CreateSliceMap()
		//{
		//	SliceMap = new();
		//	SliceMap.transform.position = Plugin.Settings.SliceMapPosition;
		//	SliceMap.transform.rotation = Quaternion.Euler(Plugin.Settings.SliceMapRotation);
		//	SliceMap.transform.localScale = Plugin.Settings.SliceMapScale * Vector3.one;
		//	SliceMap.SetActive(Plugin.Settings.ShowLiveView);
		//	Slices.Clear();
		//	SliceMaterial = new Material[2]
		//	{
		//Utils.ColorSchemeManager.SliceMaterial(ColorType.ColorA),
		//Utils.ColorSchemeManager.SliceMaterial(ColorType.ColorB),
		//	};
		//}

		//public void DrawSlice(NoteCutInfo cut)
		//{
		//	float angle = cut.noteData.cutDirection.RotationAngle() + cut.cutDirDeviation + 90f;
		//	//float index = (int)(noteInfo.noteGridPosition.y * 4 + noteInfo.noteGridPosition.x -5.5f);

		//	GameObject slash = GameObject.CreatePrimitive(PrimitiveType.Cube);
		//	slash.transform.SetParent(SliceMap.transform);
		//	slash.transform.SetPositionAndRotation(SliceMap.transform.position, SliceMap.transform.rotation);
		//	slash.transform.SetLocalPositionAndRotation(cut.cutPoint, Quaternion.Euler(0, 0, angle));
		//	slash.transform.localScale = new(Plugin.Settings.SliceLength, Plugin.Settings.SliceWidth, Plugin.Settings.SliceWidth);
		//	slash.GetComponent<Renderer>().sharedMaterial = SliceMaterial[(int)cut.noteData.colorType];
		//	slash.SetActive(true);
		//	Slices.Add(slash);
		//}
	}
}
