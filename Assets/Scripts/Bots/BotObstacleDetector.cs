using System;
using UnityEngine;


public class BotObstacleDetector : MonoBehaviour
{
    [SerializeField] private Transform _forwardRayPosition;
    [SerializeField] private Transform _leftSideRayPosition;
    [SerializeField] private Transform _rightSideRayPosition;
    [SerializeField] private LayerMask _obstacleMask;
    [SerializeField] private float _forwardRayDistance;
    [SerializeField] private float _asideRayDistance;
    [SerializeField] private int _rayCount = 4;
    [SerializeField] private float _searchForwardRadius = 60f;
    [SerializeField] private float _searchAsideRadius = 90f;
    
    
    public event Action<ObstacleScanerData> ScanerUpdate;
    public float ForwardRayDistance { get => _forwardRayDistance; }
    public float AsideRayDistance { get => _asideRayDistance; }
    public float LeftHitInfiniteDistance { get; private set; }
    public float RightHitInfiniteDistance { get; private set; }


    private void FixedUpdate()
    {
        ObstacleScanerData scanerData;

        scanerData.forwardHitDistance = GetMinDistanceFromRay(_forwardRayPosition.position, transform.forward, _forwardRayDistance, _searchForwardRadius, _rayCount);
        scanerData.leftHitDistance = GetMinDistanceFromRay(_leftSideRayPosition.position, -transform.right, _asideRayDistance, _searchAsideRadius, _rayCount);
        scanerData.rightHitDistance = GetMinDistanceFromRay(_rightSideRayPosition.position, transform.right, _asideRayDistance, _searchAsideRadius, _rayCount);

        LeftHitInfiniteDistance = GetMinDistanceFromRay(_leftSideRayPosition.position, -transform.right, Mathf.Infinity, _searchAsideRadius, 3);
        RightHitInfiniteDistance = GetMinDistanceFromRay(_rightSideRayPosition.position, transform.right, Mathf.Infinity, _searchAsideRadius, 3);

        ScanerUpdate?.Invoke(scanerData);
    }



    private void OnDrawGizmos()
    {
        DrawRay(_forwardRayPosition.position, transform.forward, _forwardRayDistance, _searchForwardRadius, _rayCount);
        DrawRay(_leftSideRayPosition.position, -transform.right, _asideRayDistance, _searchAsideRadius, _rayCount);
        DrawRay(_rightSideRayPosition.position, transform.right, _asideRayDistance, _searchAsideRadius, _rayCount);
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


    private float GetDistanceFromRay(Vector3 position, Vector3 direction, float distance)
    {
        RaycastHit hit;
        bool isHit = Physics.Raycast(position, direction, out hit, distance, _obstacleMask);

        return isHit == true ? hit.distance : distance;
    }



    private float GetMinDistanceFromRay(Vector3 position, Vector3 direction, float distance, float radius, int count)
    {
        float minDistance = distance;
        float angleStep = radius / (count - 1);

        for (int i = 0; i < count; i++)
        {
            float angle = -radius / 2f + i * angleStep;
            Vector3 rayDirection = Quaternion.Euler(0, angle, 0) * direction;
            float rayDistance = GetDistanceFromRay(position, rayDirection, distance);

            if (rayDistance < minDistance)
                minDistance = rayDistance;
        }

        return minDistance == distance ? ObstacleScanerData.emptyValue : minDistance;
    }
}