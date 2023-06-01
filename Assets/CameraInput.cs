
using UnityEngine;
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
using UnityEngine.InputSystem;
#endif

namespace StarterAssets{


    public class CameraInput : MonoBehaviour
    {
        public bool lockOn;
    #if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
    
        public void OnLockOn(InputValue value){
            LockOn(value.isPressed);
        }
    #endif

        public void LockOn(bool newLockOnState){
            lockOn = newLockOnState;
        }
    }
}