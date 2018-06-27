using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Valve.VR;

public class ViveInput : MonoBehaviour
{
    private SteamVR_ControllerManager ctrlMgr;
    private SteamVR_TrackedObject leftObj, rightObj;
    private SteamVR_Controller.Device leftDevice, rightDevice;

    private static ViveInput inst;

    public static ViveInput GetInstance()
    {
        if (inst != null) return inst;

        inst = FindObjectOfType<ViveInput>();
        if (inst != null) return inst;

        var gObj = GameObject.Find("GlobalScripts");
        if (gObj == null)
        {
            gObj = new GameObject("GlobalScripts");
            gObj.transform.SetSiblingIndex(0);
        }

        inst = gObj.AddComponent<ViveInput>();
        return inst;
    }

    void Awake()
    {
        if (inst != null) DestroyImmediate(inst);
        inst = this;
    }

    // Use this for initialization
    void Start()
    {
        ctrlMgr = FindObjectOfType<SteamVR_ControllerManager>();
        if (ctrlMgr == null) return;

        leftObj = ctrlMgr.left.GetComponent<SteamVR_TrackedObject>();
        rightObj = ctrlMgr.right.GetComponent<SteamVR_TrackedObject>();
    }

    // Update is called once per frame
    void Update()
    {
        // Swap hands
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            var temp = leftObj;
            leftObj = rightObj;
            rightObj = temp;
        }

        if (leftObj != null && (int)leftObj.index != -1)
            leftDevice = SteamVR_Controller.Input((int)leftObj.index);
        if (rightObj != null && (int)rightObj.index != -1)
            rightDevice = SteamVR_Controller.Input((int)rightObj.index);
    }

    void OnDestroy()
    {
        inst = null;
    }

    public static bool IsValid(ViveHand hand)
    {
        GetInstance();

        if (hand == ViveHand.Unknown) return false;

        var dev = hand == ViveHand.Left ? inst.leftDevice : inst.rightDevice;
        if (dev == null) return false;

        return dev.valid;
    }

    public static bool IsAnyValid()
    {
        return IsValid(ViveHand.Left) || IsValid(ViveHand.Right);
    }

    public static bool IsAllValid()
    {
        return IsValid(ViveHand.Left) && IsValid(ViveHand.Right);
    }

    public static ViveHand GetHand(GameObject ctrlr)
    {
        GetInstance();

        if (inst.leftObj.gameObject == ctrlr)
            return ViveHand.Left;
        if (inst.rightObj.gameObject == ctrlr)
            return ViveHand.Right;

        if (inst.leftObj.transform.GetChild(0).gameObject == ctrlr)
            return ViveHand.Left;
        if (inst.rightObj.transform.GetChild(0).gameObject == ctrlr)
            return ViveHand.Right;

        return ViveHand.Unknown;
    }

    public static Transform GetTransform(ViveHand hand)
    {
        GetInstance();

        if (!IsValid(hand)) return inst.transform;

        var obj = hand == ViveHand.Left ? inst.leftObj : inst.rightObj;
        return obj.transform;
    }

    public static Vector3 GetPosition(ViveHand hand)
    {
        GetInstance();

        if (!IsValid(hand)) return Vector3.zero;

        var obj = hand == ViveHand.Left ? inst.leftObj : inst.rightObj;
        return obj.transform.position;
    }

    public static Quaternion GetRotation(ViveHand hand)
    {
        GetInstance();

        if (!IsValid(hand)) return Quaternion.identity;

        var obj = hand == ViveHand.Left ? inst.leftObj : inst.rightObj;
        return obj.transform.rotation;
    }

    public static Vector3 GetVelocity(ViveHand hand)
    {
        GetInstance();

        if (!IsValid(hand)) return Vector3.zero;

        var dev = hand == ViveHand.Left ? inst.leftDevice : inst.rightDevice;
        return dev.velocity;
    }

    public static Vector3 GetAngularVelocity(ViveHand hand)
    {
        GetInstance();

        if (!IsValid(hand)) return Vector3.zero;

        var dev = hand == ViveHand.Left ? inst.leftDevice : inst.rightDevice;
        return dev.angularVelocity;
    }

    public static Vector2 GetTouchPoint(ViveHand hand)
    {
        GetInstance();

        if (!IsValid(hand)) return Vector2.zero;

        var dev = hand == ViveHand.Left ? inst.leftDevice : inst.rightDevice;
        return dev.GetAxis(EVRButtonId.k_EButton_SteamVR_Touchpad);
    }

    public static float GetTriggerValue(ViveHand hand)
    {
        GetInstance();

        if (!IsValid(hand)) return 0f;

        var dev = hand == ViveHand.Left ? inst.leftDevice : inst.rightDevice;
        return dev.GetAxis(EVRButtonId.k_EButton_SteamVR_Trigger).x;
    }

    public static bool GetButtonState(ViveHand hand, ViveButton button)
    {
        GetInstance();

        if (!IsValid(hand)) return false;

        var dev = hand == ViveHand.Left ? inst.leftDevice : inst.rightDevice;
        return dev.GetPress((ulong)button);
    }

    public static bool GetButtonDown(ViveHand hand, ViveButton button)
    {
        GetInstance();

        if (!IsValid(hand)) return false;

        var dev = hand == ViveHand.Left ? inst.leftDevice : inst.rightDevice;
        return dev.GetPressDown((ulong)button);
    }

    public static bool GetButtonUp(ViveHand hand, ViveButton button)
    {
        GetInstance();

        if (!IsValid(hand)) return false;

        var dev = hand == ViveHand.Left ? inst.leftDevice : inst.rightDevice;
        return dev.GetPressUp((ulong)button);
    }

    public static void Vibrate(ViveHand hand, float strength, float duration)
    {
        GetInstance();

        if (!IsValid(hand)) return;

        var dev = hand == ViveHand.Left ? inst.leftDevice : inst.rightDevice;
        inst.StartCoroutine(VibrationCoroutine(dev, (ushort)(strength * 3999), duration));
        //dev.TriggerHapticPulse((ushort)(strength * 3999));
    }

    private static IEnumerator VibrationCoroutine(SteamVR_Controller.Device dev, ushort uSec, float duration)
    {
        while (duration > 0)
        {
            dev.TriggerHapticPulse(uSec);

            duration -= Time.deltaTime;

            yield return null;
        }
    }
}

public enum ViveHand : byte
{
    Unknown,
    Left,
    Right,
}

public enum ViveButton : ulong
{
    ApplicationMenu = 1ul << EVRButtonId.k_EButton_ApplicationMenu,
    Grip = 1ul << EVRButtonId.k_EButton_Grip,
    Touchpad = 1ul << EVRButtonId.k_EButton_SteamVR_Touchpad,
    Trigger = (1ul << EVRButtonId.k_EButton_SteamVR_Trigger),
}
