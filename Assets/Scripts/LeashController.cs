using System.Collections;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class LeashController : MonoBehaviour
{
    public Transform person;   // ����� ��ġ
    public Transform dog;      // �������� ��ġ
    public float maxLeashLength = 2.2f;   // ���� ������� �ִ� �Ÿ�
    public float tensionForce = 10.0f;    // ����� �������� �� ũ��
    public float leashTolerance = 0.3f;   // �����׸��ý��� ����� ���� ����
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
            // ����� ������ ������ �Ÿ� ���
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
                // ĳ���Ϳ� ��� ȿ�� ����
                tensionDirection = (dog.position - person.position).normalized;
                characterMove.ApplyLeashPull(true, tensionDirection.x * 10000, tensionDirection.z * 10000);

                // IK Target �̵�
                ikTargetPosition = new Vector3(dog.position.x, dog.position.y - 2, dog.position.z);
                ikTargetRotation = Quaternion.LookRotation(tensionDirection);
                UpdateIKTargetTransform(ikTargetPosition, ikTargetRotation);
            }
            else
            {
                // ĳ���� ��� ȿ�� ����
                characterMove.ApplyLeashPull(false);

                // IK Target ����ġ
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
