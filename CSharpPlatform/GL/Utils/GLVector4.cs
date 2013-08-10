using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpPlatform.GL.Utils
{
	unsafe public struct GLVector4
	{
		public float v0, v1, v2, v3;

		public float this[int Index]
		{
			get { fixed (float* vPtr = &v0) return vPtr[Index]; }
			set { fixed (float* vPtr = &v0) vPtr[Index] = value; }
		}

		public void Set(float v0, float v1, float v2, float v3)
		{
			this.v0 = v0;
			this.v1 = v1;
			this.v2 = v2;
			this.v3 = v3;
		}

		public override string ToString()
		{
			return "(" + v0 + ", " + v1 + ", " + v2 + ", " + v3 + ")";
		}
	}
}
