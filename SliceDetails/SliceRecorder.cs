using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using SliceDetails.Data;
using SliceDetails.UI;
using SiraUtil.Logging;

namespace SliceDetails
{
	internal class SliceRecorder : IInitializable, IDisposable, ISaberSwingRatingCounterDidFinishReceiver
	{
		private readonly BeatmapObjectManager _beatmapObjectManager;
		private readonly SliceProcessor _sliceProcessor;
		public GameObject SliceMap;
		public List<GameObject> Slices = new();
		public List<Transform> Cutlist = new();
		public Material[] SliceMaterial;


		private Dictionary<ISaberSwingRatingCounter, NoteInfo> _noteSwingInfos = new Dictionary<ISaberSwingRatingCounter, NoteInfo>();
		
		private List<NoteInfo> _noteInfos = new List<NoteInfo>();

		public SliceRecorder(BeatmapObjectManager beatmapObjectManager, SliceProcessor sliceProcessor) {
			_beatmapObjectManager = beatmapObjectManager;
			_sliceProcessor = sliceProcessor;
	
		}

		public void Initialize() {
			_beatmapObjectManager.noteWasCutEvent += OnNoteWasCut;
			_sliceProcessor.ResetProcessor();
			SliceMap = new();
			SliceMap.transform.position = new(0, 0, Plugin.Settings.SliceDistance);
			SliceMap.transform.rotation = Quaternion.identity;
			SliceMap.SetActive(Plugin.Settings.ShowLiveView);
			Slices.Clear();
			SliceMaterial = new Material[2]
			{
				Utils.ColorSchemeManager.SliceMaterial(ColorType.ColorA),
				Utils.ColorSchemeManager.SliceMaterial(ColorType.ColorB),
			};
			Plugin.Log.Info(Plugin.Settings.PlayerHeight.ToString("0.###")+ " m");
		

		}

		public void Dispose() {
			_beatmapObjectManager.noteWasCutEvent -= OnNoteWasCut;
			// Process slices once the map ends
			ProcessSlices();
			GameObject.Destroy(SliceMap);
		}

		public void ProcessSlices() {
			_sliceProcessor.ProcessSlices(_noteInfos);
		}

		private void OnNoteWasCut(NoteController noteController, in NoteCutInfo noteCutInfo) {
			if (noteController.noteData.colorType == ColorType.None || !noteCutInfo.allIsOK) return;
			ProcessNote(noteController, noteCutInfo);
		}

		private void ProcessNote(NoteController noteController, NoteCutInfo noteCutInfo) {
			if (noteController == null) return;
			
			Vector2 noteGridPosition= new(noteController.noteData.lineIndex, (int)noteController.noteData.noteLineLayer);

			// No ME notes allowed >:(
			if (noteGridPosition.x >= 4 || noteGridPosition.y >= 3 || noteGridPosition.x < 0 || noteGridPosition.y < 0) return;

			NoteInfo noteInfo = new NoteInfo(noteController.noteData, noteCutInfo, noteController.transform.position, noteGridPosition);
			
			if(Plugin.Settings.ShowLiveView) DrawSlice(noteInfo);
			
			_noteSwingInfos.Add(noteCutInfo.swingRatingCounter, noteInfo);

			noteCutInfo.swingRatingCounter.UnregisterDidFinishReceiver(this);
			noteCutInfo.swingRatingCounter.RegisterDidFinishReceiver(this);
		}

		public void HandleSaberSwingRatingCounterDidFinish(ISaberSwingRatingCounter saberSwingRatingCounter) {
			NoteInfo noteSwingInfo;
			if (_noteSwingInfos.TryGetValue(saberSwingRatingCounter, out noteSwingInfo))
			{
				int preSwing, postSwing, offset;
				ScoreModel.RawScoreWithoutMultiplier(saberSwingRatingCounter, noteSwingInfo.cutInfo.cutDistanceToCenter, out preSwing, out postSwing, out offset);

				noteSwingInfo.score = new Score(preSwing, postSwing, offset);
				_noteInfos.Add(noteSwingInfo);
				_noteSwingInfos.Remove(saberSwingRatingCounter);
			}
			else {
				// Bad cut, do nothing
			}
		}

		public void DrawSlice(NoteInfo noteInfo)
		{
			float angle = noteInfo.noteData.cutDirection.RotationAngle() + noteInfo.cutInfo.cutDirDeviation+90f;
			//float index = (int)(noteInfo.noteGridPosition.y * 4 + noteInfo.noteGridPosition.x -5.5f);

			GameObject slash = GameObject.CreatePrimitive(PrimitiveType.Cube);
			slash.transform.SetParent(SliceMap.transform);
			slash.transform.SetPositionAndRotation(SliceMap.transform.position, SliceMap.transform.rotation);
			slash.transform.SetLocalPositionAndRotation(noteInfo.cutInfo.cutPoint, Quaternion.Euler(0, 0, angle));
			slash.transform.localScale = new(Plugin.Settings.SliceLength, Plugin.Settings.SliceWidth, Plugin.Settings.SliceWidth);
			slash.GetComponent<Renderer>().sharedMaterial =  SliceMaterial[(int)noteInfo.noteData.colorType];
			slash.SetActive(true);
			Slices.Add(slash);


			//go.transform.localPosition = noteInfo.cutInfo.cutPoint;
			//go.transform.localScale = Mathf.Abs(index)>1f ? new(1f, .1f, .1f): Vector3.zero;
			//go.transform.localRotation = Quaternion.Euler(0, 0, angle);
			//Renderer r = slash.GetComponent<Renderer>();
			//r.sharedMaterial = Utils.ColorSchemeManager.SliceMaterial(noteInfo.noteData.colorType);


		}

		//public void FadeSlices()
		//{
		//	foreach(GameObject g in Slices) { }
		//}

	}
}
