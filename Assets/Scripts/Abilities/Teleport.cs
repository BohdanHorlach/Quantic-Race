using UnityEngine;


public class Teleport : Abilitiy
{
    [SerializeField] private Transform _car;
    [SerializeField] private Animator _animator;
    [SerializeField] private float _teleportDistance = 50f;
    [SerializeField] private float _radiusOfSearchForFreeSpace = 5f;
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


    private void OnDrawGizmos()
    {
        Vector3 rayPosition = GetForwardPositionFromOffset(_radiusOfSearchForFreeSpace);
        Vector3 resultPos = GetForwardPositionFromOffset(_teleportDistance) + _car.forward * _radiusOfSearchForFreeSpace;


        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(resultPos, _radiusOfSearchForFreeSpace);
        Debug.DrawRay(rayPosition, _car.forward * _teleportDistance, Color.blue);
    }


    private bool CanTeleportToThePosition(Vector3 position)
    {
        Collider[] obstacle = Physics.OverlapSphere(position, _radiusOfSearchForFreeSpace, _obstacleMask);

        return obstacle.Length == 0; 
    }


    private Vector3 GetForwardPositionFromOffset(float offset)
    {
        return _car.position + _car.forward * offset + new Vector3(0, 1);
    }


    private Vector3 GetTeleportPosition()
    {
        Vector3 rayPosition = GetForwardPositionFromOffset(_radiusOfSearchForFreeSpace);

        if (Physics.Raycast(rayPosition, _car.forward, _teleportDistance, _wallsMask))
        {
            return Vector3.zero;
        }
        else
        {
            return GetForwardPositionFromOffset(_teleportDistance) + _car.forward * _radiusOfSearchForFreeSpace;
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
            _animator.SetTrigger("CantTeleport");
        }
        else {
            _animator.SetTrigger("Activate");
            _telportPosition = _updatedTeleportPosition;
        }
    }
}