using System.Collections;
using TMPro;
using UnityEngine;
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
using UnityEngine.InputSystem;
#endif

/* Note: animations are called via the controller for both the character and capsule using animator null checks
 */

namespace StarterAssets
{
	[RequireComponent(typeof(CharacterController))]
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
	[RequireComponent(typeof(PlayerInput))]
#endif
	public class ThirdPersonController : MonoBehaviour
	{
		[Header("Player")]
		[Tooltip("Move speed of the character in m/s")]
		public float MoveSpeed = 2.0f;
		[Tooltip("Sprint speed of the character in m/s")]
		public float SprintSpeed = 5.335f;
		[Tooltip("How fast the character turns to face movement direction")]
		[Range(0.0f, 0.3f)]
		public float RotationSmoothTime = 0.12f;
		[Tooltip("Acceleration and deceleration")]
		public float SpeedChangeRate = 10.0f;
        [Tooltip("Camera Sensitivity")] 
        public float Sensitivity = 1f;

		[Space(10)]
		[Tooltip("The height the player can jump")]
		public float JumpHeight = 1.2f;
		[Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
		public float Gravity = -15.0f;

		[Space(10)]
		[Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
		public float JumpTimeout = 0.50f;
		[Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
		public float FallTimeout = 0.15f;

		[Header("Player Grounded")]
		[Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
		public bool Grounded = true;
		[Tooltip("Useful for rough ground")]
		public float GroundedOffset = -0.14f;
		[Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
		public float GroundedRadius = 0.28f;
		[Tooltip("What layers the character uses as ground")]
		public LayerMask GroundLayers;

		[Header("Cinemachine")]
		[Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
		public GameObject CinemachineCameraTarget;
		[Tooltip("How far in degrees can you move the camera up")]
		public float TopClamp = 70.0f;
		[Tooltip("How far in degrees can you move the camera down")]
		public float BottomClamp = -30.0f;
		[Tooltip("Additional degress to override the camera. Useful for fine tuning camera position when locked")]
		public float CameraAngleOverride = 0.0f;
		[Tooltip("For locking the camera position on all axis")]
		public bool LockCameraPosition = false;

		// cinemachine
		private float _cinemachineTargetYaw;
		private float _cinemachineTargetPitch;

		// player
		private float _speed;
		private float _animationBlend;
		private float _targetRotation = 0.0f;
		private float _rotationVelocity;
		private float _verticalVelocity;
		private float _terminalVelocity = 53.0f;

		// timeout deltatime
		private float _jumpTimeoutDelta;
		private float _fallTimeoutDelta;

		// animation IDs
		private int _animIDSpeed;
		private int _animIDGrounded;
		private int _animIDJump;
		private int _animIDFreeFall;
		private int _animIDMotionSpeed;

		private PlayerInput _playerInput;
		private Animator _animator;
		private CharacterController _controller;
		private StarterAssetsInputs _input;
		private GameObject _mainCamera;
        private bool _rotateOnMove = true;

		private const float _threshold = 0.01f;

		private bool _hasAnimator;

		private bool IsCurrentDeviceMouse => _playerInput.currentControlScheme == "KeyboardMouse";

		public KeyCode actionKey = KeyCode.E;
		public ButtonDoor buttonDoor;
		public Mirror mirror1;
		public Mirror mirror2;
		public Mirror mirror3;
		public Mirror mirror4;
		public Mirror mirror5;

        private Health health;

		public bool stone1;
		public bool stone2;
        public bool stone3;
        public bool stone4;
        public bool stone5;
        public bool stone6;

        [SerializeField] private GameObject stone1Visual;
        [SerializeField] private GameObject stone2Visual;
        [SerializeField] private GameObject stone3Visual;
        [SerializeField] private GameObject stone4Visual;
        [SerializeField] private GameObject stone5Visual;
        [SerializeField] private GameObject stone6Visual;

        [SerializeField] private GameObject stone1UI;
        [SerializeField] private GameObject stone2UI;
        [SerializeField] private GameObject stone3UI;
        [SerializeField] private GameObject stone4UI;
        [SerializeField] GameObject stone5UI;
        [SerializeField] GameObject stone6UI;


        public GameController gameController;

        [SerializeField] private TextMeshProUGUI instructionText;

        private SceneManager sceneManager;

		private void Awake()
		{
			health = GetComponent<Health>();
			// get a reference to our main camera
			if (_mainCamera == null)
			{
				_mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
			}
		}

		private void Start()
		{
			_hasAnimator = TryGetComponent(out _animator);
			_controller = GetComponent<CharacterController>();
			_input = GetComponent<StarterAssetsInputs>();
			_playerInput = GetComponent<PlayerInput>();

			AssignAnimationIDs();

			// reset our timeouts on start
			_jumpTimeoutDelta = JumpTimeout;
			_fallTimeoutDelta = FallTimeout;
		}

		private void Update()
		{
			_hasAnimator = TryGetComponent(out _animator);
			
			JumpAndGravity();
			GroundedCheck();
			Move();

            if (_input.escape)
            {
				sceneManager.MainMenu();
            }
		}

        private void LateUpdate()
		{
			CameraRotation();
		}

		private void AssignAnimationIDs()
		{
			_animIDSpeed = Animator.StringToHash("Speed");
			_animIDGrounded = Animator.StringToHash("Grounded");
			_animIDJump = Animator.StringToHash("Jump");
			_animIDFreeFall = Animator.StringToHash("FreeFall");
			_animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
		}

		private void GroundedCheck()
		{
			// set sphere position, with offset
			Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z);
			Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers, QueryTriggerInteraction.Ignore);

			// update animator if using character
			if (_hasAnimator)
			{
				_animator.SetBool(_animIDGrounded, Grounded);
			}
		}

		private void CameraRotation()
		{
			// if there is an input and camera position is not fixed
			if (_input.look.sqrMagnitude >= _threshold && !LockCameraPosition)
			{
				//Don't multiply mouse input by Time.deltaTime;
				float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;
				
				_cinemachineTargetYaw += _input.look.x * Sensitivity * deltaTimeMultiplier;
				_cinemachineTargetPitch += _input.look.y * Sensitivity * deltaTimeMultiplier;
			}

			// clamp our rotations so our values are limited 360 degrees
			_cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
			_cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

			// Cinemachine will follow this target
			CinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + CameraAngleOverride, _cinemachineTargetYaw, 0.0f);
		}

		private void Move()
		{
			// set target speed based on move speed, sprint speed and if sprint is pressed
			float targetSpeed = _input.sprint ? SprintSpeed : MoveSpeed;

			// a simplistic acceleration and deceleration designed to be easy to remove, replace, or iterate upon

			// note: Vector2's == operator uses approximation so is not floating point error prone, and is cheaper than magnitude
			// if there is no input, set the target speed to 0
			if (_input.move == Vector2.zero) targetSpeed = 0.0f;

			// a reference to the players current horizontal velocity
			float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;

			float speedOffset = 0.1f;
			float inputMagnitude = _input.analogMovement ? _input.move.magnitude : 1f;

			// accelerate or decelerate to target speed
			if (currentHorizontalSpeed < targetSpeed - speedOffset || currentHorizontalSpeed > targetSpeed + speedOffset)
			{
				// creates curved result rather than a linear one giving a more organic speed change
				// note T in Lerp is clamped, so we don't need to clamp our speed
				_speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude, Time.deltaTime * SpeedChangeRate);

				// round speed to 3 decimal places
				_speed = Mathf.Round(_speed * 1000f) / 1000f;
			}
			else
			{
				_speed = targetSpeed;
			}
			_animationBlend = Mathf.Lerp(_animationBlend, targetSpeed, Time.deltaTime * SpeedChangeRate);

			// normalise input direction
			Vector3 inputDirection = new Vector3(_input.move.x, 0.0f, _input.move.y).normalized;

			// note: Vector2's != operator uses approximation so is not floating point error prone, and is cheaper than magnitude
			// if there is a move input rotate player when the player is moving
			if (_input.move != Vector2.zero)
			{
				_targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + _mainCamera.transform.eulerAngles.y;
				float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity, RotationSmoothTime);

				// rotate to face input direction relative to camera position
                if (_rotateOnMove)
                {
                    transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
                }
            }


			Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;

			// move the player
			_controller.Move(targetDirection.normalized * (_speed * Time.deltaTime) + new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);

			// update animator if using character
			if (_hasAnimator)
			{
				_animator.SetFloat(_animIDSpeed, _animationBlend);
				_animator.SetFloat(_animIDMotionSpeed, inputMagnitude);
			} 
		}

		private void JumpAndGravity()
		{
			if (Grounded)
			{
				// reset the fall timeout timer
				_fallTimeoutDelta = FallTimeout;

				// update animator if using character
				if (_hasAnimator)
				{
					_animator.SetBool(_animIDJump, false);
					_animator.SetBool(_animIDFreeFall, false);
				}

				// stop our velocity dropping infinitely when grounded
				if (_verticalVelocity < 0.0f)
				{
					_verticalVelocity = -2f;
				}

				// Jump
				if (_input.jump && _jumpTimeoutDelta <= 0.0f)
				{
					// the square root of H * -2 * G = how much velocity needed to reach desired height
					_verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);

					// update animator if using character
					if (_hasAnimator)
					{
						_animator.SetBool(_animIDJump, true);
					}
				}

				// jump timeout
				if (_jumpTimeoutDelta >= 0.0f)
				{
					_jumpTimeoutDelta -= Time.deltaTime;
				}
			}
			else
			{
				// reset the jump timeout timer
				_jumpTimeoutDelta = JumpTimeout;

				// fall timeout
				if (_fallTimeoutDelta >= 0.0f)
				{
					_fallTimeoutDelta -= Time.deltaTime;
				}
				else
				{
					// update animator if using character
					if (_hasAnimator)
					{
						_animator.SetBool(_animIDFreeFall, true);
					}
				}

				// if we are not grounded, do not jump
				_input.jump = false;
			}

			// apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
			if (_verticalVelocity < _terminalVelocity)
			{
				_verticalVelocity += Gravity * Time.deltaTime;
			}
		}

		private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
		{
			if (lfAngle < -360f) lfAngle += 360f;
			if (lfAngle > 360f) lfAngle -= 360f;
			return Mathf.Clamp(lfAngle, lfMin, lfMax);
		}

		private void OnDrawGizmosSelected()
		{
			Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
			Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

			if (Grounded) Gizmos.color = transparentGreen;
			else Gizmos.color = transparentRed;
			
			// when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
			Gizmos.DrawSphere(new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z), GroundedRadius);
		}

        public void SetSensitivity(float newSensitivity)
        {
            Sensitivity = newSensitivity;
        }

        public void SetRotateOnMove(bool newRotateOnMove)
        {
            _rotateOnMove = newRotateOnMove;
        }

        private void OnTriggerEnter(Collider other)
        {
            
        }

        private void OnTriggerStay(Collider other)
        {
			switch (other.gameObject.name)
            {
                case "Button":
                    instructionText.gameObject.SetActive(true);
					if (_input.use)
					{
						Debug.Log("hi");
						buttonDoor.open();
						StartCoroutine(Timer());
						IEnumerator Timer()
						{
							yield return new WaitForSeconds(6f);
							buttonDoor.closed();
						}
					}
					break;
				case "Mirror 1":
                    instructionText.gameObject.SetActive(true);
					if (_input.use)
                    {
						print("Mirror 1");
						mirror1.rotate();
                    }
					break;
				case "Mirror 2":
                    instructionText.gameObject.SetActive(true);
					if (_input.use)
					{
						mirror2.rotate();
					}
					break;
				case "Mirror 3":
                    instructionText.gameObject.SetActive(true);
					if (_input.use)
					{
						mirror3.rotate();
					}
					break;
				case "Mirror 4":
                    instructionText.gameObject.SetActive(true);
					if (_input.use)
					{
						mirror4.rotate();
					}
					break;
				case "Mirror 5":
                    instructionText.gameObject.SetActive(true);
					if (_input.use)
					{
						mirror5.rotate();
					}
					break;
				case "Dog 1":
                    instructionText.gameObject.SetActive(true);
					if (_input.use)
                    {
						if(stone1 == true)
                        {
							gameController.statueActivated();
							stone1 = false;
                            stone1UI.gameObject.SetActive(false);
							stone1Visual.gameObject.SetActive(true);
                        }
                    }
					break;
				case "Dog 2":
                    instructionText.gameObject.SetActive(true);
					if (_input.use)
                    {
						if(stone2 == true)
                        {
							gameController.statueActivated();
							stone2 = false;
                            stone2UI.gameObject.SetActive(false);
                            stone2Visual.gameObject.SetActive(true);
						}
                    }
					break;
				case "Dog 3":
                    instructionText.gameObject.SetActive(true);
					if (_input.use)
					{
						if (stone3 == true)
						{
							gameController.statueActivated();
							stone3 = false;
                            stone3UI.gameObject.SetActive(false);
                            stone3Visual.gameObject.SetActive(true);
						}
					}
					break;
				case "Dog 4":
                    instructionText.gameObject.SetActive(true);
					if (_input.use)
					{
						if (stone4 == true)
						{
							gameController.statueActivated();
							stone4 = false;
                            stone4UI.gameObject.SetActive(false);
                            stone4Visual.gameObject.SetActive(true);
						}
					}
					break;
				case "Dog 5":
                    instructionText.gameObject.SetActive(true);
					if (_input.use)
					{
						if (stone5 == true)
						{
							gameController.statueActivated();
							stone5 = false;
                            stone5UI.gameObject.SetActive(false);
                            stone5Visual.gameObject.SetActive(true);
						}
					}
					break;
				case "Dog 6":
                    instructionText.gameObject.SetActive(true);
					if (_input.use)
					{
						if (stone6 == true)
						{
							gameController.statueActivated();
							stone6 = false;
                            stone6UI.gameObject.SetActive(false);
                            stone6Visual.gameObject.SetActive(true);
						}
					}
					break;
				case "Victory Trigger":
					{
						UnityEngine.SceneManagement.SceneManager.LoadScene(6);
					}
					break;
			}
        }

        private void OnTriggerExit(Collider other)
        {
            instructionText.gameObject.SetActive(false);
        }


        public void Hit()
        {
            health.ChangeHealth(false);
        }

        public void Heal()
        {
			health.ChangeHealth(true);
        }
    }
}