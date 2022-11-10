using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Android;
using TMPro;
using UnityEngine.UI;

public class KeyBoardOrPadController : MonoBehaviour
{
    public TextMeshProUGUI GamePadIsAdd;
    MyInput_Manager gamePadMobile;
    Mobile_Commands virtualPadMobile;
    GraphicRaycaster m_Raycaster;
    public static bool Key_Space;
    public static bool Key_Enter;
    public static bool Key_Shift;
    public static bool Key_Esc;
    public static bool Key_W;
    public static bool Key_A;
    public static bool Key_S;
    public static bool Key_D;
    public static bool Key_E;
    public static bool Key_E_Released;
    public static bool Key_Up;
    public static bool Key_Left;
    public static bool Key_Right;
    public static bool Key_Down;

    bool padIsAdded;
    
    private void Awake()
    {
        gamePadMobile = new MyInput_Manager();    
    }

    void SetKeyBoardKey()
    {
        if(SystemInfo.deviceType == DeviceType.Handheld) //TODO: rimettere ==
        {
            virtualPadMobile.EnableObj();
            Key_Space = virtualPadMobile.a_Button.Pressed;
            Key_Enter = virtualPadMobile.start_Button.Pressed;
            Key_Shift = virtualPadMobile.r2_Button.Pressed;
            Key_Esc = virtualPadMobile.b_Button.Pressed;
            Key_E = virtualPadMobile.l2_Button.Pressed;
            Key_E_Released = !virtualPadMobile.l2_Button.Pressed;
            Key_Up = virtualPadMobile.dPad_Up_Button.Pressed;
            Key_Left = virtualPadMobile.dPad_Left_Button.Pressed;
            Key_Right = virtualPadMobile.dPad_Right_Button.Pressed;
            Key_Down = virtualPadMobile.dPad_Down_Button.Pressed;

            Key_W = virtualPadMobile.dPad_Up_Button.Pressed;
            Key_A = virtualPadMobile.dPad_Left_Button.Pressed;
            Key_S = virtualPadMobile.dPad_Down_Button.Pressed;
            Key_D = virtualPadMobile.dPad_Right_Button.Pressed;
        }
        else
        {
            Key_Space = Keyboard.current.spaceKey.isPressed;
            Key_Enter = Keyboard.current.enterKey.isPressed;
            Key_Shift = Keyboard.current.shiftKey.isPressed;
            Key_Esc = Keyboard.current.escapeKey.isPressed;
            Key_W = Keyboard.current.wKey.isPressed;
            Key_A = Keyboard.current.aKey.isPressed;
            Key_S = Keyboard.current.sKey.isPressed;
            Key_D = Keyboard.current.dKey.isPressed;
            Key_E = Keyboard.current.eKey.isPressed;
            Key_E_Released = Keyboard.current.eKey.wasReleasedThisFrame;
            Key_Up = Keyboard.current.upArrowKey.isPressed;
            Key_Left = Keyboard.current.leftArrowKey.isPressed;
            Key_Right = Keyboard.current.rightArrowKey.isPressed;
            Key_Down = Keyboard.current.downArrowKey.isPressed;
        }
    }

    void SetPadKey()
    {
        #region TRY ANDROID GAMEPAD
        /*if (SystemInfo.deviceType == DeviceType.Handheld) //TODO: inserire un pad compatibile con lo smartphone
        {
            virtualPadMobile.DisableObj();
            /*gamePadMobile.AndroidInput.SPACE_A.performed += ctx => Key_Space = ctx.ReadValueAsButton();
            gamePadMobile.AndroidInput.START_ENTER.performed += ctx => Key_Enter = ctx.ReadValueAsButton();
            gamePadMobile.AndroidInput.SHIFT_R2.performed += ctx => Key_Shift = ctx.ReadValueAsButton();
            gamePadMobile.AndroidInput.ESC_B.performed += ctx => Key_Esc = ctx.ReadValueAsButton();
            
            gamePadMobile.AndroidInput.W_MOVE_UP.performed += ctx => Key_W = ctx.ReadValueAsButton();
            gamePadMobile.AndroidInput.A_MOVE_LEFT.performed += ctx => Key_A = ctx.ReadValueAsButton();
            gamePadMobile.AndroidInput.S_MOVE_DOWN.performed += ctx => Key_S = ctx.ReadValueAsButton();
            gamePadMobile.AndroidInput.D_MOVE_RIGHT.performed += ctx => Key_D = ctx.ReadValueAsButton();


            gamePadMobile.AndroidInput.E_L2.performed += ctx => Key_E = ctx.ReadValueAsButton();
            gamePadMobile.AndroidInput.E_L2.performed += ctx => Key_E_Released = !ctx.ReadValueAsButton();

            gamePadMobile.AndroidInput.D_PADUP.performed += ctx => Key_Up = ctx.ReadValueAsButton();
            gamePadMobile.AndroidInput.D_PADDOWN.performed += ctx => Key_Down = ctx.ReadValueAsButton();
            gamePadMobile.AndroidInput.D_PADLEFT.performed += ctx => Key_Left = ctx.ReadValueAsButton();
            gamePadMobile.AndroidInput.D_PADRIGHT.performed += ctx => Key_Right = ctx.ReadValueAsButton();


            Key_Space = Gamepad.current.buttonSouth.isPressed;

            Key_Enter = Gamepad.current.startButton.isPressed;
        }
        else
        {
            Key_Space = Gamepad.current.aButton.isPressed;
            Key_Enter = Gamepad.current.startButton.isPressed;
            Key_Shift = Gamepad.current.leftTrigger.isPressed;
            Key_Esc = Gamepad.current.bButton.isPressed;
            Key_W = Gamepad.current.leftStick.y.ReadValue() == 1;
            Key_A = Gamepad.current.leftStick.x.ReadValue() == -1;
            Key_S = Gamepad.current.leftStick.y.ReadValue() == -1;
            Key_D = Gamepad.current.leftStick.x.ReadValue() == 1;
            Key_E = Gamepad.current.rightTrigger.isPressed;
            Key_E_Released = Gamepad.current.rightTrigger.wasReleasedThisFrame;
            Key_Up = Gamepad.current.dpad.up.isPressed;
            Key_Left = Gamepad.current.dpad.left.isPressed;
            Key_Right = Gamepad.current.dpad.right.isPressed;
            Key_Down = Gamepad.current.dpad.down.isPressed;
        }*/
        #endregion

        if (SystemInfo.deviceType == DeviceType.Handheld)
            virtualPadMobile.DisableObj();

        Key_Space = Gamepad.current.aButton.isPressed;
        Key_Enter = Gamepad.current.startButton.isPressed;
        Key_Shift = Gamepad.current.leftTrigger.isPressed;
        Key_Esc = Gamepad.current.bButton.isPressed;
        Key_W = Gamepad.current.leftStick.y.ReadValue() == 1;
        Key_A = Gamepad.current.leftStick.x.ReadValue() == -1;
        Key_S = Gamepad.current.leftStick.y.ReadValue() == -1;
        Key_D = Gamepad.current.leftStick.x.ReadValue() == 1;
        Key_E = Gamepad.current.rightTrigger.isPressed;
        Key_E_Released = Gamepad.current.rightTrigger.wasReleasedThisFrame;
        Key_Up = Gamepad.current.dpad.up.isPressed;
        Key_Left = Gamepad.current.dpad.left.isPressed;
        Key_Right = Gamepad.current.dpad.right.isPressed;
        Key_Down = Gamepad.current.dpad.down.isPressed;
    }

    private void Start()
    {
        virtualPadMobile = GameObject.FindGameObjectWithTag("MobileCommands").GetComponent<Mobile_Commands>();
        Debug.Log($"PADMOBILE = {virtualPadMobile}");

        //m_Raycaster = GameObject.Find("UI_Draw").GetComponent<GraphicRaycaster>();
    }

    // Update is called once per frame
    void Update()
    {
        padIsAdded = Gamepad.current != null;

        if(Gamepad.current != null && GamePadIsAdd != null)
            GamePadIsAdd.text = $"PadIsAdded = {padIsAdded} / GamePad = {AndroidGamepadWithDpadAxes.current} / PRESSED BUTTON = {AndroidGamepadWithDpadAxes.current.IsPressed()}";
       
        if (padIsAdded)
            SetPadKey();
        else
            SetKeyBoardKey();

        /*if (Mouse.current.leftButton.isPressed)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            
            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log(hit.transform.name);
            }

        }*/

        if (Key_W)
        {
            Debug.Log("PRESS");
        }

        //RAYCAST UI
       /* EventSystem m_EventSystem;
        m_EventSystem = m_Raycaster.gameObject.GetComponent<EventSystem>();
        PointerEventData m_pointer = new PointerEventData(m_EventSystem);
        m_pointer.position = Mouse.current.position.ReadValue();

        List<RaycastResult> results = new List<RaycastResult>();

        m_Raycaster.Raycast(m_pointer, results);
        
        foreach(RaycastResult result in results)
        {
            Debug.Log(result.gameObject.name);
        }*/
        

    }
}
