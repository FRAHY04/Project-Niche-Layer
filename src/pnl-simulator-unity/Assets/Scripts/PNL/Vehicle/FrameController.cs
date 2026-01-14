using UnityEngine;
using PNL.Core;

namespace PNL.Vehicle
{
    /// <summary>
    /// フレーム（飛行ドローンユニット）の制御クラス
    /// ポッドを輸送するための「実行基盤」としての機能のみを持つ。
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    public class FrameController : MonoBehaviour
    {
        private Rigidbody _rb;
        
        [Header("Hardware Specs")]
        [SerializeField] private float _batteryLevel = 100f; // %

        void Start()
        {
            _rb = GetComponent<Rigidbody>();
            _rb.mass = Constants.FRAME_MASS_KG;
        }

        void FixedUpdate()
        {
            // 飛行制御ロジック (Placeholder)
            // ここにPID制御や推力計算が入る
        }

        /// <summary>
        /// ポッドとのドッキング（物理結合）を行う
        /// </summary>
        /// <param name="pod">対象ポッド</param>
        public void Dock(PodController pod)
        {
            // ラッチ機構の実装 (Issue #1)
            Debug.Log("[FRAME] Docking sequence initiated.");
            pod.OnLatched();
            
            // Jointコンポーネントによる結合を予定
            // var joint = gameObject.AddComponent<FixedJoint>();
            // joint.connectedBody = pod.GetComponent<Rigidbody>();
        }

        /// <summary>
        /// 緊急または通常の切り離し
        /// </summary>
        public void Undock(PodController pod)
        {
            Debug.Log("[FRAME] Undocking pod.");
            pod.OnUnlatched();
            
            // Jointの破棄
            // Destroy(GetComponent<FixedJoint>());
        }
    }
}
