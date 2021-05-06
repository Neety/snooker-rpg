using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToActive : MonoBehaviour
{
    [SerializeField] GameObject battleHandler;
    [SerializeField] private float transitionSpeed;
    private Vector3 offset = new Vector3(0, 0, 1);
    private Transform currActive;
    private bool inPosition, change;
    public bool moving = false;

    private void Start()
    {
        inPosition = false;

        transform.position = battleHandler.GetComponent<BattleHandler>().GetEntity(false)[0].transform.position;

        currActive = battleHandler.GetComponent<BattleHandler>().GetEntity(false)[0].transform;
    }

    private void FixedUpdate()
    {
        if (moving == true)
        {
            transform.position = currActive.position + offset;
        }

        if (battleHandler.GetComponent<BattleHandler>().GetActive() == "Player")
            currActive = battleHandler.GetComponent<BattleHandler>().GetEntity(false)[0].transform;
        else
            currActive = battleHandler.GetComponent<BattleHandler>().GetEntity(true)[0].transform;

        if (transform.position == currActive.position + offset)
            inPosition = true;
        else
            inPosition = false;


    }
    private void LateUpdate()
    {
        if (inPosition == false)
        {
            transform.position = Vector3.Lerp(transform.position, currActive.position + offset, Time.deltaTime * transitionSpeed);
        }
    }

    public bool CheckPosition()
    {
        return inPosition;
    }
}
