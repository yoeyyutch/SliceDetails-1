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
		public GameObject _sliceParent;
		public Transform _sliceContainer;
		public List<GameObject> _slices = new();


		private Dictionary<ISaberSwingRatingCounter, NoteInfo> _noteSwingInfos = new Dictionary<ISaberSwingRatingCounter, NoteInfo>();
		
		private List<NoteInfo> _noteInfos = new List<NoteInfo>();

		public SliceRecorder(BeatmapObjectManager beatmapObjectManager, SliceProcessor sliceProcessor) {
			_beatmapObjectManager = beatmapObjectManager;
			_sliceProcessor = sliceProcessor;
	
		}

		public void Initialize() {
			_beatmapObjectManager.noteWasCutEvent += OnNoteWasCut;
			_sliceProcessor.ResetProcessor();
			_slices.Clear();
			_sliceParent.transform.position = new(0, 0, 5f);
			_sliceParent.transform.rotation = Quaternion.identity;
			_sliceParent.SetActive(Plugin.Settings.ShowLiveView);
			
		}

		public void Dispose() {
			_beatmapObjectManager.noteWasCutEvent -= OnNoteWasCut;
			// Process slices once the map ends
			ProcessSlices();
			GameObject.Destroy(_sliceParent);
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
			float index = (int)(noteInfo.noteGridPosition.y * 4 + noteInfo.noteGridPosition.x -5.5f);

			GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
			go.transform.SetParent(_sliceParent.transform);
			//go.transform.SetPositionAndRotation(_sliceParent.transform.position, _sliceParent.transform.rotation);
			go.transform.SetLocalPositionAndRotation(noteInfo.cutInfo.cutPoint, Quaternion.Euler(0, 0, angle));
			//go.transform.localPosition = noteInfo.cutInfo.cutPoint;
			//go.transform.localScale = Mathf.Abs(index)>1f ? new(1f, .1f, .1f): Vector3.zero;
			go.transform.localScale = new(.5f, .005f, .005f);
			//go.transform.localRotation = Quaternion.Euler(0, 0, angle);
			Renderer r = go.GetComponent<Renderer>();
			r.sharedMaterial = Utils.ColorSchemeManager.SliceMaterial(noteInfo.noteData.colorType);
			go.SetActive(true);
			_slices.Add(go);

		}

	}
}
