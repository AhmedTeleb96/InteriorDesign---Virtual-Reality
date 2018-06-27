using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grab : MonoBehaviour
{
    public ViveHand Hand;

    private List<Rigidbody> attachedBodies;

    private Collider[] cols;

    // Use this for initialization
    void Start()
    {
        attachedBodies = new List<Rigidbody>();

        cols = new Collider[16];
    }

    // Update is called once per frame
    void Update()
    {
        if (ViveInput.GetButtonDown(Hand, ViveButton.Grip))
        {
            var count = Physics.OverlapSphereNonAlloc(transform.position, 0.04f, cols);
            for (int i = 0; i < count; i++)
            {
                var body = cols[i].GetComponent<Rigidbody>();
                if (body != null)
                {
                    attachedBodies.Add(body);
                    body.isKinematic = true;
                    body.transform.SetParent(transform);
                }
            }
        }

        if (ViveInput.GetButtonUp(Hand, ViveButton.Grip))
        {
            for(int i = 0; i < attachedBodies.Count; i++)
            {
                var body = attachedBodies[i];

                body.transform.SetParent(null);
                body.isKinematic = false;

               body.velocity = ViveInput.GetVelocity(Hand);
               body.angularVelocity = ViveInput.GetAngularVelocity(Hand);

                attachedBodies.RemoveAt(i);
            }
        }
    }
}
