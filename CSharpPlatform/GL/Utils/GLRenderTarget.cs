using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpPlatform.GL.Utils
{
	unsafe public class GLRenderTarget : IDisposable
	{
		private uint FrameBuffer;
		public GLTexture TextureColor { get; private set; }
		public GLTexture TextureDepth { get; private set; }
		//private GLTexture TextureStencil;
		private int _Width;
		private int _Height;

		public int Width
		{
			get
			{
				//if (FrameBuffer == 0) return OpenglContextFactory.Current.Size.Width;
				if (FrameBuffer == 0) return 64;
				return _Width;
			}
		}

		public int Height
		{
			get
			{
				//if (FrameBuffer == 0) return OpenglContextFactory.Current.Size.Height;
				if (FrameBuffer == 0) return 64;
				return _Height;
			}
		}

		static public GLRenderTarget Default
		{
			get
			{
				var Render = new GLRenderTarget();
				Render.Bind();
				return Render;
			}
		}

		private GLRenderTarget()
		{
		}

		private GLRenderTarget(int Width, int Height)
		{
			this._Width = Width;
			this._Height = Height;
			Initialize();
			Bind();
		}

		static public GLRenderTarget Create(int Width, int Height)
		{
			return new GLRenderTarget(Width, Height);
		}

		private void Initialize()
		{
			fixed (uint* FrameBufferPtr = &FrameBuffer)
			{
				GL.glGenFramebuffers(1, FrameBufferPtr);
				TextureColor = GLTexture.Create().SetFormat(TextureFormat.RGBA).SetSize(_Width, _Height);
				TextureDepth = GLTexture.Create().SetFormat(TextureFormat.DEPTH).SetSize(_Width, _Height);
				//TextureStencil = GLTexture.Create().SetFormatStencil(Width, Height);
			}
		}

		public void Dispose()
		{
			fixed (uint* FrameBufferPtr = &FrameBuffer)
			{
				GL.glDeleteFramebuffers(1, FrameBufferPtr);
				TextureColor.Dispose();
				TextureDepth.Dispose();
				//TextureStencil.Dispose();
			}
		}

		public GLRenderTarget Bind()
		{
			GL.glBindFramebuffer(GL.GL_FRAMEBUFFER, FrameBuffer);
			if (FrameBuffer != 0)
			{
				GL.glFramebufferTexture2D(GL.GL_FRAMEBUFFER, GL.GL_COLOR_ATTACHMENT0, GL.GL_TEXTURE_2D, TextureColor.Texture, 0);
				GL.glFramebufferTexture2D(GL.GL_FRAMEBUFFER, GL.GL_DEPTH_ATTACHMENT, GL.GL_TEXTURE_2D, TextureDepth.Texture, 0);
				//GL.glFramebufferTexture2D(GL.GL_FRAMEBUFFER, GL.GL_STENCIL_ATTACHMENT, GL.GL_TEXTURE_2D, TextureStencil.Texture, 0);
			}
			GL.glViewport(0, 0, Width, Height);
			return this;
		}

		public byte[] ReadPixels()
		{
			var Data = new byte[Width * Height * 4];
			fixed (byte* DataPtr = Data)
			{
				GL.glReadPixels(0, 0, Width, Height, GL.GL_RGBA, GL.GL_UNSIGNED_BYTE, DataPtr);
			}
			return Data;
		}
	}
}
