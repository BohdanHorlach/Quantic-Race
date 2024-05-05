using System;
using UnityEngine;


public class BotObstacleDetector : MonoBehaviour
{
    [SerializeField] private Transform _forwardRayPosition;
    [SerializeField] private Transform _leftSideRayPosition;
    [SerializeField] private Transform _rightSideRayPosition;
    [SerializeField] private LayerMask _obstacleMask;
    [SerializeField] private float _forwardRayDistance = 10;
    [SerializeField] private float _asideRayDistance = 4;
    [SerializeField] private int _rayCount = 6;
    [SerializeField] private float _searchForwardRaysAngle = 60f;
    [SerializeField] private float _searchAsideRaysAngle = 90f;


    public event Action<ObstacleScanerDataStruct> ScanerUpdate;
    public float ForwardRayDistance { get => _forwardRayDistance; }
    public float AsideRayDistance { get => _asideRayDistance; }


    private void FixedUpdate()
    {
        ObstacleScanerDataStruct scanerData;

        scanerData.forwardHitDistance = GetMinDistanceFromRay(_forwardRayPosition.position, transform.forward, _forwardRayDistance, _searchForwardRaysAngle, _rayCount);
        scanerData.leftHitDistance = GetMinDistanceFromRay(_leftSideRayPosition.position, -transform.right, _asideRayDistance, _searchAsideRaysAngle, _rayCount);
        scanerData.rightHitDistance = GetMinDistanceFromRay(_rightSideRayPosition.position, transform.right, _asideRayDistance, _searchAsideRaysAngle, _rayCount);
        scanerData.forwardHitAngle = GetAngleFromRay(_forwardRayPosition.position, transform.forward, _forwardRayDistance);

        int infinityRayCount = 3;
        scanerData.leftHitInfiniteDistance = GetMinDistanceFromRay(_leftSideRayPosition.position, -transform.right, Mathf.Infinity, _searchAsideRaysAngle, infinityRayCount);
        scanerData.rightHitInfiniteDistance = GetMinDistanceFromRay(_rightSideRayPosition.position, transform.right, Mathf.Infinity, _searchAsideRaysAngle, infinityRayCount);

        ScanerUpdate?.Invoke(scanerData);
    }



    private float GetDistanceFromRay(Vector3 position, Vector3 direction, float distance)
    {
        RaycastHit hit;
        bool isHit = Physics.Raycast(position, direction, out hit, distance, _obstacleMask);

        return isHit == true ? hit.distance : distance;
    }

    private float GetAngleFromRay(Vector3 position, Vector3 direction, float distance)
    {
        RaycastHit hit;
        if (Physics.Raycast(position, direction, out hit, distance, _obstacleMask))
        {

            Vector3 hitNormal = hit.normal;
            float angle = Vector3.Angle(hitNormal, direction);

            return angle;
        }
        else
        {
            return ObstacleScanerDataStruct.angleEmptyValue;
        }

    }



    private float GetMinDistanceFromRay(Vector3 position, Vector3 direction, float distance, float radius, int numberOfRays)
    {
        float minDistance = distance;
        float angleStep = radius / (numberOfRays - 1);

        for (int i = 0; i < numberOfRays; i++)
        {
            float angle = -radius / 2f + i * angleStep;
            Vector3 rayDirection = Quaternion.Euler(0, angle, 0) * direction;
            float rayDistance = GetDistanceFromRay(position, rayDirection, distance);

            if (rayDistance < minDistance)
                minDistance = rayDistance;
        }

        return minDistance == distance ? ObstacleScanerDataStruct.emptyValue : minDistance;
    }


    // visual for testing
    private void OnDrawGizmos()
    {
        DrawRay(_forwardRayPosition.position, transform.forward, _forwardRayDistance, _searchForwardRaysAngle, _rayCount);
        DrawRay(_leftSideRayPosition.position, -transform.right, _asideRayDistance, _searchAsideRaysAngle, _rayCount);
        DrawRay(_rightSideRayPosition.position, transform.right, _asideRayDistance, _searchAsideRaysAngle, _rayCount);
    }



    private void DrawRay(Vector3 position, Vector3 direction, float distance, float radius, int rayCount)
    {
        float angleStep = radius / (rayCount - 1);

        for (int i = 0; i < rayCount; i++)
        {
            float angle = -radius / 2f + i * angleStep;
            Vector3 rayDirection = Quaternion.Euler(0, angle, 0) * direction;

            Debug.DrawRay(position, rayDirection * distance, Color.red);
        }
    }
}