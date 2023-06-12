
using UnityEngine;
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
using UnityEngine.InputSystem;
#endif

namespace StarterAssets{


    public class CameraInput : MonoBehaviour
    {
        public bool lockOn;

        public float changeEnemy;
    #if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
    
        public void OnLockOn(InputValue value){
            LockOn(value.isPressed);
        }

        public void OnChangeLockOn(InputValue value){
            //print("scrolling "+ value.Get<float>());
            ChangeLock(value.Get<float>());
        }
    #endif

        public void LockOn(bool newLockOnState){
            lockOn = newLockOnState;
        }

        public void ChangeLock(float nextEnemy){
            changeEnemy = nextEnemy;
        }
    }
}