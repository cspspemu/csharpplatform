using CSharpUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CSharpPlatform.GL.Utils
{
	public enum TextureFormat
	{
		UNSET = 0,
		RGBA = 1,
		DEPTH = 2,
		STENCIL = 3,
	}

	unsafe public class GLTexture : IDisposable
	{
		private bool CapturedAndMustDispose;
		private uint _Texture;
		public int Width { get; private set; }
		public int Height { get; private set; }
		private TextureFormat TextureFormat = TextureFormat.RGBA;
		private byte[] Data = null;

		public uint Texture { get { return _Texture; } }

		private GLTexture(uint _Texture)
		{
			this._Texture = _Texture;
			this.CapturedAndMustDispose = false;
			Bind();
		}

		private GLTexture()
		{
			Initialize();
		}

		static public GLTexture Create()
		{
			return new GLTexture();
		}

		public static GLTexture Wrap(uint Texture)
		{
			return new GLTexture(Texture);
		}


		private void Initialize()
		{
			fixed (uint* TexturePtr = &_Texture) GL.glGenTextures(1, TexturePtr);
			this.CapturedAndMustDispose = true;
			Bind();
		}

		public void BindTemp(Action Action)
		{
			var OldTexture = (uint)GL.glGetInteger(GL.GL_TEXTURE_BINDING_2D);
			try
			{
				Bind();
				Action();
			}
			finally
			{
				GL.glBindTexture(GL.GL_TEXTURE_2D, OldTexture);
			}
		}

		public GLTexture Bind()
		{
			//GL.glGetIntegerv(GL.GL_TEXTURE_BINDING_2D, 
			//GL.glEnable(GL.GL_TEXTURE_2D);
			GL.glBindTexture(GL.GL_TEXTURE_2D, this._Texture);
			return this;
		}

		static public void Unbind()
		{
			//GL.glDisable(GL.GL_TEXTURE_2D);
		}

		public GLTexture SetFormat(TextureFormat TextureFormat)
		{
			this.TextureFormat = TextureFormat;
			_SetTexture();
			return this;
		}

		public GLTexture SetSize(int Width, int Height)
		{
			this.Width = Width;
			this.Height = Height;
			_SetTexture();
			return this;
		}

		public GLTexture SetData(void* Pointer)
		{
			var Size = this.Width * this.Height * 4;
			this.Data = new byte[Size];
			Marshal.Copy(new IntPtr(Pointer), this.Data, 0, Size);
			_SetTexture();
			return this;
		}

		public GLTexture SetData<T>(T[] SetData)
		{
			var SetDataHandle = GCHandle.Alloc(SetData, GCHandleType.Pinned);
			try
			{
				int Size = SetData.Length * Marshal.SizeOf(typeof(T));
				this.Data = new byte[Size];
				Marshal.Copy(
					SetDataHandle.AddrOfPinnedObject(),
					this.Data,
					0,
					Size
				);
			}
			finally
			{
				SetDataHandle.Free();
			}
			_SetTexture();
			return this;
		}

		//public GLTexture Upload()
		//{
		//	_SetTexture();
		//	return this;
		//}

		private void _SetTexture()
		{
			if (TextureFormat == Utils.TextureFormat.UNSET) return;
			if (Width == 0 || Height == 0) return;

			Bind();
			fixed (byte* DataPtr = this.Data)
			{
				//Console.WriteLine("{0}:{1}: {2}x{3}: {4}", Texture, TextureFormat, Width, Height, new IntPtr(DataPtr));
				//if (this.Data != null) Console.WriteLine(String.Join(",", this.Data));
				switch (TextureFormat)
				{
					case TextureFormat.RGBA: GL.glTexImage2D(GL.GL_TEXTURE_2D, 0, 4, this.Width, this.Height, 0, GL.GL_RGBA, GL.GL_UNSIGNED_BYTE, DataPtr); break;
					case TextureFormat.DEPTH: GL.glTexImage2D(GL.GL_TEXTURE_2D, 0, GL.GL_DEPTH_COMPONENT, this.Width, this.Height, 0, GL.GL_DEPTH_COMPONENT, GL.GL_UNSIGNED_SHORT, DataPtr); break;
					//case TextureFormat.STENCIL: GL.glTexImage2D(GL.GL_TEXTURE_2D, 0, GL.GL_DEPTH_COMPONENT, this.Width, this.Height, 0, GL.GL_DEPTH_COMPONENT, GL.GL_UNSIGNED_SHORT, DataPtr); break;
					default: throw (new InvalidOperationException("Unsupported " + TextureFormat));
				}
			}
		}

		public void Dispose()
		{
			if (this.CapturedAndMustDispose)
			{
				fixed (uint* TexturePtr = &_Texture) GL.glDeleteTextures(1, TexturePtr);
			}
			_Texture = 0;
		}

		public byte[] GetData()
		{
			return this.Data;
		}
	}
}
