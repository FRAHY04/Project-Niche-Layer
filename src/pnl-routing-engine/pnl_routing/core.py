from dataclasses import dataclass
from typing import List, Tuple

@dataclass
class RouteSegment:
    """経路の一区間を表すデータクラス"""
    from_node_id: str
    to_node_id: str
    distance_m: float
    flight_time_s: float
    is_swap_point: bool = False
    swap_cost_s: float = 0.0

@dataclass
class RouteResult:
    """計算された経路全体の結果"""
    total_time_s: float
    total_distance_m: float
    segments: List[RouteSegment]
    
    def __str__(self):
        return (f"Route Result:\n"
                f"  Total Time    : {self.total_time_s:.2f} s\n"
                f"  Total Distance: {self.total_distance_m:.2f} m\n"
                f"  Swap Counts   : {sum(1 for s in self.segments if s.is_swap_point)}")
