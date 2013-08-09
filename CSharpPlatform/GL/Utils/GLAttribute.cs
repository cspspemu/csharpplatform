using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpPlatform.GL.Utils
{
	unsafe public class GLAttribute
	{
		public uint Location;

		internal GLAttribute(uint Location)
		{
			this.Location = Location;
		}

		public int Size
		{
			get
			{
				int Out;
				GL.glGetVertexAttribiv(Location, GL.GL_VERTEX_ATTRIB_ARRAY_SIZE, &Out);
				return Out;
			}
		}

		private void Enable()
		{
			GL.glEnableVertexAttribArray(Location);
		}

		private void Disable()
		{
			GL.glDisableVertexAttribArray(Location);
		}

		public void SetData(GLBuffer Buffer, int ElementSize = 4, int Offset = 0, int Stride = 0, bool Normalize = false)
		{
			Buffer.Bind();
			GL.glVertexAttribPointer(
				Location,
				ElementSize,
				GL.GL_FLOAT,
				Normalize,
				Stride,
				(void*)Offset
			);
		}

		public void SetPointer(float* Data)
		{
			if (Data == null)
			{
				Disable();
			}
			else
			{
				GL.glVertexAttribPointer(
					Location,
					Size,
					GL.GL_FLOAT,
					false,
					0,
					Data
				);
				Enable();
			}
		}

		public override string ToString()
		{
			return String.Format("GLAttribute({0}, Size:{1})", Location, Size);
		}
	}
}
