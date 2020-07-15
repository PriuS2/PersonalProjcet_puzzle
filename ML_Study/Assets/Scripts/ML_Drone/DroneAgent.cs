using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using UnityEngine;
using Random = UnityEngine.Random;

public class DroneAgent : Agent
{
    private DroneMovement _droneController;
    private DroneSensors _sensor;

    public float[] result;

    public Transform targetObject;
    public Transform[] obstacles;

    [SerializeField]
    private Vector3 _initPosition;

    private Rigidbody _rigidbody;

    public TextMeshPro text;
    
    // Agent 클래스가 처음 실행될 때 실행 //생명주기 Awake() -> OnEnabled() -> Initialize() -> start() -> OnEpisodeBegin()
    public override void Initialize()
    {
        _droneController = GetComponent<DroneMovement>();
        _sensor = GetComponent<DroneSensors>();
        _initPosition = transform.position;
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        _droneController.customFeed = true;
    }

    //에이전트가 값을 추정했을 때 그 값으로 행도할 내용을 실행하는 함수
    //매 프레임(default)마다 실행된다. 설정가능
    public override void OnActionReceived(float[] vectorAction)
    {
        int index = 0;
        _droneController.CustomFeed_pitch = vectorAction[index++];//앞뒤
        _droneController.CustomFeed_roll = vectorAction[index++];//좌우
        // print(((vectorAction[index]) + 1) *0.5f);
        _droneController.CustomFeed_throttle = ((vectorAction[index++]) + 1) *0.5f;//(vectorAction[index++] + 1) *0.5f;//상하

        _droneController.CustomFeed_yaw = vectorAction[index++];//(좌,우)회전


        CheckScore();

    }

    // 프로그래머가 로직을 만들거나, 키 입력을 직접 할 때에 실행하는 함수
    public override void Heuristic(float[] actionsOut)
    {
        int index = 0;
        var pitchInput = 0.0f;
        pitchInput += Input.GetKey(KeyCode.W) ? 1.0f : 0.0f;
        pitchInput += Input.GetKey(KeyCode.S) ? -1.0f : 0.0f;
        actionsOut[index++] = pitchInput;
        
        var rollInput = 0.0f;
        rollInput += Input.GetKey(KeyCode.D) ? 1.0f : 0.0f;
        rollInput += Input.GetKey(KeyCode.A) ? -1.0f : 0.0f;
        actionsOut[index++] = rollInput;

        var throttleInput = 0.0f;
        throttleInput += Input.GetKey(KeyCode.I) ? 1f : -1f;
        //throttleInput += Input.GetKey(KeyCode.K) ? -1.0f : 0.0f;
        actionsOut[index++] = throttleInput;

        var yawInput = 0.0f;
        yawInput += Input.GetKey(KeyCode.L) ? -1.0f : 0.0f;
        yawInput += Input.GetKey(KeyCode.J) ? 1.0f : 0.0f;
        actionsOut[index++] = yawInput;

    }
    
    // Agent 리셋 시 실행
    public override void OnEpisodeBegin()
    {
        transform.position = _initPosition;
        transform.rotation = Quaternion.identity;
        ReplaceObjects();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        var deltaPos = targetObject.position - transform.position;
        sensor.AddObservation(deltaPos);
        
        sensor.AddObservation(transform.eulerAngles);
        
        result = _sensor.SensorCheck();
        sensor.AddObservation(result);
    }


    private void CheckScore()
    {
        //기본점수(per tick)
        AddReward(-0.01f);
        
        //거리비례점수
        var deltaDistance = Vector3.Distance(transform.position, targetObject.position);
        var distanceReward = -0.01f * deltaDistance + 0.1f;
        if( distanceReward> 0) AddReward(distanceReward);

        if (deltaDistance < 1)
        {
            AddReward(0.2f);
        }

        var deltaDir = (transform.position - targetObject.position).normalized;


        var dirDot = Vector3.Dot(_rigidbody.velocity.normalized, deltaDir);
        // if (Vector3.Dot(_rigidbody.velocity.normalized, deltaDir) > 1)
        // {
        //     AddReward(0.02f);
        // }
        // else
        // {
        //     AddReward(-0.02f);
        // }
        
        AddReward(dirDot*0.1f);
        
        
        
        // // dot(Vector3.up, transform.up) > 0   => dot(Vector3.up, transform.up) * 점수
        // // else reset
        //
        // var dot = Vector3.Dot(Vector3.up, transform.up);
        //
        // if (dot > 0)
        // {
        //     // AddReward(dot*0.007f);
        // }
        // else
        // {
        //     AddReward(-4.0f);
        //     EndEpisode();
        // }
        //
        if (transform.position.y < -0.3f)
        {
            AddReward(-4.0f);
            EndEpisode();
        }
        
        if (deltaDistance > 35)
        {
            AddReward(-4.0f);
            EndEpisode();
        }


        text.text = GetCumulativeReward().ToString();
    }

    // private void OnTriggerStay(Collider other)
    // {
    //     AddReward(-0.07f);
    // }

    private void OnTriggerEnter(Collider other)
    {
        AddReward(-4.0f);
        //EndEpisode();
    }


    private void ReplaceObjects()
    {
        
        
        foreach (var obstacle in obstacles)
        {
            var x = _initPosition.x + GetRandomRange(15, 2);
            var z = _initPosition.z + GetRandomRange(15, 2);
            var y = _initPosition.y + Random.Range(2.0f, 25.0f);
            var newPos = new Vector3(x,y,z);

            var newRot = Random.rotation;

            var scaleX = Random.Range(2.0f, 5.0f);
            var newScale = new Vector3(scaleX, 1, 1);
            
            obstacle.SetPositionAndRotation(newPos, newRot);
            obstacle.localScale = newScale;
        }
        StartCoroutine(ReplaceTarget());
    }

    private float GetRandomRange(float range, float offset)
    {
        var result = Random.Range(offset, range);
        
        var sign = Random.Range(0,100) > 50;

        result *= sign ? 1.0f : -1.0f;

        return result;
    }

    IEnumerator ReplaceTarget()
    {
        yield return null;

        while (true)
        {
            var x = _initPosition.x + GetRandomRange(15, 2);
            var z = _initPosition.z + GetRandomRange(15, 2);
            var y = _initPosition.y + Random.Range(2.0f, 25.0f);
            var newPos = new Vector3(x,y,z);

            var cols = Physics.OverlapSphere(newPos, 3);
            if (cols.Length == 0)
            {
                targetObject.position = newPos;
                break;
            }
        }
    }
}
