using System.Collections;
using UnityEngine;

namespace Core.Manager
{
    // [Manager("Input","Core")]
    public class InputManager
    {
        private BaseInputController _controller;
        public BaseInputController Controller
        {
            get
            {
                if (OverrideController != null)
                {
                    return OverrideController;
                }
                return _controller;
            }
        }
        public BaseInputController OverrideController;
        private Coroutine _inputDetectCoroutine;
        public void Init()
        {
            _inputDetectCoroutine = CoroutineHelper.StartCoroutine(DetectInputDevice());
        }
        public Vector3 GetMoveDirection()
        {
            return Controller?.GetMoveDirection()??Vector3.zero; 
        }
        public bool GetAttack()
        {
            return Controller?.GetAttack()??false;
        }
        public Vector3 GetAimDirection()
        {
            return Controller?.GetAimDirection()??Vector3.zero;
        }
        public bool GetSwapWeapon1()
        {
            return Controller?.GetSwapWeapon1()??false;
        }
        public bool GetSwapWeapon2()
        {
            return Controller?.GetSwapWeapon2()??false;
        }
        public bool GetSwapWeapon3()
        {
            return Controller?.GetSwapWeapon3()??false;
        }

        private IEnumerator DetectInputDevice()
        {
            while (true)
            {
                if (Input.anyKey || Input.GetMouseButton(0) || Input.GetMouseButton(1) || Input.GetMouseButton(2))
                {
                    _controller = new KeyBoardInputController();
                }
                else if (Input.GetJoystickNames().Length > 0)
                {
                    _controller = new JoystickInputController();
                }
                yield return null;
            }
        }
    }


    public class JoystickInputController : BaseInputController
    {
        public override Vector3 GetMoveDirection()
        {
            throw new System.NotImplementedException();
        }

        public override bool GetAttack()
        {
            throw new System.NotImplementedException();
        }

        public override Vector3 GetAimDirection()
        {
            throw new System.NotImplementedException();
        }

        public override bool GetSwapWeapon1()
        {
            throw new System.NotImplementedException();
        }
        public override bool GetSwapWeapon2()
        {
            throw new System.NotImplementedException();
        }

        public override bool GetSwapWeapon3()
        {
            throw new System.NotImplementedException();
        }
    }

    public class KeyBoardInputController : BaseInputController
    {
        public override Vector3 GetMoveDirection()
        {
            var moveDir = Vector3.zero;
            if (Input.GetKey(KeyCode.W))
                moveDir += Vector3.up;
            if (Input.GetKey(KeyCode.S))
                moveDir += Vector3.down;
            if (Input.GetKey(KeyCode.D))
                moveDir += Vector3.right;
            if (Input.GetKey(KeyCode.A))
                moveDir += Vector3.left;
            return moveDir.normalized;
        }

        public override bool GetAttack()
        {
            if (Input.GetMouseButton(0))
                return true;
            return false;
        }

        public override Vector3 GetAimDirection()
        {
            return Vector3.zero;
        }
        
        public override bool GetSwapWeapon1()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
                return true;
            return false;
        }

        public override bool GetSwapWeapon2()
        {
            if (Input.GetKeyDown(KeyCode.Alpha2))
                return true;
            return false;
        }

        public override bool GetSwapWeapon3()
        {
            if (Input.GetKeyDown(KeyCode.Alpha3))
                return true;
            return false;
        }
    }

    public abstract class BaseInputController
    {
        public virtual void Init()
        {
            
        }

        public abstract Vector3 GetMoveDirection();
        public abstract bool GetAttack();
        public abstract Vector3 GetAimDirection();
        public abstract bool GetSwapWeapon1();
        public abstract bool GetSwapWeapon2();
        public abstract bool GetSwapWeapon3();
    }
}