﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpPlatform.GL.Utils
{
	abstract unsafe public class GLUniformAttribute
	{
		protected GLShader Shader;
		protected string Name;
		protected int Location;
		public int ArrayLength { get; private set; }
		protected GLValueType ValueType;

		public unsafe GLUniformAttribute(GLShader Shader, string Name, int Location, int ArrayLength, GLValueType ValueType)
		{
			this.Shader = Shader;
			this.Name = Name;
			this.Location = Location;
			this.ArrayLength = ArrayLength;
			this.ValueType = ValueType;
		}

		public bool IsAvailable
		{
			get { return this.Location >= 0; }
		}

		[DebuggerHidden]
		protected void CheckAvailable()
		{
			if (!Shader.IsUsing) throw (new Exception("Not using shader"));
		}

	}

	sealed unsafe public class GLUniform : GLUniformAttribute
	{
		public unsafe GLUniform(GLShader Shader, string Name, int Location, int ArrayLength, GLValueType ValueType)
			: base(Shader, Name, Location, ArrayLength, ValueType)
		{
		}

		[DebuggerHidden]
		public void Set(int Value)
		{
			CheckAvailable();
			GL.glUniform1i(Location, Value);
		}

		[DebuggerHidden]
		public void Set(GLTextureUnit GLTextureUnit)
		{
			GLTextureUnit.MakeCurrent();
			if (this.ValueType != GLValueType.GL_SAMPLER_2D) throw(new Exception(String.Format("Trying to bind a TextureUnit to something not a Sampler2D : {0}", ValueType)));
			Set(GLTextureUnit.Index);
		}

		[DebuggerHidden]
		public void Set(Matrix4f Matrix)
		{
			Set(new[] { Matrix });
		}

		[DebuggerHidden]
		public void Set(Matrix4f[] Matrices)
		{
			if (this.ValueType != GLValueType.GL_FLOAT_MAT4) throw (new InvalidOperationException("this.ValueType != GLValueType.GL_FLOAT_MAT4"));
			if (this.ArrayLength != Matrices.Length) throw (new InvalidOperationException("this.ArrayLength != Matrices.Length"));
			CheckAvailable();
			Matrices[0].FixValues((Pointer) =>
			{
				GL.glUniformMatrix4fv(Location, Matrices.Length, false, Pointer);
			});
		}

		public override string ToString()
		{
			return String.Format("GLUniform('{0}'({1}), {2}[{3}])", Name, Location, ValueType, ArrayLength);
		}
	}

	sealed unsafe public class GLAttribute : GLUniformAttribute
	{
		public unsafe GLAttribute(GLShader Shader, string Name, int Location, int ArrayLength, GLValueType ValueType)
			: base(Shader, Name, Location, ArrayLength, ValueType)
		{
		}

		private void Enable()
		{
			GL.glEnableVertexAttribArray((uint)Location);
		}

		private void Disable()
		{
			GL.glDisableVertexAttribArray((uint)Location);
		}

		public void UnsetData()
		{
			Disable();
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
			Buffer.Unbind();
			Enable();
		}

		//public void SetData(GLMatrix4 ModelViewProjectionMatrix)
		//{
		//	if (this.ValueType != GLValueType.GL_FLOAT_MAT4) throw (new InvalidOperationException("this.ValueType != GLValueType.GL_FLOAT_MAT4"));
		//	if (this.ArrayLength != 1) throw (new InvalidOperationException("this.ArrayLength != 1"));
		//}

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
			return String.Format("GLAttribute('{0}'({1}), {2}[{3}])", Name, Location, ValueType, ArrayLength);
		}
	}
}
