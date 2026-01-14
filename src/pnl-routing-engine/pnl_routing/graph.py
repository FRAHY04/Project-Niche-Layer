import networkx as nx
import math
from typing import List, Dict
from .core import RouteSegment, RouteResult

class RoutingEngine:
    """
    Project Niche-Layer 専用経路探索エンジン
    白書 2.2: 全行程の移動時間 T_total = Σ(T_flight + T_swap) に基づくコスト計算を行う
    """

    def __init__(self, swap_time_s: float = 45.0, drone_speed_mps: float = 20.0):
        """
        Args:
            swap_time_s (float): バッテリー交換にかかる固定コスト (T_swap)
            drone_speed_mps (float): ドローンの平均巡航速度 (m/s)
        """
        self.swap_time_s = swap_time_s
        self.drone_speed_mps = drone_speed_mps
        self.graph = nx.DiGraph()

    def add_hub(self, hub_id: str, pos: Tuple[float, float]):
        """ノード（ハブ）を追加"""
        self.graph.add_node(hub_id, pos=pos, type='hub')

    def add_route(self, from_id: str, to_id: str, distance_m: float = None):
        """
        エッジ（移動ルート）を追加
        distance_m が指定されない場合、座標からユークリッド距離を計算
        """
        if distance_m is None:
            p1 = self.graph.nodes[from_id]['pos']
            p2 = self.graph.nodes[to_id]['pos']
            distance_m = math.sqrt((p1[0]-p2[0])**2 + (p1[1]-p2[1])**2)

        flight_time = distance_m / self.drone_speed_mps
        
        # エッジの重み = 飛行時間 + 交換コスト(宛先がハブの場合)
        # ここでは単純化のため、全てのエッジ移動後に交換が必要なモデルとする
        # (実際はバッテリー残量に応じた動的判定が必要: Issue #2)
        weight = flight_time + self.swap_time_s

        self.graph.add_edge(from_id, to_id, 
                            weight=weight, 
                            distance=distance_m,
                            flight_time=flight_time)

    def find_shortest_path(self, start_id: str, goal_id: str) -> RouteResult:
        """Dijkstra法に基づく最短時間経路の探索"""
        try:
            # networkxのDijkstraはweightパラメータを最小化する
            path_nodes = nx.dijkstra_path(self.graph, start_id, goal_id, weight='weight')
            
            segments = []
            total_time = 0.0
            total_dist = 0.0

            for i in range(len(path_nodes) - 1):
                u = path_nodes[i]
                v = path_nodes[i+1]
                data = self.graph[u][v]
                
                # ゴール地点ではスワップ不要とする補正
                is_goal = (v == goal_id)
                swap_cost = 0.0 if is_goal else self.swap_time_s
                
                segment_time = data['flight_time'] + swap_cost
                
                segments.append(RouteSegment(
                    from_node_id=u,
                    to_node_id=v,
                    distance_m=data['distance'],
                    flight_time_s=data['flight_time'],
                    is_swap_point=(not is_goal),
                    swap_cost_s=swap_cost
                ))
                
                total_time += segment_time
                total_dist += data['distance']

            return RouteResult(total_time, total_dist, segments)

        except nx.NetworkXNoPath:
            return None
