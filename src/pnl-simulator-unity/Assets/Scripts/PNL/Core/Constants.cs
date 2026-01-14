namespace PNL.Core
{
    /// <summary>
    /// Project Niche-Layer 物理シミュレーション用定数定義
    /// 定義元: src/pnl-simulator-unity/README.md
    /// </summary>
    public static class Constants
    {
        // ---------------------------------------------------------
        // Mass Configuration (kg)
        // ---------------------------------------------------------
        
        /// <summary>
        /// ポッド（客室）の空車重量
        /// </summary>
        public const float POD_EMPTY_MASS_KG = 400.0f;

        /// <summary>
        /// ポッドの最大積載量（ペイロード）
        /// </summary>
        public const float POD_MAX_PAYLOAD_KG = 250.0f;

        /// <summary>
        /// フレーム（飛行体）の自重（バッテリー含む）
        /// </summary>
        public const float FRAME_MASS_KG = 350.0f;

        // ---------------------------------------------------------
        // Safety / Thresholds
        // ---------------------------------------------------------

        /// <summary>
        /// 逆噴射トリガーを作動させる対地高度閾値 (m)
        /// Whitepaper 4.2: 対地高度 10m 以下での逆噴射点火
        /// </summary>
        public const float EMERGENCY_TRIGGER_ALTITUDE_M = 10.0f;

        /// <summary>
        /// 自由落下判定とみなす無重力状態の閾値 (G)
        /// Whitepaper 4.2: 0G状態の特定
        /// </summary>
        public const float FREEFALL_G_THRESHOLD = 0.1f;
    }
}
