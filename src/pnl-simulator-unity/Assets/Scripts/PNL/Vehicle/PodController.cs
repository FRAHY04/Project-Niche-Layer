using UnityEngine;
using PNL.Core;

namespace PNL.Vehicle
{
    /// <summary>
    /// サバイバル・カプセル（ポッド）の自律制御クラス
    /// フレーム（飛行体）の状態に依存せず、自身のセンサーのみで安全を確保する責務を持つ。
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    public class PodController : MonoBehaviour
    {
        [Header("Status")]
        [SerializeField] private bool _isDocked = false;
        [SerializeField] private float _currentAltitude;
        [SerializeField] private Vector3 _acceleration;

        private Rigidbody _rb;

        void Start()
        {
            _rb = GetComponent<Rigidbody>();
            _rb.mass = Constants.POD_EMPTY_MASS_KG;
            
            // 物理挙動の初期化
            // ポッドはフレームに掴まれるまでは物理演算に従う
            _rb.isKinematic = false; 
        }

        void FixedUpdate()
        {
            // センサー情報の更新 (Simulation)
            UpdateSensors();

            // 安全監視ロジック (Safety Kernel Prototype)
            CheckSafetyProtocols();
        }

        private void UpdateSensors()
        {
            // Raycastによる対地高度計測（擬似LiDAR）
            if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit))
            {
                _currentAltitude = hit.distance;
            }
            else
            {
                _currentAltitude = float.MaxValue;
            }

            // IMU加速度（擬似）: 現在の物理演算加速度を取得
            // UnityのRigidbodyからは直接取得できないため、速度差分等で計算する必要があるが
            // ここでは簡易的に重力影響下のVelocity変化を監視する想定
             _acceleration = _rb.linearVelocity; // ※Unity 6 / 2023以降はlinearVelocity, 古い場合はvelocity
        }

        /// <summary>
        /// 独立自律型判定ロジック (Whitepaper 4.2)
        /// </summary>
        private void CheckSafetyProtocols()
        {
            // 1. 自由落下検知 (TODO: 実装)
            
            // 2. 超低遅延トリガー (高度10m以下 && 降下中)
            if (_currentAltitude < Constants.EMERGENCY_TRIGGER_ALTITUDE_M && _rb.linearVelocity.y < -1.0f)
            {
                EmergencyLanding();
            }
        }

        private void EmergencyLanding()
        {
            // ここに逆噴射ロジックを実装 (Issue #3)
            // Debug.LogWarning($"[POD] Emergency Thrusters ENGAGED at Alt: {_currentAltitude}m");
        }

        // ---------------------------------------------------------
        // Physical API (Latch)
        // ---------------------------------------------------------
        public void OnLatched()
        {
            _isDocked = true;
            // 物理挙動をフレームに委ねる場合、ここでJoint接続などの処理を行う
        }

        public void OnUnlatched()
        {
            _isDocked = false;
        }
    }
}
