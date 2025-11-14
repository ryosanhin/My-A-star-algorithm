namespace TileMaps.AStarAlgorithms{
	/// <summary>
	/// A*アルゴリズム用インターフェース
	/// </summary>
	public interface IAStarable{
		/// <summary>
		/// 通行可能か(=IsWalkable)
		/// </summary>
		bool IsOpenable{get;}
		
		/// <summary>
		/// 通行コスト
		/// </summary>
		float Cost{get;}
	}
}