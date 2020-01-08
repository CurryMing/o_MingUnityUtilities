using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.IO;
using UnityEngine.EventSystems;

namespace o_Ming
{

    public class Ming : MonoBehaviour
    {
        //Reflection
        public static void DrawReflection2D(Vector2 originPosition, Vector2 direction, LineRenderer lr,float lr_Length)
        {
            originPosition += direction * 0.7f;

            lr.SetPosition(0, originPosition);

            RaycastHit2D hit = Physics2D.Raycast(originPosition, direction, lr_Length);
            if (hit.collider != null)
            {
                lr.SetPosition(1, hit.point);
                direction = Vector2.Reflect(direction, hit.normal);
                lr.SetPosition(2, (hit.point + direction.normalized * lr_Length));
                originPosition = hit.point;
            }
            else
            {
                lr.SetPosition(1, (originPosition + direction.normalized * lr_Length));
                lr.SetPosition(2, (originPosition + direction.normalized * lr_Length));
            }
        }
        public static void DrawReflection3D(Vector3 originPosition, Vector3 direction, LineRenderer lr, float lr_Length,LayerMask wallLayer)
        {
            originPosition += direction * 0.7f;

            lr.SetPosition(0, originPosition);

            Ray ray = new Ray(originPosition,direction);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, lr_Length, wallLayer))
            {
                if (hit.collider != null)
                {
                    lr.SetPosition(1, hit.point);

                    direction = Vector3.Reflect(direction, hit.normal);
                    lr.SetPosition(2, hit.point + direction * lr_Length);
                    originPosition = hit.point;
                }
                else
                {
                    lr.SetPosition(1, (originPosition + direction * lr_Length));
                    lr.SetPosition(2, (originPosition + direction * lr_Length));
                }
            }

        }

        //抛物线
        public static void LaunchProjectile(Rigidbody2D originRb,float t)
        {
            Vector2 mousePoint = Camera.main.ScreenToWorldPoint((Vector2)Input.mousePosition);
            RaycastHit2D hitInfo = Physics2D.Raycast(mousePoint, mousePoint, 1);
            if (hitInfo.collider != null)
            {
                //Debug.Log(hitInfo.point);
                originRb.velocity = CalculateProjectileVelocity(originRb.position, hitInfo.point, t);
            }
        }

        static Vector2 CalculateProjectileVelocity(Vector2 origin, Vector2 target, float time)
        {
            Vector2 distance = target - origin;

            float Vx = distance.x / time;
            //Debug.Log(Vx);
            float Vy = distance.y / time + 0.5f * Mathf.Abs(Physics2D.gravity.y) * time;

            Vector2 velocity = distance.normalized;
            velocity *= Mathf.Abs(Vx);
            velocity.y = Vy;

            return velocity;
        }

        //EnemyBase
        public static IEnumerator EnemyMove(Rigidbody2D rb,Vector2 targetPosition,float moveSpeed)
        {
            while(rb.position != targetPosition)
            {
                Vector2 moveToTarget = Vector2.MoveTowards(rb.position, targetPosition, moveSpeed * Time.deltaTime);
                rb.MovePosition(moveToTarget);
                yield return null;
            }
        }

        //Bullet
        public static void BulletFire(Transform bulletTransform,Vector2 direction,float fireSpeed)
        {
            bulletTransform.Translate(direction * fireSpeed * Time.deltaTime, Space.World);
        }

        //获取最近的物体
        public static GameObject GetNearestObject(Transform origin,GameObject[] objects)
        {
            GameObject nearestObject = null;
            float nearestDistance = Mathf.Infinity;

            foreach (var m_object in objects)
            {
                float nearestDis = Vector2.Distance(origin.position, m_object.transform.position);
                if (nearestDis < nearestDistance)
                {
                    nearestDistance = nearestDis;
                    nearestObject = m_object;
                }
            }
            return nearestObject;
        }

        //转向目标
        public static void TurnToTarget2D(Transform origin,Vector2 target,bool isReverse,float minAngle = -360,float maxAngle = 360f)
        {
            Vector3 turnToTarget = (target - (Vector2)origin.position).normalized;
            float angle = Mathf.Atan2(turnToTarget.y, turnToTarget.x) * Mathf.Rad2Deg;
            angle = Mathf.Clamp(angle, minAngle, maxAngle);
            if(isReverse)
                origin.rotation = Quaternion.Euler(0f, 0f, angle + 90);
            else
                origin.rotation = Quaternion.Euler(0f, 0f, angle - 90);
        }
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
                    if(isReverse)
                        origin.rotation = Quaternion.Euler(0f, rotation.y, 0f);
                    else
                        origin.rotation = Quaternion.Euler(0f, rotation.y, 0f);
                }
            }
        }


        [Header("摇杆")]
        //JoyStick
        private static Image m_bgImage;
        private static Image m_joystickImage;
        private static Vector3 inputVector3;

        private static Vector2 movement = Vector2.zero;

        public static void SetImage(Image bgImage, Image joystickImage)
        {
            m_bgImage = bgImage;
            m_joystickImage = joystickImage;
        }
        public static void JoyStick(PointerEventData ped)
        {
            Vector2 pos;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(m_bgImage.rectTransform,
                                                                       ped.position,
                                                                       ped.pressEventCamera,
                                                                       out pos))
            {
                pos.x = pos.x / m_bgImage.rectTransform.sizeDelta.x;
                pos.y = pos.y / m_bgImage.rectTransform.sizeDelta.y;
                inputVector3 = new Vector3(pos.x * 2 , pos.y * 2 , 0);
                inputVector3 = (inputVector3.magnitude > 1) ? inputVector3.normalized : inputVector3;

                m_joystickImage.rectTransform.anchoredPosition = new Vector3(
                    inputVector3.x * m_bgImage.rectTransform.sizeDelta.x / 3,
                    inputVector3.y * m_bgImage.rectTransform.sizeDelta.y / 3
                    );
            }
            inputVector3 = inputVector3.normalized;
            Debug.Log(m_bgImage.rectTransform.sizeDelta.x);
            movement = new Vector2(inputVector3.x, inputVector3.y);
        }
        public static void ResetJoyImage()
        {
            inputVector3 = Vector3.zero;
            movement = Vector2.zero;
            m_joystickImage.rectTransform.anchoredPosition = Vector2.zero;
        }

        [Header("Save and Load")]
        private const string SAVE_SEPARATOR = "#SAVE-VALUE#";

        //Save and Load Position
        public static void SavePosition(Vector3 targetPosition)
        {
            /*PlayerPrefs.SetFloat("targetPositionX", targetPosition.x);
            PlayerPrefs.SetFloat("targetPositionY", targetPosition.y);
            PlayerPrefs.Save();*/

            string[] contents = new string[]
            {
                ""+targetPosition.x,
                ""+targetPosition.y
            };
            string saveString = string.Join(SAVE_SEPARATOR, contents);
            File.WriteAllText(Application.dataPath + "/Save.txt", saveString);
            Debug.Log("Saved!");
        }

        public static Vector3 LoadPosition()
        {
            Vector3 targetPosition = Vector3.zero;
            /*if (PlayerPrefs.HasKey("targetPositionX"))
            {
                float targetPositionX = PlayerPrefs.GetFloat("targetPositionX");
                float targetPositionY = PlayerPrefs.GetFloat("targetPositionY");
                targetPosition = new Vector3(targetPositionX, targetPositionY);
            }
            else
            {
                Debug.Log("NoSave");
            }
            return targetPosition;*/
            string targetPos = File.ReadAllText(Application.dataPath + "/Save.txt");
            string[] contents = targetPos.Split(new[] { SAVE_SEPARATOR }, System.StringSplitOptions.None);
            float targetPositionX = float.Parse(contents[0]);
            float targetPositionY = float.Parse(contents[1]);
            targetPosition = new Vector3(targetPositionX, targetPositionY);
            Debug.Log("Loaded!");
            return targetPosition;
        }


        //2D移动
        public static void Move_2D(Rigidbody2D rb2D,float speed)
        {
            float x = 0, y = 0;
            x = Input.GetAxisRaw("Horizontal");
            y = Input.GetAxisRaw("Vertical");

            if (movement.x != 0)
            {
                x = movement.x;
            }

            if (movement.y != 0)
            {
                y = movement.y;
            }

            Vector2 m_movement = new Vector2(x, y);

            rb2D.velocity = m_movement * speed * Time.deltaTime;
        }


        //摄像机震动
        public static IEnumerator CameraShake(GameObject camera, float shakeSpeed, float shakeTime, float shakeAmount)
        {
            float countTimer = 0;
            Vector3 startPos = camera.transform.position;
            while (countTimer <= shakeTime)
            {
                Vector3 randomPoint = startPos + Random.insideUnitSphere * shakeAmount;
                camera.transform.localPosition = Vector3.Lerp(camera.transform.position, randomPoint, shakeSpeed * Time.deltaTime);
                countTimer += Time.deltaTime;
                yield return null;
            }

            camera.transform.localPosition = startPos;
        }
        //摄像机跟随
        public static void CameraFollow(Transform target, Transform cameraTransform, float speed)
        {
            Vector3 offset = target.position - cameraTransform.position;
            Vector3 newPos = cameraTransform.position + offset;
            newPos.z = -10;
            cameraTransform.position = Vector3.Lerp(cameraTransform.position, newPos, speed * Time.deltaTime);
        }


        //文字闪烁
        public static void TextFade(Text text)
        {
            text.color = Color.Lerp(text.color, Color.clear, 5 * Time.deltaTime);
        }
        //文字出现效果
        public static GameObject TextUp2D(GameObject textPrefabs, string textContent,Transform target)
        {
            Vector3 showPos = target.position + new Vector3(2.5f, -1, 0);

            GameObject textClone = Instantiate(textPrefabs, showPos, Quaternion.identity);

            Text m_Text = textClone.GetComponentInChildren<Text>();

            DestroyObjectDelay(textClone, 1.5f);

            m_Text.text = textContent;

            return textClone;
        }
        //文字向上动画
        public static IEnumerator TextAnimation(GameObject textClone)
        {
            RectTransform rectTransform;
            rectTransform = textClone.GetComponent<RectTransform>();

            Vector2 targetPoint = rectTransform.anchoredPosition + Vector2.up;
            float y = rectTransform.anchoredPosition.y;

            while (rectTransform.anchoredPosition.y < targetPoint.y)
            {
                y += Time.deltaTime * 3.5f;
                rectTransform.anchoredPosition = new Vector2(targetPoint.x, y);
                //rectTransform.anchoredPosition =
                //Vector2.Lerp
                //(
                //    rectTransform.anchoredPosition,
                //    targetPoint,
                //    3.5f * Time.deltaTime
                //);

                yield return null;

                if (rectTransform.anchoredPosition.y > targetPoint.y - 0.05)
                {
                    rectTransform.anchoredPosition = targetPoint;
                }
            }
        }

        //物体延迟消失效果
        public static void DestroyObjectDelay(GameObject targetObject,float time)
        {
            Destroy(targetObject, time);
        } 

    }
}
