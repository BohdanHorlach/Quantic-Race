using UnityEngine;


public class TeleportMultiplayer : AbilityMultiplayer
{
    private const string CAN_NOT_TELEPORT_ANIMATION_TRIGER = "CantTeleport";
    private const string ACTIVATE_TELEPORT_ANIMATION_TRIGER = "Activate";

    [SerializeField] private Transform _car;
    [SerializeField] private Animator _animator;
    [SerializeField] private float _teleportDistance = 50f;
    [SerializeField] private float _freeSpaceRadius = 5f;
    [SerializeField] private LayerMask _obstacleMask;
    [SerializeField] private LayerMask _wallsMask;

    private Vector3 _telportPosition;
    private Vector3 _updatedTeleportPosition;
    private bool _canTeleported;
    private TypeAbility _type = TypeAbility.Teleport;

    public override TypeAbility Type { get => _type; }

    private void Update()
    {
        _updatedTeleportPosition = GetTeleportPosition();
        _canTeleported = CanTeleportToThePosition(_updatedTeleportPosition) && _updatedTeleportPosition != Vector3.zero;
    }


    private bool CanTeleportToThePosition(Vector3 position)
    {
        Collider[] obstacle = Physics.OverlapSphere(position, _freeSpaceRadius, _obstacleMask);

        return obstacle.Length == 0;
    }


    private Vector3 GetForwardPositionFromOffset(float offset)
    {
        BoxCollider carBoxCollider = _car.GetComponent<BoxCollider>();
        Vector3 carCenter = _car.transform.TransformPoint(carBoxCollider.center);
        return carCenter + _car.forward * offset;
    }

    // TODO prevent teleport away from the track(upwards over wall)
    // TODO MAYBE teleport to a smaller distance if max distance is blocked
    private Vector3 GetTeleportPosition()
    {
        BoxCollider carBoxCollider = _car.GetComponent<BoxCollider>();
        Vector3 center = _car.transform.TransformPoint(carBoxCollider.center);
        Vector3 sizeFromCenter = Vector3.Scale(carBoxCollider.size, carBoxCollider.transform.lossyScale) * 0.5f;
        float safeTeleportDistance = _teleportDistance + _freeSpaceRadius;

        if (Physics.BoxCastAll(center, sizeFromCenter, _car.forward, Quaternion.identity, safeTeleportDistance, _wallsMask).Length > 0)//IsWallForward())
        {
            return Vector3.zero;
        }
        else
        {
            return GetForwardPositionFromOffset(_teleportDistance);
        }
    }


    private void Teleporting()
    {
        _car.position = _telportPosition;
    }


    public override void Activate()
    {
        if (_canTeleported == false)
        {
            _animator.SetTrigger(CAN_NOT_TELEPORT_ANIMATION_TRIGER);
        }
        else
        {
            _animator.SetTrigger(ACTIVATE_TELEPORT_ANIMATION_TRIGER);
            _telportPosition = _updatedTeleportPosition;
        }
    }

    public override bool IsActive()
    {
        return false;
    }
}