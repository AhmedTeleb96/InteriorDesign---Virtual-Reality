using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class scrollList : MonoBehaviour
{

    public ScrollRect myScrollRect;
	public ScrollRect myScrollRect_matrial;

    private float scroll;


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        scroll = (ViveInput.GetTouchPoint(ViveHand.Right).y + 1) / 2;
        Debug.Log(scroll);
		if (scroll != 0.5) {
			myScrollRect.verticalNormalizedPosition = scroll;
			myScrollRect_matrial.verticalNormalizedPosition = scroll;
		}
    }
}
