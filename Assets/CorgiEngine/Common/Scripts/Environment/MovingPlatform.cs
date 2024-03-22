using UnityEngine;
using System.Collections;
using MoreMountains.Tools;
using MoreMountains.Feedbacks;
using UnityEngine.Events;

// I MODIFIED THIS FILE SO THAT MM SCRIPT DOESN'T DEPEND ON PLAYER HAVING A CORGICONTROLLER COMPONENT
namespace MoreMountains.CorgiEngine
{
	/// <summary>
	/// Add this component to a platform and it'll be able to follow a path and carry a character
	/// </summary>
	[AddComponentMenu("Corgi Engine/Environment/Moving Platform")]
	public class MovingPlatform : MMPathMovement, Respawnable, MMEventListener<CorgiEngineEvent>
	{
		[Header("Activation")]
		[MMInformation("Check the <b>Only Moves When Player Is Colliding</b> checkbox to have the object wait for a collision with your player to start moving.",MoreMountains.Tools.MMInformationAttribute.InformationType.Info,false)]

		/// If true, the object will only move when colliding with the player
		[Tooltip("If true, the object will only move when colliding with the player")]
		public bool OnlyMovesWhenPlayerIsColliding = false;
		/// if true, the object will only move if a character is colliding
		[Tooltip("if true, the object will only move if a character is colliding")]
		public bool OnlyMovesWhenCharacterIsColliding = false;
		/// If true, this moving platform will reset position and behaviour when the player respawns
		[Tooltip("If true, this moving platform will reset position and behaviour when the player respawns")]
		public bool ResetPositionWhenPlayerRespawns = false;
		/// If true, this platform will only moved when commanded to by another script
		[Tooltip("If true, this platform will only moved when commanded to by another script")]
		public bool ScriptActivated = false;
		/// If true, the object will start moving when a player collides with it. This requires that ScriptActivated be set to true (and it will set it to true on init otherwise)
		[Tooltip("If true, the object will start moving when a player collides with it. This requires that ScriptActivated be set to true (and it will set it to true on init otherwise)")]
		public bool StartMovingWhenPlayerIsColliding = false;

		[MMInspectorButton("ToggleMovementAuthorization")]
		public bool ToggleButton;
		[MMInspectorButton("ChangeDirection")]
		public bool ChangeDirectionButton;
		[MMInspectorButton("ResetEndReached")]
		public bool ResetEndReachedButton;
		[MMInspectorButton("MoveTowardsStart")]
		public bool MoveTowardsStartButton;
		[MMInspectorButton("MoveTowardsEnd")]
		public bool MoveTowardsEndButton;

		[Header("Feedbacks")]

		/// a feedback to trigger when a new point is reached
		[Tooltip("a feedback to trigger when a new point is reached")]
		public MMFeedbacks PointReachedFeedback;
		/// a feedback to trigger when the end of the path is reached
		[Tooltip("a feedback to trigger when the end of the path is reached")]
		public MMFeedbacks EndReachedFeedback;

		[Header("Events")] 
		/// a UnityEvent fired when a character collides with the platform
		[Tooltip("a UnityEvent fired when a character collides with the platform")]
		public UnityEvent OnCharacterEnter;
		/// a UnityEvent fired when a character exits the platform
		[Tooltip("a UnityEvent fired when a character exits the platform")]
		public UnityEvent OnCharacterExit;

		protected Collider2D _collider2D;
		private Collider2D otherCollider;
		protected float _platformTopY;
		protected const float _toleranceY = 0.05f;
		protected bool _scriptActivatedAuthorization = false;
		protected Collider2D _colliderLastFrame;

		/// <summary>
		/// Flag inits, initial movement determination, and object positioning
		/// </summary>
		protected override void Initialization()
		{
			base.Initialization ();
			PointReachedFeedback?.Initialization(this.gameObject);
			EndReachedFeedback?.Initialization(this.gameObject);
			_collider2D = GetComponent<Collider2D> ();
			SetMovementAuthorization (false);
			if (StartMovingWhenPlayerIsColliding)
			{
				ScriptActivated = true;
			}
		}

		/// <summary>
		/// On Update, we check if we've started or stopped colliding with a controller, and trigger events if needed
		/// </summary>
		protected override void Update()
		{
			base.Update();

			if (otherCollider != _colliderLastFrame)
			{
				if (otherCollider != null)
				{
					OnCharacterEnter?.Invoke();
				}
				else
				{
					OnCharacterExit?.Invoke();
				}
			}

			_colliderLastFrame = otherCollider;
		}

		// My own method
		/// <summary>
		/// Gets a value indicating whether this instance can move.
		/// </summary>
		/// <value><c>true</c> if this instance can move; otherwise, <c>false</c>.</value>
		public override bool CanMove
		{
			get 
			{
				if (OnlyMovesWhenCharacterIsColliding)
				{
					if (!_collidingWithPlayer)
					{
						return false;
					}

					if (otherCollider == null)
					{
						return false;
					}
					// if we're colliding with a character, we check that's it's actually above the platform's top
					_platformTopY = (_collider2D != null) ? _collider2D.bounds.max.y : this.transform.position.y;
					if (otherCollider.bounds.min.y < _platformTopY - _toleranceY)
					{
						return false;
					}
                    
					if (OnlyMovesWhenPlayerIsColliding && otherCollider.tag != "Player")
					{
						return false;
					}
				}

				if (OnlyMovesWhenPlayerIsColliding && otherCollider == null)
				{
					return false;
				}

				if (ScriptActivated)
				{
					return _scriptActivatedAuthorization;
				}

				return true;
			}
		}

		public override void MoveAlongThePath()
		{
			base.MoveAlongThePath();
			Physics2D.SyncTransforms();
		}

		protected bool _collidingWithPlayer;

		/// <summary>
		/// Sets the movement authorization to true or false based on the status set in parameter
		/// </summary>
		/// <param name="status"></param>
		public virtual void SetMovementAuthorization(bool status)
		{
			_scriptActivatedAuthorization = status;
		}

		/// <summary>
		/// Sets the script authorization to true
		/// </summary>
		public virtual void AuthorizeMovement()
		{
			_scriptActivatedAuthorization = true;
		}

		/// <summary>
		/// Sets the script authorization to false
		/// </summary>
		public virtual void ForbidMovement()
		{
			_scriptActivatedAuthorization = false;
		}

		/// <summary>
		/// Sets the script authorization to true if it was false, false if it was true
		/// </summary>
		public virtual void ToggleMovementAuthorization()
		{
			_scriptActivatedAuthorization = !_scriptActivatedAuthorization;
		}

		/// <summary>
		/// Resets the end reached status, allowing you to move in the opposite direction if CycleOption is set to StopAtBounds
		/// </summary>
		public virtual void ResetEndReached()
		{
			_endReached = false;
		}

		/// <summary>
		/// Forces the moving platform to move back to its start position
		/// </summary>
		public virtual void MoveTowardsStart()
		{
			_endReached = false;
			_direction = -1;
			if (CurrentSpeed.magnitude > 0f)
			{
				_previousPoint = _currentPoint.Current;
				_currentPoint.MoveNext();
			}
			_scriptActivatedAuthorization = true;
		}

		/// <summary>
		/// Forces the moving platform to move to its end position
		/// </summary>
		public virtual void MoveTowardsEnd()
		{
			_endReached = false;
			_direction = 1;
			if (CurrentSpeed.magnitude > 0f)
			{
				_previousPoint = _currentPoint.Current;
				_currentPoint.MoveNext();
			}
			_scriptActivatedAuthorization = true;
		}

		protected override void PointReached()
		{
			base.PointReached();
			PointReachedFeedback?.PlayFeedbacks(this.transform.position);
		}

		protected override void EndReached()
		{
			base.EndReached();
			EndReachedFeedback?.PlayFeedbacks(this.transform.position);
		}

		// My own methods
		public virtual void OnCollisionEnter2D(Collision2D collision)
		{
			if (collision.collider.name == "Player"){
				otherCollider = collision.collider;
			}
			if (otherCollider==null)
				return;

			_collidingWithPlayer = true;	

			if (StartMovingWhenPlayerIsColliding)
			{
				AuthorizeMovement();
			}
		}

		public virtual void OnCollisionExit2D(Collision2D collision)
		{
			if (collision.collider.tag == "Player")
				otherCollider = null;

			_collidingWithPlayer=false;		
		}

		/// <summary>
		/// When the player respawns, we reset the position and behaviour of this moving platform
		/// </summary>
		/// <param name="checkpoint">Checkpoint.</param>
		/// <param name="player">Player.</param>
		public virtual void OnPlayerRespawn (CheckPoint checkpoint, Character player)
		{
			if (ResetPositionWhenPlayerRespawns)
			{
				Initialization ();	
			}
		}

		public virtual void OnPlayerRespawn ()
		{
			if (ResetPositionWhenPlayerRespawns)
			{
				Initialization ();	
			}
		}

		/// <summary>
		/// Catches respawn events and resets the platform's position if needed
		/// </summary>
		/// <param name="eventType"></param>
		public virtual void OnMMEvent(CorgiEngineEvent eventType)
		{
			if (eventType.EventType == CorgiEngineEventTypes.Respawn)
			{
				if (ResetPositionWhenPlayerRespawns)
				{
					Initialization();
				}
			}            
		}

		/// <summary>
		/// On enable, starts listening for CorgiEngine events
		/// </summary>
		protected virtual void OnEnable()
		{
			this.MMEventStartListening<CorgiEngineEvent>();
		}

		/// <summary>
		/// On disable, stops listening for CorgiEngine events
		/// </summary>
		protected virtual void OnDisable()
		{
			this.MMEventStopListening<CorgiEngineEvent>();
		}
	}
}