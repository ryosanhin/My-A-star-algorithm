using System;
using System.Collections;
using System.Collections.Generic;

namespace TileMaps.AStarAlgorithms{	
	public class AStarPathFinder{
		/// <summary>
		/// A*アルゴリズムで使うノードの内部クラス
		/// </summary>
		private class AStarNode : IEquatable<AStarNode>{
			public Coords Coords{get;}
			public AStarNode ParentNode{get;}
			public float Cost{get;}
			public float Heuristic{get;}
			public float Score=>Cost+Heuristic;
			
			/// <summary>
			/// コンストラクタ
			/// </summary>
			/// <prama name="coords">対象の座標</param>
			/// <prama name="cost">対象の座標のコスト</param>
			/// <prama name="goalCoords">目標の座標</param>
			/// <prama name="parentNode">親ノード</param>
			public AStarNode(Coords coords, float cost, Coords goalCoords, AStarNode parentNode){
				this.ParentNode=parentNode;
				this.Coords=coords;
				
				if(parentNode==null){
					//初期ノードの場合
					this.Cost=0f;
				}else{
					this.Cost=parentNode.Cost+cost;
				}
				
				this.Heuristic=Math.Max(goalCoords.X-coords.X, goalCoords.Y-coords.Y);
			}
			
			/// <summary>
			/// IEquatable
			/// </summary>
			public bool Equals(AStarNode other){
				if(other==null){
					return false;
				}
				return this.Coords==other.Coords;
			}
			
			/// <summary>
			/// Object.Equals override
			/// </summary>
			public override bool Equals(object obj){
				if(obj is AStarNode other){
					return this.Equals(other);
				}
				return false;
			}
			
			/// <summary>
			/// Object.GetHashCode override
			/// </summary>
			public override int GetHashCode(){
				return HashCode.Combine(this.Coords);
			}
		}
		
		/// <summary>
		/// A*アルゴリズムで道順のタイルを取得
		/// </summary>
		/// <prama name="start">スタート地点の座標</param>
		/// <prama name="goal">ゴール地点の座標</param>
		/// <prama name="mapCells">マップ情報</param>
		/// <returns>経路</returns>
		public static Coords[] FindPath(Coords startCoords, Coords goalCoords, IAStarable[,] mapCells){
			//Open状態のノードは重複したら困るのでHashSetに格納
			HashSet<AStarNode> openedNodes=new HashSet<AStarNode>();
			//処理に関わった全てのノードはDictionaryに格納
			//Keyをタイル座標にすることでゴールのノードを検索しやすくする
			Dictionary<Coords, AStarNode> allNodes=new Dictionary<Coords, AStarNode>();
			
			//初期ノードを取得
			AStarNode currentNode=new AStarNode(startCoords, 0, goalCoords, null);
			
			//初期基準ノードを両方に登録
			allNodes.Add(currentNode.Coords, currentNode);
			openedNodes.Add(currentNode);
			
			//探索範囲
			Coords[] dires=new Coords[]{
				Coords.Up,
				Coords.Down,
				Coords.Right,
				Coords.Left
			};
			
			int width=mapCells.GetLength(1);
			int height=mapCells.GetLength(0);
			
			//Open状態のノードが無くなるまで探索を続ける
			while(openedNodes.Count>0){
				//基準ノードの周りのノードを開ける
				foreach(Coords dire in dires){
					Coords tempCoords=currentNode.Coords+dire;
					var column=tempCoords.X;
					var row=tempCoords.Y;
					
					//配列の範囲内か確認
					if(row<0 || row>=height || column<0 || column>=width){
						continue;
					}
					
					//開ける場所が歩行可能か、既に処理済み(開封済み)か確認
					if(!mapCells[row, column].IsOpenable || allNodes.ContainsKey(tempCoords)){
						continue;
					}
					//それぞれAStarNodeに格納
					var node=new AStarNode(tempCoords, mapCells[row, column].Cost, goalCoords, currentNode);
					allNodes.Add(node.Coords, node);
					openedNodes.Add(node);
				}
				
				//Open状態のリストから削除
				openedNodes.Remove(currentNode);
				
				//次の基準ノードを取得、別に並べ替える必要はないのでforeachで探索
				//Temp扱いでnextNodeに次のノードを保持
				AStarNode nextNode=null;
				foreach(AStarNode node in openedNodes){
					if(nextNode!=null){
						//今保持しているノードよりもスコアの小さいノードがあったらそっちを保持
						if(node.Score<nextNode.Score){
							nextNode=node;
						}
					}else{
						nextNode=node;
					}
				}
				
				//最後まで保持されたノードが次の基準ノード
				currentNode=nextNode;
			}
			
			//Open状態のノードが無くなった＝探索終了
			//Dictionaryからゴール地点の座標をKeyにゴールのノードを取得
			//ゴールのノードの親ノードから更にその親のノードに……とたどり道順取得
			return NodeToCoords(allNodes[goalCoords]);
		}
		
		/// <summary>
		/// ゴールのノードからスタートのノードまで列挙し、スタートからゴールの順にして返す
		/// </summary>
		/// <prama name="goalNode">ゴールのノード</param>
		/// <returns>経路</returns>
		private static Coords[] NodeToCoords(AStarNode goalNode){
			var currentNode=goalNode;
			List<Coords> paths=new List<Coords>();
			paths.Add(goalNode.Coords);
			
			//親ノードが無くなる、つまり初期ノードになるまでループ
			while(currentNode.ParentNode!=null){
				paths.Add(currentNode.ParentNode.Coords);
				currentNode=currentNode.ParentNode;
			}
			
			//ゴールから遡っていったので反転して先頭にスタートの座標を持ってくる
			paths.Reverse();
			
			return paths.ToArray();
		}
	}
}