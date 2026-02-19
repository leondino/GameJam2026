using UnityEngine;

public class WalkObjective : MonoBehaviour
{
    [SerializeField]
    private float walkSpeed = 1f;
    private bool isInQueue = false;
    [HideInInspector]
    public bool queueComplete = false;
    [HideInInspector]
    public bool goesDancing = false;
    private bool newQueueMember = true;
    private Rigidbody npcBody;
    private Transform targetPoint;
    private float stopDistance = 0.1f;
    [HideInInspector]
    public Transform dancefloorSpot; // Assigned when sent to dancefloor, used to track which spot the NPC is occupying

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
                WalkToPoint(NPCManager.queuePoints[4]);
                newQueueMember = false;
            }
            else if (!isInQueue &! goesDancing) 
            { 
                isInQueue = true;
                NPCManager.AddToQueue(gameObject);
                npcBody.transform.LookAt(NPCManager.queuePoints[0]);
            }
            else if (goesDancing && isInQueue)
            {
                isInQueue = false;
                dancefloorSpot = NPCManager.GetAvailableDancefloorSpot();
                WalkToPoint(dancefloorSpot);
            }
            else
            {
                if (queueComplete & !goesDancing)
                {
                    animator.SetBool("isSearched", true);
                    GetComponent<SearchInformation>().GenerateContraband();
                }
                if (goesDancing)
                {
                    animator.SetBool("isDancing", true);
                    npcBody.useGravity = false; // Disable gravity to allow for smooth dancing movement without physics interference
                    transform.position = (new Vector3(npcBody.position.x, npcBody.position.y+0.2f, npcBody.position.z)); //ofset for dance animation to prevent clipping with the floor
                    // Give the dancing NPC a random Y rotation so they face a random direction on the dancefloor
                    float randomDanceY = UnityEngine.Random.Range(0f, 360f);
                    npcBody.MoveRotation(Quaternion.Euler(0f, randomDanceY, 0f));
                }

                targetPoint = null;
                animator.SetBool("isWalking", false);
            }
        }
    }

    public void WalkToPoint(Transform point)
    {
        // Start moving towards the given point; movement and rotation are handled in FixedUpdate
        targetPoint = point;
        animator.SetBool("isWalking", true);
        animator.SetBool("isSearched", false);
    }
}
