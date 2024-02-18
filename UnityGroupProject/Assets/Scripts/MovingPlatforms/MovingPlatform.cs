using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] MovePointPath _movePointPath;
    [SerializeField] float _speed;

    private int _targetMovePointIndex;

    private Transform _previousMovePoint;
    private Transform _targetMovePoint;

    private float _timeToMovePoint;
    private float _elapsedTime;

    // Start is called before the first frame update
    void Start()
    {
        TargetNextMovePoint();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        MovePlatform();
    }

    void MovePlatform()
    {
        _elapsedTime += Time.deltaTime;

        float elapsedPerc = _elapsedTime / _timeToMovePoint;

        elapsedPerc = Mathf.SmoothStep(0, 1, elapsedPerc);

        transform.position = Vector3.Lerp(_previousMovePoint.position, _targetMovePoint.position, elapsedPerc);    // moves platform to next target
        transform.rotation = Quaternion.Lerp(_previousMovePoint.rotation, _targetMovePoint.rotation, elapsedPerc); // rotates platform if next targets local rotation

        if (elapsedPerc >= 1)
        {
            TargetNextMovePoint();
        }
    }

    void TargetNextMovePoint()
    {
        _previousMovePoint = _movePointPath.GetMovePoint(_targetMovePointIndex);
        _targetMovePointIndex = _movePointPath.GetNextMovePoint(_targetMovePointIndex);
        _targetMovePoint = _movePointPath.GetMovePoint(_targetMovePointIndex);

        _elapsedTime = 0;

        float distanceToMovePoint = Vector3.Distance(_previousMovePoint.position, _targetMovePoint.position);
        _timeToMovePoint = distanceToMovePoint / _speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        other.transform.SetParent(transform);
    }

    private void OnTriggerExit(Collider other)
    {
        other.transform.SetParent(null);
    }
}
