using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class AgentController : Agent
{

    [Header("Transform do Alvo")]
    //armazena o transform do alvo    
    [SerializeField] private Transform targetTransform;

    [Header("SpriteRenderer do Ground")]
    // pega o spriteRender do ground
    [SerializeField] private SpriteRenderer GroundSpriteRenderer;

    [Header("Velocidade do Agent")]
    //Define a variavel velocidade do movimento do agent
    [SerializeField] private float moveSpeed = 5F;


    public override void OnEpisodeBegin()
    {
        //Retorna o agente para posisão inicial
        transform.localPosition = new Vector2(0f, 1.5f);

        //Spawnar o alvo em uma posisão aleatoria do ground
      //  targetTransform.localPosition = new Vector2(Random.Range(-2f, +2f), Random.Range(-2f, +2f));

    }


    public override void CollectObservations(VectorSensor sensor)
    {
        //coleta os dados da posissão do agent
        sensor.AddObservation(transform.localPosition);

        //coleta a posissão do objetivo alvo
        sensor.AddObservation(targetTransform.localPosition);
    }


    public override void Heuristic(in ActionBuffers actionsOut)
    {
        // controla a movimentação do perssonagem pelo input 
        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
        continuousActions[0] = Input.GetAxisRaw("Horizontal");
        continuousActions[1] = Input.GetAxisRaw("Vertical");
    }


    public override void OnActionReceived(ActionBuffers actions)
    {
        //debuga o valor das acções continuas
        // Debug.Log(actions.ContinuousActions[0]);

        //Movimenta o agent no eixo do vector.X
        float moveX = actions.ContinuousActions[0];

        //Movimenta o agent no eixo do vector.Y
        float moveY = actions.ContinuousActions[1];

         //Define a velocidade do movimento do agent
         transform.localPosition += new Vector3(moveX, moveY, 0) * Time.deltaTime * moveSpeed;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.tag == "coin")
        {
            //da uma recompença positiva ao agente
            SetReward(+1f);

            //termina o episodio;
            EndEpisode();

            //Muda a cor do spriteRender do objeto ground para verde
            GroundSpriteRenderer.color = Color.green;
        }


        if (collision.tag == "wall")
        {
            //da uma recompença negativa ao agente
            SetReward(-1f);

            //termina o episodio;
            EndEpisode();

            //Muda a cor do spriteRender do objeto ground para vermelho
            GroundSpriteRenderer.color = Color.red;
        }
    }


}