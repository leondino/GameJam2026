using UnityEngine;

public class WalkObjective : MonoBehaviour
{
    [SerializeField]
    private float walkSpeed = 1f;
    private bool isInQueue = false;
    private bool queueComplete = false;
    private bool newQueueMember = true;
    private Rigidbody npcBody;
    private Transform targetPoint;
    private float stopDistance = 0.1f;

    private ActiveNPCManager NPCManager;
    private Animator animator;
    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        NPCManager = GameManager.Instance.activeNPCManager;
        npcBody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        WalkToPoint(NPCManager.firstWalkPoint);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (targetPoint == null || npcBody == null) return;

        // Move at a constant speed towards the target (avoid slowing near the target)
        Vector3 currentPos = npcBody.position;
        Vector3 targetPos = targetPoint.position;
        float step = walkSpeed * Time.fixedDeltaTime;
        Vector3 newPos = Vector3.MoveTowards(currentPos, targetPos, step);
        npcBody.MovePosition(newPos);

        // Smooth rotation to face the target using Slerp for natural turning
        Vector3 toTarget = (targetPos - currentPos);
        toTarget.y = 0f; // keep rotation on the Y axis only
        if (toTarget.sqrMagnitude > 0.0001f)
        {
            Quaternion targetRot = Quaternion.LookRotation(toTarget.normalized);
            Quaternion newRot = Quaternion.Slerp(npcBody.rotation, targetRot, walkSpeed*2.5f * Time.fixedDeltaTime);
            npcBody.MoveRotation(newRot);
        }

        // Stop when close enough
        if (Vector3.Distance(newPos, targetPos) <= stopDistance)
        {
            if (newQueueMember)
            {
                WalkToPoint(NPCManager.queuePoints[0]);
                newQueueMember = false;
            }
            else
            {
                if (!isInQueue) 
                { 
                    isInQueue = true;
                    NPCManager.queueNPCs.Enqueue(gameObject);
                    npcBody.transform.LookAt(NPCManager.queuePoints[4]);
                }
                targetPoint = null;
                animator.SetBool("isWalking", false);
            }
        }
    }

    private void WalkToPoint(Transform point)
    {
        // Start moving towards the given point; movement and rotation are handled in FixedUpdate
        targetPoint = point;
        animator.SetBool("isWalking", true);
    }
}
