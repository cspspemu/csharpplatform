using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpPlatform
{
	unsafe public struct Vector4
	{
		public float x, y, z, w;

		public float this[int Index]
		{
			get { fixed (float* ValuesPtr = &x) return ValuesPtr[Index]; }
			set { fixed (float* ValuesPtr = &x) ValuesPtr[Index] = value; }
		}

		static public Vector4 Create(params float[] Values)
		{
			var Vector = default(Vector4);
			for (int n = 0; n < 4; n++) Vector[n] = Values[n];
			return Vector;
		}

		public void AddInplace(Vector4 Right)
		{
			for (int n = 0; n < 4; n++) this[n] = Right[n];
		}

		static public void Add(ref Vector4 Left, ref Vector4 Right, ref Vector4 Destination)
		{
			for (int n = 0; n < 4; n++) Destination[n] = Left[n] + Right[n];
		}

		public override string ToString()
		{
			return String.Format("Vector4({0}, {1}, {2}, {3})", x, y, z, w);
		}
	}
}
