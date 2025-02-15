using UnityEngine;
using UnityEngine.U2D.IK; // Make sure you have this!

public class IKPuller : MonoBehaviour
{
    public Transform ikTarget;
    public float pullThreshold = 0.1f;
    public float pullSpeed = 2f;
    private float limbMaxLength;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Get Limb Solver component
        LimbSolver2D limbSolver = GetComponentInChildren<LimbSolver2D>();

        if (limbSolver != null)
        {
            limbMaxLength = 0;
            IKChain2D chain = limbSolver.GetChain(0); // Correct way to get the chain

            for (int i = 0; i < chain.transformCount - 1; i++)
            {
                Transform bone1 = chain.transforms[i];
                Transform bone2 = chain.transforms[i + 1];

                limbMaxLength += Vector2.Distance(bone1.position, bone2.position);
            }
        }
        else
        {
            Debug.LogError("LimbSolver2D not found on " + gameObject.name);
        }
    }
}