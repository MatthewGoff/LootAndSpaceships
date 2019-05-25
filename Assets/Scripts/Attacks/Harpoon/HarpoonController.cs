using System.Collections;
using UnityEngine;

public class HarpoonController : MonoBehaviour
{
    private readonly float EXTENSION_SPEED = 2f;
    private readonly float RETRIEVAL_SPEED = 2f;
    private readonly float VERTICAL_LINK_LENGTH = 0.4f;
    private readonly float HORIZONTAL_LINK_LENGTH = 0.2f;
    private readonly float MINIMUM_DISTANCE_JOINT_LENGTH = 0.1f;

    private float Speed;
    private float Range;
    private float Duration;
    private Vector2 HookVelocity;
    private GameObject RootLink;
    private GameObject Hook;
    private HarpoonAttackManager Manager;
    private Spaceship Attacker;
    private DistanceJoint2D TotalDistanceJoint;
    private DistanceJoint2D RootDistanceJoint;
    private DistanceJoint2D HookDistanceJoint;
    private bool RootLinkIsVertical;
    private bool ExtendInput;
    private bool RetrieveInput;
    private float LockCountdown;
    private Rigidbody2D LockedRigidbody;

    public void Initialize(HarpoonAttackManager manager, Spaceship attacker, Vector2 position, Vector2 direction, Vector2 initialVelocity, float speed, float range, float duration)
    {
        Manager = manager;
        Attacker = attacker;
        Speed = speed;
        Range = range;
        Duration = duration;

        float angle = Vector2.SignedAngle(Vector2.right, direction);
        Hook = Instantiate(GeneralPrefabs.Instance.HarpoonHook, position + (MINIMUM_DISTANCE_JOINT_LENGTH * direction.normalized), Quaternion.Euler(0, 0, angle));
        Hook.transform.SetParent(transform);
        HookVelocity = Speed * direction.normalized + initialVelocity;
        Hook.GetComponent<Rigidbody2D>().velocity = HookVelocity;
        Hook.GetComponent<HarpoonHookController>().AssignManager(manager);
        RootLink = Hook;

        TotalDistanceJoint = Attacker.gameObject.AddComponent<DistanceJoint2D>();
        TotalDistanceJoint.connectedBody = Hook.GetComponent<Rigidbody2D>();
        TotalDistanceJoint.autoConfigureConnectedAnchor = false;
        TotalDistanceJoint.anchor = Vector2.zero;
        TotalDistanceJoint.connectedAnchor = Hook.GetComponent<HarpoonLinkData>().ConnectedAnchorOffset;
        TotalDistanceJoint.autoConfigureDistance = false;
        TotalDistanceJoint.distance = MINIMUM_DISTANCE_JOINT_LENGTH;
        TotalDistanceJoint.maxDistanceOnly = true;

        RootDistanceJoint = Attacker.gameObject.AddComponent<DistanceJoint2D>();
        RootDistanceJoint.connectedBody = Hook.GetComponent<Rigidbody2D>();
        RootDistanceJoint.autoConfigureConnectedAnchor = false;
        RootDistanceJoint.connectedAnchor = Hook.GetComponent<HarpoonLinkData>().ConnectedAnchorOffset;
        RootDistanceJoint.autoConfigureDistance = false;
        RootDistanceJoint.distance = MINIMUM_DISTANCE_JOINT_LENGTH;
        RootDistanceJoint.maxDistanceOnly = true;

        RootLinkIsVertical = false;
    }

    private void Update()
    {
        ExtendInput = Input.GetKey(KeyCode.Z);
        RetrieveInput = Input.GetKey(KeyCode.X);
        if (Input.GetKeyDown(KeyCode.C))
        {
            Manager.Retrieve();
        }
    }

    private void FixedUpdate()
    {
        if (Attacker == null)
        {
            Dispose();
        }
        else if (Manager.State == HarpoonState.Fireing)
        {
            if (TotalDistanceJoint.distance > Range)
            {
                Manager.Retrieve();
            }
            else
            {
                ModifyChainLength(Speed * Time.fixedDeltaTime);
                Hook.GetComponent<Rigidbody2D>().velocity = HookVelocity;
            }
        }
        else if (Manager.State == HarpoonState.Locked)
        {
            if (LockedRigidbody == null)
            {
                Manager.Retrieve();
                return;
            }

            LockCountdown -= Time.fixedDeltaTime;
            if (LockCountdown <= 0)
            {
                Manager.Retrieve();
                return;
            }

            if (ExtendInput)
            {
                ModifyChainLength(EXTENSION_SPEED * Time.fixedDeltaTime);
            }
            else if (RetrieveInput)
            {
                ModifyChainLength(-RETRIEVAL_SPEED * Time.fixedDeltaTime);
            }
        }
        else if (Manager.State == HarpoonState.Retrieving)
        {
            ModifyChainLength(-RETRIEVAL_SPEED * Time.fixedDeltaTime);
            if (TotalDistanceJoint.distance <= MINIMUM_DISTANCE_JOINT_LENGTH)
            {
                Manager.Expire();
                Dispose();
            }
        }
    }

    private void Dispose()
    {
        if (Attacker != null)
        {
            Destroy(Attacker.GetComponent<DistanceJoint2D>());
            Destroy(Attacker.GetComponent<DistanceJoint2D>());
        }
        Destroy(gameObject);
    }

    private void ModifyChainLength(float modification)
    {
        TotalDistanceJoint.distance += modification;
        RootDistanceJoint.distance += modification;
        if (modification > 0)
        {
            ExtendChain();
        }
        else if (modification < 0)
        {
            RetrieveChain();
        }
    }

    private void ExtendChain()
    {
        if (RootLinkIsVertical)
        {
            if (RootDistanceJoint.distance > HORIZONTAL_LINK_LENGTH + MINIMUM_DISTANCE_JOINT_LENGTH)
            {
                AddLink(GeneralPrefabs.Instance.HarpoonHorizontalLink);
                ExtendChain();
            }
        }
        else
        {
            if (RootDistanceJoint.distance > VERTICAL_LINK_LENGTH + MINIMUM_DISTANCE_JOINT_LENGTH)
            {
                AddLink(GeneralPrefabs.Instance.HarpoonVerticalLink);
                ExtendChain();
            }
        }
    }

    private void RetrieveChain()
    {
        while (RootDistanceJoint.distance < MINIMUM_DISTANCE_JOINT_LENGTH)
        {
            if (!RemoveLink())
            {
                return;
            }
        }
    }

    private void AddLink(GameObject prefab)
    {
        Vector2 direction = (RootLink.GetComponent<Rigidbody2D>().position - Attacker.Position).normalized;
        Quaternion quaternion = Quaternion.Euler(0, 0, RootLink.GetComponent<Rigidbody2D>().rotation);
        GameObject newLink = Instantiate(prefab, Vector2.zero, quaternion);
        float distance = RootLink.GetComponent<HarpoonLinkData>().ConnectedAnchorOffset.magnitude * RootLink.transform.localScale.x
            + newLink.GetComponent<HarpoonLinkData>().ConnectedAnchorOffset.magnitude * newLink.transform.localScale.x;
        Vector2 position = RootLink.GetComponent<Rigidbody2D>().position - (distance * direction);
        newLink.transform.position = position;
        newLink.transform.SetParent(transform);
        newLink.GetComponent<Rigidbody2D>().velocity = RootLink.GetComponent<Rigidbody2D>().velocity;

        HingeJoint2D hingeJoint = newLink.GetComponent<HingeJoint2D>();
        hingeJoint.connectedBody = RootLink.GetComponent<Rigidbody2D>();
        hingeJoint.autoConfigureConnectedAnchor = false;
        hingeJoint.connectedAnchor = RootLink.GetComponent<HarpoonLinkData>().ConnectedAnchorOffset;

        newLink.GetComponent<HarpoonLinkData>().NextLink = RootLink;
        RootLink = newLink;

        RootDistanceJoint.connectedBody = RootLink.GetComponent<Rigidbody2D>();
        RootDistanceJoint.connectedAnchor = RootLink.GetComponent<HarpoonLinkData>().ConnectedAnchorOffset;
        RootDistanceJoint.distance -= 2 * RootLink.GetComponent<HarpoonLinkData>().ConnectedAnchorOffset.magnitude * RootLink.transform.localScale.x;

        RootLinkIsVertical = !RootLinkIsVertical;
    }

    /// <summary>
    /// Attempt to remove a link from the harpoon chain. Returns true on
    /// success and false if there are no more links to remove.
    /// </summary>
    /// <returns></returns>
    private bool RemoveLink()
    {
        if (RootLink == Hook)
        {
            return false;
        }
        else
        {
            GameObject newRoot = RootLink.GetComponent<HarpoonLinkData>().NextLink;
            RootDistanceJoint.connectedBody = newRoot.GetComponent<Rigidbody2D>();
            RootDistanceJoint.connectedAnchor = newRoot.GetComponent<HarpoonLinkData>().ConnectedAnchorOffset;
            RootDistanceJoint.distance += 2 * RootLink.GetComponent<HarpoonLinkData>().ConnectedAnchorOffset.magnitude * RootLink.transform.localScale.x;

            Destroy(RootLink);
            RootLink = newRoot;

            RootLinkIsVertical = !RootLinkIsVertical;
            return true;
        }
    }

    public void Lock(Spaceship target)
    {
        LockedRigidbody = target.GetComponent<Rigidbody2D>();

        HookDistanceJoint = Hook.AddComponent<DistanceJoint2D>();
        HookDistanceJoint.connectedBody = LockedRigidbody;
        HookDistanceJoint.autoConfigureConnectedAnchor = false;
        HookDistanceJoint.anchor = Hook.GetComponent<HarpoonLinkData>().AnchorOffset;
        HookDistanceJoint.connectedAnchor = Vector2.zero;
        HookDistanceJoint.autoConfigureDistance = false;
        HookDistanceJoint.distance = MINIMUM_DISTANCE_JOINT_LENGTH;
        HookDistanceJoint.maxDistanceOnly = true;
        
        TotalDistanceJoint.connectedBody = LockedRigidbody;
        TotalDistanceJoint.anchor = Vector2.zero;
        TotalDistanceJoint.connectedAnchor = Vector2.zero;
        TotalDistanceJoint.distance += (Hook.GetComponent<HarpoonLinkData>().AnchorOffset - Hook.GetComponent<HarpoonLinkData>().ConnectedAnchorOffset).magnitude;

        LockCountdown = Duration;
    }

    public void Unlock()
    {
        Destroy(Hook.GetComponent<DistanceJoint2D>());
        TotalDistanceJoint.connectedBody = Hook.GetComponent<Rigidbody2D>();
        TotalDistanceJoint.anchor = Vector2.zero;
        TotalDistanceJoint.connectedAnchor = Hook.GetComponent<HarpoonLinkData>().AnchorOffset;
        TotalDistanceJoint.distance -= (Hook.GetComponent<HarpoonLinkData>().AnchorOffset - Hook.GetComponent<HarpoonLinkData>().ConnectedAnchorOffset).magnitude;

    }
}
