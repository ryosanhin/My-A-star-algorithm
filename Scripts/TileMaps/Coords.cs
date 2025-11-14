using System;

namespace TileMaps{
	public readonly struct Coords : IEquatable<Coords>{
		public int X{get;}
		public int Y{get;}
		
		public Coords(int x, int y){
			this.X=x;
			this.Y=y;
		}
		
		#region static variables
		/// <summary>
		/// unit up coods
		/// </summary>
		public static readonly Coords Up=new Coords(0, 1);
		
		/// <summary>
		/// unit down coods
		/// </summary>
		public static readonly Coords Down=new Coords(0, -1);
		
		/// <summary>
		/// unit right coods
		/// </summary>
		public static readonly Coords Right=new Coords(1, 0);
		
		/// <summary>
		/// unit left coods
		/// </summary>
		public static readonly Coords Left=new Coords(-1, 0);
		#endregion
		
		#region operator overload
		/// <summary>
		/// == operator overload
		/// </summary>
		public static bool operator ==(Coords lhs, Coords rhs){
			return lhs.Equals(rhs);
		}
		
		/// <summary>
		/// != operator overload
		/// </summary>
		public static bool operator !=(Coords lhs, Coords rhs){
			return !(lhs==rhs);
		}
		
		/// <summary>
		/// + operator overload
		/// </summary>
		public static Coords operator +(Coords lhs, Coords rhs){
			return new Coords(lhs.X+rhs.X, lhs.Y+rhs.Y);
		}
		
		/// <summary>
		/// - operator overload
		/// </summary>
		public static Coords operator -(Coords lhs, Coords rhs){
			return new Coords(lhs.X-rhs.X, lhs.Y-rhs.Y);
		}
		#endregion
		
		/// <summary>
		/// IEquatable
		/// </summary>
		public bool Equals(Coords other){
			if(other==null){
				return false;
			}
			return this.X==other.X && this.Y==other.Y;
		}
		
		#region overrides
		/// <summary>
		/// Object.Equals override
		/// </summary>
		public override bool Equals(object obj){
			if(obj is Coords other){
				return this.Equals(other);
			}
			return false;
		}
		
		/// <summary>
		/// Object.GetHashCode override
		/// </summary>
		public override int GetHashCode(){
			return HashCode.Combine(this.X, this.Y);
		}
		
		/// <summary>
		/// Object.ToString override
		/// </summary>
		public override string ToString(){
			return $"(X, Y) = ({this.X}, {this.Y})";
		}
		#endregion
	}
}