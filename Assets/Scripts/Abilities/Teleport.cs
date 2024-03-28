using UnityEngine;


public class Teleport : Abilities
{
    [SerializeField] private Transform _car;
    [SerializeField] private float _teleportDistance = 50f;
    [SerializeField] private float _radiusOfSearchForFreeSpace = 5f;
    [SerializeField] private LayerMask _obstacleMask;


    private Vector3 _telportPosition;
    private bool _canTeleported;


    private void Update()
    {
        _telportPosition = GetTeleportPosition();
        _canTeleported = _telportPosition != Vector3.zero;
    }


    private bool CanTeleportToThePosition(Vector3 position)
    {
        Collider[] obstacle = Physics.OverlapSphere(position, _radiusOfSearchForFreeSpace, _obstacleMask);

        return obstacle.Length != 0; 
    }


    private Vector3 GetTeleportPosition()
    {
        RaycastHit hit;
        Vector3 rayPosition = _car.position + new Vector3(0, 0, _radiusOfSearchForFreeSpace);
        
        if (Physics.Raycast(rayPosition, _car.forward, out hit, _teleportDistance, _obstacleMask))
        {
            return Vector3.zero;
        }
        else
        {
            Vector3 resultPos = _car.position + new Vector3(0, 0, _teleportDistance);

            if (CanTeleportToThePosition(resultPos))
                return resultPos;
            else
                return Vector3.zero;
        }
    }


    private void TeleportToPosition(Vector3 position)
    {
        _car.position = position;
    }


    protected override void Activate()
    {
        if (_canTeleported == true)
            TeleportToPosition(_telportPosition);
    }
}