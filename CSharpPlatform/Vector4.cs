using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CSharpPlatform
{
	[StructLayout(LayoutKind.Sequential, Pack = 1, Size = 16)]
	unsafe public struct Vector4fRaw
	{
		public float X, Y, Z, W;

		public static Vector4fRaw Zero {
			get { return new Vector4fRaw(0, 0, 0, 0); }
		}

		public float this[int Index]
		{
			get { fixed (float* ValuesPtr = &X) return ValuesPtr[Index]; }
			set { fixed (float* ValuesPtr = &X) ValuesPtr[Index] = value; }
		}

		public Vector4fRaw(float X, float Y, float Z, float W)
		{
			this.X = X;
			this.Y = Y;
			this.Z = Z;
			this.W = W;
		}

		public static Vector4fRaw operator*(Vector4fRaw Vector, float Multiplier)
		{
			return new Vector4fRaw()
			{
				X = Vector.X * Multiplier,
				Y = Vector.Y * Multiplier,
				Z = Vector.Z * Multiplier,
				W = Vector.W * Multiplier,
			};
		}

		public static Vector4fRaw operator +(Vector4fRaw Vector1, Vector4fRaw Vector2)
		{
			return new Vector4fRaw()
			{
				X = Vector1.X + Vector2.X,
				Y = Vector1.Y + Vector2.Y,
				Z = Vector1.Z + Vector2.Z,
				W = Vector1.W + Vector2.W,
			};
		}

		static public Vector4fRaw Create(params float[] Values)
		{
			var Vector = default(Vector4fRaw);
			for (int n = 0; n < 4; n++) Vector[n] = Values[n];
			return Vector;
		}

		public void AddInplace(Vector4fRaw Right)
		{
			for (int n = 0; n < 4; n++) this[n] = Right[n];
		}

		static public void Add(ref Vector4fRaw Left, ref Vector4fRaw Right, ref Vector4fRaw Destination)
		{
			for (int n = 0; n < 4; n++) Destination[n] = Left[n] + Right[n];
		}

		public override string ToString()
		{
			return String.Format("Vector4({0}, {1}, {2}, {3})", X, Y, Z, W);
		}
	}
}
