﻿using UnityEngine;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;
using MoreMountains.Tools;
using MoreMountains.MMInterface;

namespace MoreMountains.CorgiEngine
{
	/// <summary>
	/// Simple start screen class.
	/// </summary>
	public class StartScreen : CorgiMonoBehaviour
	{
		/// the level to load after the start screen
		[Tooltip("the level to load after the start screen")]
		public string NextLevel;
		/// the name of the loading screen to use to load NextLevel
		[Tooltip("the name of the loading screen to use to load NextLevel")]
		public string LoadingSceneName = "LoadingScreen";
		/// the delay after which the level should auto skip (if less than 1s, won't autoskip)
		[Tooltip("the delay after which the level should auto skip (if less than 1s, won't autoskip)")]
		public float AutoSkipDelay = 0f;

		[Header("Fades")]
		/// the duration of the fade in of the start screen, in seconds
		[Tooltip("the duration of the fade in of the start screen, in seconds")]
		public float FadeInDuration = 1f;
		/// the duration of the fade out of the start screen, in seconds
		[Tooltip("the duration of the fade out of the start screen, in seconds")]
		public float FadeOutDuration = 1f;
		/// the tween type this fade should happen on
		public MMTweenType Tween;

		[Header("Sound Settings Bindings")]

		/// the switch used to turn music on or off
		[Tooltip("the switch used to turn music on or off")]
		public MMSwitch MusicSwitch;
		/// the switch used to turn sfx on or off
		[Tooltip("the switch used to turn sfx on or off")]
		public MMSwitch SfxSwitch;
		[Tooltip("THIS IS CODE THAT WAS ADDED BY AMORIEM")]
		public GameObject PersistentObjects; // THIS IS CODE THAT WAS ADDED BY AMORIEM

		/// <summary>
		/// Initialization
		/// </summary>
		protected virtual void Awake()
		{	
			GUIManager.Instance.SetHUDActive (false);
			MMFadeOutEvent.Trigger(FadeInDuration, Tween);
			PersistentObjects.SetActive(false); // THIS IS CODE THAT WAS ADDED BY AMORIEM

			if (AutoSkipDelay > 1f)
			{
				FadeOutDuration = AutoSkipDelay;
				StartCoroutine (LoadFirstLevel ());
			}
		}

		/// <summary>
		/// On Start we initialize our switches
		/// </summary>
		protected async void Start()
		{
			await Task.Delay(1);
			
			if (MusicSwitch != null)
			{
				MusicSwitch.CurrentSwitchState = MMSoundManager.Instance.settingsSo.Settings.MusicOn ? MMSwitch.SwitchStates.Right : MMSwitch.SwitchStates.Left;
				MusicSwitch.InitializeState ();
			}

			if (SfxSwitch != null)
			{
				SfxSwitch.CurrentSwitchState = MMSoundManager.Instance.settingsSo.Settings.SfxOn ? MMSwitch.SwitchStates.Right : MMSwitch.SwitchStates.Left;
				SfxSwitch.InitializeState ();
			}
		}

		/// <summary>
		/// During update we simply wait for the user to press the "jump" button.
		/// </summary>
		protected virtual void Update()
		{
			if (!Input.GetButtonDown ("Player1_Jump"))
				return;
			
			ButtonPressed ();
		}

		/// <summary>
		/// What happens when the main button is pressed
		/// </summary>
		public virtual void ButtonPressed()
		{
			MMFadeInEvent.Trigger(FadeOutDuration, Tween, 0, true);
			// if the user presses the "Jump" button, we start the first level.
			StartCoroutine (LoadFirstLevel ());
		}

		/// <summary>
		/// Loads the next level.
		/// </summary>
		/// <returns>The first level.</returns>
		protected virtual IEnumerator LoadFirstLevel()
		{
			yield return new WaitForSeconds (FadeOutDuration);
			SceneManager.LoadScene(NextLevel);
			PersistentObjects.SetActive(true); // THIS IS CODE THAT WAS ADDED BY AMORIEM
			//MMSceneLoadingManager.LoadScene (NextLevel, LoadingSceneName);
		}
	}
}