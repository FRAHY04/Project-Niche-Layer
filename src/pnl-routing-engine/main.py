import sys
import os

# パスを通す（簡易的）
sys.path.append(os.path.dirname(os.path.abspath(__file__)))

from pnl_routing.graph import RoutingEngine

def main():
    print("=== Project Niche-Layer Routing Engine Prototype ===")
    
    # エンジン初期化 (交換コスト: 45秒, 巡航速度: 20m/s = 72km/h)
    engine = RoutingEngine(swap_time_s=45.0, drone_speed_mps=20.0)

    # マップ構築 (架空の都市グリッド: 1単位=1000m)
    # Hub A (0,0) -> Hub B (0, 2000) -> Hub C (2000, 2000)
    #              \
    #               -> Hub D (1000, 1000) -> Hub E (3000, 0)
    
    engine.add_hub("Hub_A", (0, 0))
    engine.add_hub("Hub_B", (0, 2000))
    engine.add_hub("Hub_C", (2000, 2000)) # Goal
    engine.add_hub("Hub_D", (1000, 1000)) # Shortcut?
    
    # ルート接続 (双方向ではない)
    engine.add_route("Hub_A", "Hub_B")
    engine.add_route("Hub_B", "Hub_C")
    
    engine.add_route("Hub_A", "Hub_D")
    engine.add_route("Hub_D", "Hub_C") # 斜め移動ショートカット

    print("Searching path from Hub_A to Hub_C...")
    result = engine.find_shortest_path("Hub_A", "Hub_C")

    if result:
        print(result)
        print("\nDetails:")
        for i, seg in enumerate(result.segments):
            print(f"  Segment {i+1}: {seg.from_node_id} -> {seg.to_node_id}")
            print(f"    Flight: {seg.flight_time_s:.1f}s, Distance: {seg.distance_m:.1f}m")
            if seg.is_swap_point:
                print(f"    [SWAP ACTION] Battery replacement (+{seg.swap_cost_s}s)")
    else:
        print("No path found.")

if __name__ == "__main__":
    main()
