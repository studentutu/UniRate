﻿using UnityEngine;

namespace PWR.LowPowerMemoryConsumption {

	public class FrameRateTrigger : MonoBehaviour {

		#region <<---------- Properties and Fields ---------->>

		[SerializeField] private FrameRateType _type = FrameRateType.FPS;

		[SerializeField] private UnityEventInt _currentRateChangedEvent;

		[SerializeField] private UnityEventInt _targetRateChangedEvent;

		/// <summary>
		/// Rate type.
		/// </summary>
		public FrameRateType Type {
			get { return this._type; }
			set {
				if (this._type == value) return;
				this._type = value;
				this.OnTypeChanged();
			}
		}

		/// <summary>
        /// Event raised when current rate of <see cref="Type"/> changes.
        /// </summary>
		public UnityEventInt CurrentRateChangedEvent {
			get {
				if (this._currentRateChangedEvent == null) {
					this._currentRateChangedEvent = new UnityEventInt();
				}
				return this._currentRateChangedEvent;
			}
		}

		/// <summary>
        /// Event raised when target rate of <see cref="Type"/> changes.
        /// </summary>
		public UnityEventInt TargetRateChangedEvent {
			get {
				if (this._targetRateChangedEvent == null) {
					this._targetRateChangedEvent = new UnityEventInt();
				}
				return this._targetRateChangedEvent;
			}
		}

		private bool _isApplicationQuitting = false;

		#endregion <<---------- Properties and Fields ---------->>




		#region <<---------- MonoBehaviour ---------->>

		protected virtual void OnEnable() {
			this.NotifyAllRatesChanged();
			this.StartListening();
		}

		protected virtual void OnDisable() {
			if (this._isApplicationQuitting) return;
			this.StopListening();
		}

		protected virtual void OnApplicationQuit() {
            this._isApplicationQuitting = true;
        }

		#if UNITY_EDITOR
		protected virtual void OnValidate() {
			this.OnTypeChanged();
		}
		#endif

		#endregion <<---------- MonoBehaviour ---------->>




		#region <<---------- General ---------->>

		private void NotifyAllRatesChanged() {
			this.NotifyTargetFrameRateChanged();
			this.NotifyTargetFixedFrameRateChanged();
			this.NotifyCurrentFrameRateChanged();
			this.NotifyCurrentFixedFrameRateChanged();
		}

		protected virtual void OnTypeChanged() {
			if (!this.isActiveAndEnabled) return;
			this.NotifyAllRatesChanged();
		}

		protected void StartListening() {
			FrameRateManager.Instance.TargetFrameRateChanged += this.NotifyTargetFrameRateChanged;
			FrameRateManager.Instance.FrameRateChanged += this.NotifyCurrentFrameRateChanged;

			FrameRateManager.Instance.TargetFixedFrameRateChanged += this.NotifyTargetFixedFrameRateChanged;
			FrameRateManager.Instance.FixedFrameRateChanged += this.NotifyCurrentFixedFrameRateChanged;
		}

		protected void StopListening() {
			FrameRateManager.Instance.TargetFrameRateChanged -= this.NotifyTargetFrameRateChanged;
			FrameRateManager.Instance.FrameRateChanged -= this.NotifyCurrentFrameRateChanged;

			FrameRateManager.Instance.TargetFixedFrameRateChanged -= this.NotifyTargetFixedFrameRateChanged;
			FrameRateManager.Instance.FixedFrameRateChanged -= this.NotifyCurrentFixedFrameRateChanged;
		}

		protected void NotifyCurrentFrameRateChanged() {
			this.NotifyCurrentFrameRateChanged(FrameRateManager.Instance.FrameRate);
		}
		protected virtual void NotifyCurrentFrameRateChanged(int rate) {
			if (this._type != FrameRateType.FPS) return;
			if (this._currentRateChangedEvent != null) this._currentRateChangedEvent.Invoke(rate);
		}

		protected void NotifyTargetFrameRateChanged() {
			this.NotifyTargetFrameRateChanged(FrameRateManager.Instance.TargetFrameRate);
		}
		protected virtual void NotifyTargetFrameRateChanged(int rate) {
			if (this._type != FrameRateType.FPS) return;
			if (this._targetRateChangedEvent != null) this._targetRateChangedEvent.Invoke(rate);
		}

		protected void NotifyCurrentFixedFrameRateChanged() {
			this.NotifyCurrentFixedFrameRateChanged(FrameRateManager.Instance.FixedFrameRate);
		}
		protected virtual void NotifyCurrentFixedFrameRateChanged(int rate) {
			if (this._type != FrameRateType.FixedFPS) return;
			if (this._currentRateChangedEvent != null) this._currentRateChangedEvent.Invoke(rate);
		}

		protected void NotifyTargetFixedFrameRateChanged() {
			this.NotifyTargetFixedFrameRateChanged(FrameRateManager.Instance.TargetFixedFrameRate);
		}
		protected virtual void NotifyTargetFixedFrameRateChanged(int rate) {
			if (this._type != FrameRateType.FixedFPS) return;
			if (this._targetRateChangedEvent != null) this._targetRateChangedEvent.Invoke(rate);
		}

		#endregion <<---------- General ---------->>
	}
}