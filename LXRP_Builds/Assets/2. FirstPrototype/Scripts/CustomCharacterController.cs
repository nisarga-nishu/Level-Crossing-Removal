﻿using UnityEngine;
using UnityEngine.AI;

public class CustomCharacterController : MonoBehaviour
{
    [SerializeField] Camera cam = null;
    [SerializeField] NavMeshAgent agent = null;
    [SerializeField] Animator animator = null;

    const float locomotionAnimationSmoothTime = .1f;

    private void Start()
    {
        if (cam == null && agent == null && animator)
        {
            Debug.Log("Referrences missing in CustomCharacterController Script");
        }
    }

    void Update()
    {
        float speedPercent = agent.velocity.magnitude / agent.speed;
        animator.SetFloat("speedPercent", speedPercent, locomotionAnimationSmoothTime, Time.deltaTime);

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            Ray ray = cam.ScreenPointToRay(Input.GetTouch(0).position);
            RaycastHit hit;

            if(Physics.Raycast(ray, out hit))
            {
                agent.SetDestination(hit.point);
            }
        }
    }
}
