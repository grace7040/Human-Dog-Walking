using System.Collections;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class LeashController : MonoBehaviour
{
    public Transform person;   // 사람의 위치
    public Transform dog;      // 강아지의 위치
    public float maxLeashLength = 2.2f;   // 줄이 당겨지는 최대 거리
    public float tensionForce = 10.0f;    // 장력이 가해지는 힘 크기
    public float leashTolerance = 0.3f;   // 히스테리시스를 고려한 여유 길이
    public Transform ikTarget;
    public Transform OriginalPos;
    public BioAnimation_Original characterMove;
    

    void Start()
    {
        StartCoroutine(nameof(CalculateLeashTensionCoroutine));
    }

    IEnumerator CalculateLeashTensionCoroutine()
    {
        Vector3 ikTargetPosition;
        Quaternion ikTargetRotation;
        Vector3 tensionDirection;
        float distance;
        bool isTensioned = false;

        yield return new WaitForSeconds(2f);

        while (true)
        {
            // 사람과 강아지 사이의 거리 계산
            distance = Vector3.Distance(person.position, dog.position);

            if (!isTensioned && distance > maxLeashLength + leashTolerance)
            {
                isTensioned = true;
                
            }
            else if (isTensioned && distance < maxLeashLength)
            {
                isTensioned = false;
            }

            if (isTensioned)
            {
                // 캐릭터에 당김 효과 적용
                tensionDirection = (dog.position - person.position).normalized;
                characterMove.ApplyLeashPull(true, tensionDirection.x * 10000, tensionDirection.z * 10000);

                // IK Target 이동
                ikTargetPosition = new Vector3(dog.position.x, dog.position.y - 2, dog.position.z);
                ikTargetRotation = Quaternion.LookRotation(tensionDirection);
                UpdateIKTargetTransform(ikTargetPosition, ikTargetRotation);
            }
            else
            {
                // 캐릭터 당김 효과 해제
                characterMove.ApplyLeashPull(false);

                // IK Target 원위치
                if (distance < maxLeashLength - leashTolerance)
                {
                    UpdateIKTargetTransform(OriginalPos.position, OriginalPos.rotation, 2f);
                }
            }


            yield return null;
        }
    }

    void UpdateIKTargetTransform(Vector3 targetPosition, Quaternion targetRotation, float speed = 1f)
    {
        ikTarget.SetPositionAndRotation(Vector3.Lerp(ikTarget.position, targetPosition, speed * Time.deltaTime), 
                                    Quaternion.Slerp(ikTarget.rotation, targetRotation, speed * Time.deltaTime));
    }

}
