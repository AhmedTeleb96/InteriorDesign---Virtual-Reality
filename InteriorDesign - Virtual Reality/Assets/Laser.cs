using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Laser : MonoBehaviour
{
    private LineRenderer line;

    // Use this for initialization
    void Start()
    {
        line = GetComponent<LineRenderer>();
      //  line.enabled = false;

    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        if (ViveInput.GetButtonState(ViveHand.Left, ViveButton.Trigger))
        {
            line.enabled = true;
            if (Physics.Raycast(transform.position, transform.forward, out hit))
                line.SetPosition(1, new Vector3(0f, 0f, hit.distance));
            else
                line.SetPosition(1, new Vector3(0f, 0f, 100f));

        }
        if (ViveInput.GetButtonUp(ViveHand.Left, ViveButton.Trigger) )
        {
            line.enabled = false;

            if (Physics.Raycast(transform.position, transform.forward, out hit))
            {
              var btn =  hit.collider.gameObject.GetComponent<Button>();
                if(btn)
                {
                    btn.onClick.Invoke();
                }

				var MeshRendererfound = hit.collider.gameObject.GetComponent<MeshRenderer> ();
				if (MeshRendererfound)
				{
					MeshRendererfound.material = Material_Singletone.Instance.SelectMaterial;
				}

            }
        }

    }
}
