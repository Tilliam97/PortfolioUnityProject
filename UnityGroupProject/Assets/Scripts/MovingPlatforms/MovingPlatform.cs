using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour, IToggle
{
    [SerializeField] MovePointPath _movePointPath;
    [SerializeField] bool onButton;
    [SerializeField] float _speed;

    bool _goToNext;

    private int _targetMovePointIndex;

    [SerializeField] Transform _startPoint;
    [SerializeField] Transform _platformPos;
    private Transform _previousMovePoint;
    private Transform _targetMovePoint;

    private float _timeToMovePoint;
    private float _elapsedTime;

    //bool _goToStart;

    // Start is called before the first frame update
    void Awake()
    {

        OnButtonCheck();
        TargetNextMovePoint();
    }

    // Update is called once per frame
    void FixedUpdate()  // changed to Fixed update to allow transformations of the platform movement to affect all children (this includes the player)
    {
        if (_goToNext)
        {
            MovePlatform();
        }
        //else if (_goToStart)
        //{
        //    MovePlatform();
        //}
    }
    
    void OnButtonCheck()
    {
        if (onButton)
            _goToNext = onButton;
        else
            _goToNext = true;
    }

    public void ToggleMe()
    {
        _goToNext = !_goToNext;
        onButton = !onButton;
    }

    void MovePlatform()
    {
        //Debug.Log("go to start is " + _goToStart + " go to next is " + _goToNext);
        _elapsedTime += Time.deltaTime;

        float elapsedPerc = _elapsedTime / _timeToMovePoint;  // time it takes to get from point A to Point B

        elapsedPerc = Mathf.SmoothStep(0, 1, elapsedPerc); // slows down when reaching a point  that has been set

        transform.position = Vector3.Lerp(_previousMovePoint.position, _targetMovePoint.position, elapsedPerc);    // moves platform to next target
        transform.rotation = Quaternion.Lerp(_previousMovePoint.rotation, _targetMovePoint.rotation, elapsedPerc); // rotates platform if next targets local rotation

        if (elapsedPerc >= 1)
        {
            TargetNextMovePoint(); 
        }
        //if (_goToStart)
       // {
        //    _goToStart = false;
        //}
    }

    void TargetNextMovePoint()
    {
        _previousMovePoint = _movePointPath.GetMovePoint(_targetMovePointIndex);
        _targetMovePointIndex = _movePointPath.GetNextMovePoint(_targetMovePointIndex);  // setting next point in path
        _targetMovePoint = _movePointPath.GetMovePoint(_targetMovePointIndex);

        _elapsedTime = 0;

        float distanceToMovePoint = Vector3.Distance(_previousMovePoint.position, _targetMovePoint.position); // Distance from point A to Point B
        _timeToMovePoint = distanceToMovePoint / _speed;
    }

    private void OnTriggerEnter(Collider other)
    {
            other.transform.SetParent(transform);  // making object that enters a child of the platform
    }

    private void OnTriggerExit(Collider other)
    {
        other.transform.SetParent(null);  // when object leaves it is no longer a child of platform
    }
}
