using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpPlatform.GL.Utils
{
	unsafe public class GLBuffer : IDisposable
	{
		uint Buffer;

		public GLBuffer()
		{
			Initialize();
		}

		private void Initialize()
		{
			fixed (uint* BufferPtr = &Buffer)
			{
				GL.glGenBuffers(1, BufferPtr);
			}
		}

		public GLBuffer SetData(int Size, void* Data)
		{
			Bind();
			GL.glBufferData(GL.GL_ARRAY_BUFFER, (uint)Size, Data, GL.GL_STATIC_DRAW);
			return this;
		}

		public void Bind()
		{
			GL.glBindBuffer(GL.GL_ARRAY_BUFFER, Buffer);
		}

		public void Dispose()
		{
			fixed (uint* BufferPtr = &Buffer)
			{
				GL.glDeleteBuffers(1, BufferPtr);
			}
		}
	}
}
