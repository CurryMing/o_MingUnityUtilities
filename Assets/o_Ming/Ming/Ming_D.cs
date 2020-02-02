using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace o_Ming
{
    public class Ming_D : MonoBehaviour
    {
        public enum RotationAxes
        {
            MouseXAndY = 0,
            MouseX = 1,
            MouseY = 2
        }

        //转向Target
        public static void LookToTarget(Transform origin, Vector3 target, float smoothTurn)
        {
            Vector3 t_dir = target - origin.position;
            Quaternion lookRotation = Quaternion.LookRotation(t_dir);
            Vector3 newRotation = Quaternion.Lerp(origin.rotation, lookRotation, smoothTurn * Time.deltaTime).eulerAngles;
            origin.rotation = Quaternion.Euler(0f, newRotation.y, 0f);
        }
        //面向鼠标
        public static void TurnToMouse3D(Transform origin, bool isReverse, LayerMask layerMask)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100f, layerMask))
            {
                if (hit.collider != null)
                {
                    Vector3 turnToTarget = hit.point - origin.position;

                    Quaternion lookRotation = Quaternion.LookRotation(turnToTarget);
                    Vector3 rotation = lookRotation.eulerAngles;
                    if (isReverse)
                        origin.rotation = Quaternion.Euler(0f, rotation.y, 0f);
                    else
                        origin.rotation = Quaternion.Euler(0f, rotation.y, 0f);
                }
            }
        }
    }
}

