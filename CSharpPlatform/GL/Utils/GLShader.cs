using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CSharpPlatform.GL.Utils
{
	unsafe public class GLShader : IDisposable
	{
		uint Program;
		uint VertexShader;
		uint FragmentShader;

		public GLShader(string VertexShaderSource, string FragmentShaderSource)
		{
			Initialize();

			ShaderSource(VertexShader, VertexShaderSource);
			GL.glCompileShader(VertexShader);
			Console.WriteLine(GetShaderInfoLog(VertexShader));

			ShaderSource(FragmentShader, FragmentShaderSource);
			GL.glCompileShader(FragmentShader);
			Console.WriteLine(GetShaderInfoLog(FragmentShader));

			GL.glAttachShader(Program, VertexShader);
			GL.glAttachShader(Program, FragmentShader);

			Link();
		}

		public GLAttribute GetAttribute(string Name)
		{
			var AttributeLocation = GL.glGetAttribLocation(Program, Name);
			//GL.glGetVertexAttribfv
			if (AttributeLocation == -1) throw(new Exception(String.Format("Can't find '{0}' attribute", Name)));
			return new GLAttribute((uint)AttributeLocation);
		}

		private void Link()
		{
			GL.glLinkProgram(Program);
		}

		public void Use()
		{
			GL.glUseProgram(Program);
		}

		private void Initialize()
		{
			Program = GL.glCreateProgram();
			VertexShader = GL.glCreateShader(GL.GL_VERTEX_SHADER);
			FragmentShader = GL.glCreateShader(GL.GL_FRAGMENT_SHADER);
		}

		static private void ShaderSource(uint Shader, string Source)
		{
			var SourceBytes = new UTF8Encoding(false, true).GetBytes(Source);
			var SourceLength = SourceBytes.Length;
			fixed (byte* _SourceBytesPtr = SourceBytes)
			{
				byte* SourceBytesPtr = _SourceBytesPtr;
				GL.glShaderSource(Shader, 1, &SourceBytesPtr, &SourceLength);
			}
		}

		static private string GetShaderInfoLog(uint Shader)
		{
			int Length;
			var Data = new byte[1024];
			fixed (byte* DataPtr = Data)
			{
				GL.glGetShaderInfoLog(Shader, Data.Length, &Length, DataPtr);
				return Marshal.PtrToStringAnsi(new IntPtr(DataPtr), Length);
			}
		}

		public void Dispose()
		{
			GL.glDeleteProgram(Program);
			GL.glDeleteShader(VertexShader);
			GL.glDeleteShader(FragmentShader);
			Program = 0;
			VertexShader = 0;
			FragmentShader = 0;
		}
	}
}
