using UnityEngine;
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
using UnityEngine.InputSystem;
#endif

namespace StarterAssets
{
	public class StarterAssetsInputs : MonoBehaviour
	{
		//Singleton

		public static StarterAssetsInputs instance = null;

        private void Awake()
        {
			if (instance == null)
			{
				instance = this;
			}
			else if (instance != this)
			{
				Destroy(this);
			}
		}

        [Header("Character Input Values")]
		public Vector2 move;
		public Vector2 look;
		public bool jump;
		public bool sprint;
		public bool aim;
		public bool shoot;
		public bool reload;
		public bool interact;
		public bool dialogue;
		public bool questLog;
		public bool ability1;
		public bool ability2;
		public bool ability3;
		public bool ability4;

		[Header("Movement Settings")]
		public bool analogMovement;

#if !UNITY_IOS || !UNITY_ANDROID
		[Header("Mouse Cursor Settings")]
		public bool cursorLocked = true;
		public bool cursorInputForLook = true;
#endif

#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
		public void OnMove(InputAction.CallbackContext value)
		{
			MoveInput(value.ReadValue<Vector2>());
		}

		public void OnLook(InputAction.CallbackContext value)
		{
			if(cursorInputForLook)
			{
				LookInput(value.ReadValue<Vector2>());
			}
		}

		public void OnJump(InputAction.CallbackContext value)
		{
			JumpInput(value.performed);
		}

		public void OnInteract(InputAction.CallbackContext value)
		{
			InteractInput(value.performed);
		}

		public void OnQuestLog(InputAction.CallbackContext value)
		{
			QuestLogInput(value.performed);
		}

		public void OnDialogue(InputAction.CallbackContext value)
        {
			DialogueInput(value.performed);
        }

		public void OnShoot(InputAction.CallbackContext value)
		{
			ShootInput(value.performed);
		}

		public void OnReload(InputAction.CallbackContext value)
		{
			ReloadInput(value.performed);
		}

		public void OnAbility1(InputAction.CallbackContext value)
		{
			Ability1Input(value.performed);
		}

		public void OnAbility2(InputAction.CallbackContext value)
		{
			Ability2Input(value.performed);
		}

		public void OnAbility3(InputAction.CallbackContext value)
		{
			Ability3Input(value.performed);
		}

		public void OnAbility4(InputAction.CallbackContext value)
		{
			Ability4Input(value.performed);
		}

		public void OnSprint(InputAction.CallbackContext value)
		{
			SprintInput(value.action.ReadValue<float>() == 1);
		}

		public void OnAim(InputAction.CallbackContext value)
		{
			AimInput(value.action.ReadValue<float>() == 1);
		}
#else
	// old input sys if we do decide to have it (most likely wont)...
#endif


		public void MoveInput(Vector2 newMoveDirection)
		{
			move = newMoveDirection;
		} 

		public void LookInput(Vector2 newLookDirection)
		{
			look = newLookDirection;
		}

		public void JumpInput(bool newJumpState)
		{
			jump = newJumpState;
		}

		public void InteractInput(bool newInteractState)
		{
			interact = newInteractState;
		}

		public void QuestLogInput(bool newQuestLogState)
		{
			questLog = newQuestLogState;
		}

		public void DialogueInput(bool newDialogueState)
		{
			dialogue = newDialogueState;
		}

		public void ShootInput(bool newShootState)
		{
			shoot = newShootState;
		}

		public void ReloadInput(bool newShootState)
		{
			reload = newShootState;
		}

		public void Ability1Input(bool newShootState)
		{
			ability1 = newShootState;
		}

		public void Ability2Input(bool newShootState)
		{
			ability2 = newShootState;
		}

		public void Ability3Input(bool newShootState)
		{
			ability3 = newShootState;
		}

		public void Ability4Input(bool newShootState)
		{
			ability4 = newShootState;
		}

		public void SprintInput(bool newSprintState)
		{
			sprint = newSprintState;
		}

		public void AimInput(bool newAimState)
		{
			aim = newAimState;
		}

#if !UNITY_IOS || !UNITY_ANDROID

		private void OnApplicationFocus(bool hasFocus)
		{
			SetCursorState(cursorLocked);
		}

		private void SetCursorState(bool newState)
		{
			Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
		}

#endif

	}
	
}