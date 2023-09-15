using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowingCamera : MonoBehaviour
{
    public Transform target;//объявление переменной для отслеживания персонажа, за которым должна следовать камера
    void Update()
    {
        CameraMoving();
    }
    private void CameraMoving() => transform.position = new Vector3(target.transform.position.x, 
        target.transform.position.y, transform.position.z);//вычисление позиции камеры кажддую секунду
}
