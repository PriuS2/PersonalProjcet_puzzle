using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using UnityEngine;


public class EaterAgent : Agent
{
     public override void Initialize()
    {

    }

    private void Start()
    {

    }

    //에이전트가 값을 추정했을 때 그 값으로 행도할 내용을 실행하는 함수
    //매 프레임(default)마다 실행된다. 설정가능
    public override void OnActionReceived(float[] vectorAction)
    {
        
    }

    // 프로그래머가 로직을 만들거나, 키 입력을 직접 할 때에 실행하는 함수
    public override void Heuristic(float[] actionsOut)
    {

    }
    
    // Agent 리셋 시 실행
    public override void OnEpisodeBegin()
    {

    }

    public override void CollectObservations(VectorSensor sensor)
    {

    }
}
