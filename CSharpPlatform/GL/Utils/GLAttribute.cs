using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpPlatform.GL.Utils
{
	unsafe public class GLUniform
	{
		private GLShader Shader;
		private int Location;

		public GLUniform(GLShader Shader, int Location)
		{
			this.Shader = Shader;
			this.Location = Location;
		}

		public bool IsAvailable
		{
			get { return this.Location < 0; }
		}

		[DebuggerHidden]
		private void CheckAvailable()
		{
			if (!Shader.IsUsing) throw (new Exception("Not using shader"));
		}

		[DebuggerHidden]
		public void Set(GLMatrix4 Matrix)
		{
			Set(new[] { Matrix });
		}

		[DebuggerHidden]
		public void Set(GLMatrix4[] Matrices)
		{
			CheckAvailable();
			Matrices[0].FixValues((Pointer) =>
			{
				GL.glUniformMatrix4fv(Location, Matrices.Length, false, Pointer);
			});
		}
	}

	unsafe public class GLAttribute
	{
		private GLShader Shader;
		private int Location;

		internal GLAttribute(GLShader Shader, int Location)
		{
			this.Shader = Shader;
			this.Location = Location;
		}

		public bool IsAvailable
		{
			get { return this.Location < 0; }
		}

		public int Size
		{
			get
			{
				int Out;
				GL.glGetVertexAttribiv((uint)Location, GL.GL_VERTEX_ATTRIB_ARRAY_SIZE, &Out);
				return Out;
			}
		}

		private void Enable()
		{
			GL.glEnableVertexAttribArray((uint)Location);
		}

		private void Disable()
		{
			GL.glDisableVertexAttribArray((uint)Location);
		}

		public void SetData<TType>(GLBuffer Buffer, int ElementSize = 4, int Offset = 0, int Stride = 0, bool Normalize = false)
		{
			if (!Shader.IsUsing) throw(new Exception("Not using shader"));
			int GlType = GL.GL_FLOAT;
			var Type = typeof(TType);
			if (Type == typeof(float)) GlType = GL.GL_FLOAT;
			else if (Type == typeof(short)) GlType = GL.GL_SHORT;
			else if (Type == typeof(ushort)) GlType = GL.GL_UNSIGNED_SHORT;
			else if (Type == typeof(sbyte)) GlType = GL.GL_BYTE;
			else if (Type == typeof(byte)) GlType = GL.GL_UNSIGNED_BYTE;
			else throw(new Exception("Invalid type " + Type));

			Buffer.Bind();
			GL.glVertexAttribPointer(
				(uint)Location,
				ElementSize,
				GlType,
				Normalize,
				Stride,
				(void*)Offset
			);
			Enable();
		}

		//public void SetPointer(float* Data)
		//{
		//	if (Data == null)
		//	{
		//		Disable();
		//	}
		//	else
		//	{
		//		GL.glVertexAttribPointer(
		//			(uint)Location,
		//			Size,
		//			GL.GL_FLOAT,
		//			false,
		//			0,
		//			Data
		//		);
		//		Enable();
		//	}
		//}

		public override string ToString()
		{
			return String.Format("GLAttribute({0}, Size:{1})", Location, Size);
		}
	}
}
