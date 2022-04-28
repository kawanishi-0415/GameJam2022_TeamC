using UnityEngine;
using System.Collections;

namespace SGJ
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerController : MonoBehaviour
    {
        [Label("コントローラー"), SerializeField]
        private FloatingJoystick m_joystick = null;

        [Label("移動スピード"), SerializeField]
        private float m_speed = 1f;

        [Label("自動で走る"), SerializeField]
        private bool m_isAutoRun = false;

        [SerializeField]
        private Animator m_animator = null;

        private Rigidbody m_rigidbody = null;
        private Camera m_camera = null;

        private bool m_isEnd = false;

        private void Start()
        {
            m_rigidbody = GetComponent<Rigidbody>();
            m_camera = Camera.main;
        }

        private void FixedUpdate()
        {
            // プレイ中
            if (GameManager.Instance.IsPlay)
            {
                // カメラの方向から、X-Z平面の単位ベクトルを取得
                Vector3 cameraForward = Vector3.Scale(m_camera.transform.forward, new Vector3(1, 0, 1)).normalized;

                // 方向キーの入力値とカメラの向きから、移動方向を決定
                Vector3 moveForward = cameraForward * m_joystick.Vertical + m_camera.transform.right * m_joystick.Horizontal;

                if (m_isAutoRun)
                {
                    moveForward += Vector3.forward;
                }

                // キャラクターの向きを進行方向に
                if (moveForward != Vector3.zero)
                {
                    transform.rotation = Quaternion.LookRotation(moveForward);
                    m_rigidbody.MovePosition(m_rigidbody.position + moveForward * m_speed * Time.deltaTime);
                }
                else
                {
                    // 物理挙動Off
                    m_rigidbody.velocity = Vector3.zero;
                }
                m_animator.SetFloat("Speed", moveForward.magnitude);
            }
            else
            {
                if (m_isEnd == false)
                {
                    // クリアした？
                    if (GameManager.Instance.IsGameClear)
                    {
                        // カメラの方を向く
                        var vec = m_camera.transform.position - transform.position;
                        vec.y = 0;
                        transform.forward = vec.normalized;

                        // アニメーションセット
                        m_animator.SetBool("isClear", true);
                        m_isEnd = true;
                    }
                }
            }
        }

    }

}
