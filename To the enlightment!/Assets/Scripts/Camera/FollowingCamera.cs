using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowingCamera : MonoBehaviour
{
    public Transform target;//���������� ���������� ��� ������������ ���������, �� ������� ������ ��������� ������
    void Update()
    {
        CameraMoving();
    }
    private void CameraMoving() => transform.position = new Vector3(target.transform.position.x, 
        target.transform.position.y, transform.position.z);//���������� ������� ������ ������� �������
}
