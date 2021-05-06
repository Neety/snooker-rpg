using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class TargetSwitch : MonoBehaviour
{
    CinemachineVirtualCamera entityCam;
    [SerializeField] BattleHandler battleHandler;

    private void Awake()
    {
        entityCam = GetComponent<CinemachineVirtualCamera>();
    }

    public IEnumerator SwitchTarget(GameObject entity)
    {
        entityCam.LookAt = entity.transform;
        yield return new WaitForSeconds(5f);
        entityCam.Follow = entity.transform;
    }

    private void LateUpdate()
    {

    }
}
