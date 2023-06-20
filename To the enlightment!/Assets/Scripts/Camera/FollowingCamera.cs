using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowingCamera : MonoBehaviour
{
    public Transform target;//объявление для отслеживания персонажа, за которым должна следовать камера
    void Update()
    {
        transform.position = new Vector3(target.transform.position.x, target.transform.position.y,transform.position.z);//вычисление позиции камеры кажддую секунду

    }
}
