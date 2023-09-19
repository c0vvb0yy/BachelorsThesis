using UnityEngine;
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
using UnityEngine.InputSystem;
#endif

namespace StarterAssets
{
	public class StarterAssetsInputs : MonoBehaviour
	{
		[Header("Character Input Values")]
		public Vector2 move;
		public Vector2 look;
		public bool jump;
		public bool sprint;

		public bool attack;

		public bool drawWeapon;
		public bool sheathWeapon;

		public bool interact;

		[Header("Movement Settings")]
		public bool analogMovement;

		[Header("Mouse Cursor Settings")]
		public bool cursorLocked = true;
		public bool cursorInputForLook = true;

		[Header("Debug Helps")]
		public bool FarmQuest = false;

#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
		public void OnMove(InputValue value)
		{
			MoveInput(value.Get<Vector2>());
		}

		public void OnLook(InputValue value)
		{
			if(cursorInputForLook)
			{
				LookInput(value.Get<Vector2>());
			}
		}

		public void OnJump(InputValue value)
		{
			JumpInput(value.isPressed);
		}

		public void OnAttack(InputValue value)
		{
			AttackInput(value.isPressed);
		}

		public void OnDrawWeapon(InputValue value)
		{
			DrawWeaponInput(value.isPressed);
		}

		public void OnSheathWeapon(InputValue value)
		{
			SheathWeaponInput(value.isPressed);
		}
		public void OnSprint(InputValue value)
		{
			SprintInput(value.isPressed);
		}

		public void OnInteract(InputValue value){
			InteractInput(value.isPressed);
		}

		public void OnFinishFarmQuest(InputValue value){
			FinishFarmQuest(value.isPressed);
		}

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

		public void AttackInput(bool newAttackState)
		{
			attack = newAttackState;
		}
		public void DrawWeaponInput(bool newDrawWeaponState)
		{
			drawWeapon = newDrawWeaponState;
		}
		public void SheathWeaponInput(bool newSheathWeaponState)
		{
			sheathWeapon = newSheathWeaponState;
		}

		public void SprintInput(bool newSprintState)
		{
			sprint = newSprintState;
		}

		public void InteractInput(bool newInteractState)
		{
			interact = newInteractState;
		}

		private void OnApplicationFocus(bool hasFocus)
		{
			SetCursorState(cursorLocked);
		}

		private void SetCursorState(bool newState)
		{
			Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
		}

		private void FinishFarmQuest(bool finish){
			var questManager = GetQuestManager<EnemyQuestManager>();
			questManager.DebugFinishQuest(!questManager.questFulfilled);
			//questManager.questFulfilled = !questManager.questFulfilled;
			Debug.Log("Quest finished is: "+ questManager.questFulfilled);
		}

		private T GetQuestManager<T>(){
			var objects = GameObject.FindGameObjectsWithTag("QuestManager");
			foreach (var manager in objects){
				if(manager.TryGetComponent<T>(out T questmanager)){
					return questmanager;
				}
        	}
			Debug.LogWarning("Could not find the QuestManager for the starterInputs");
			return default(T);
		}
	}
	
}