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
		private uint TextureColor;
		private uint TextureDepth;
		private uint TextureStencil;
		private int Width;
		private int Height;

		static public readonly GLRenderTarget Default = new GLRenderTarget()
		{
			Width = 512,
			Height = 272,
		};

		private GLRenderTarget()
		{
		}

		private GLRenderTarget(int Width, int Height)
		{
			this.Width = Width;
			this.Height = Height;
			Initialize();

			GL.glBindTexture(GL.GL_TEXTURE_2D, TextureColor);
			GL.glTexParameteri(GL.GL_TEXTURE_2D, GL.GL_TEXTURE_MIN_FILTER, GL.GL_LINEAR_MIPMAP_LINEAR);
			GL.glTexParameteri(GL.GL_TEXTURE_2D, GL.GL_TEXTURE_MAG_FILTER, GL.GL_LINEAR);
			GL.glTexParameteri(GL.GL_TEXTURE_2D, GL.GL_TEXTURE_WRAP_S, GL.GL_CLAMP_TO_EDGE);
			GL.glTexParameteri(GL.GL_TEXTURE_2D, GL.GL_TEXTURE_WRAP_T, GL.GL_CLAMP_TO_EDGE);
			GL.glTexImage2D(GL.GL_TEXTURE_2D, 0, GL.GL_RGBA, Width, Height, 0, GL.GL_RGBA, GL.GL_UNSIGNED_BYTE, null);

			GL.glBindTexture(GL.GL_TEXTURE_2D, TextureDepth);
			GL.glTexParameteri(GL.GL_TEXTURE_2D, GL.GL_TEXTURE_MIN_FILTER, GL.GL_LINEAR_MIPMAP_LINEAR);
			GL.glTexParameteri(GL.GL_TEXTURE_2D, GL.GL_TEXTURE_MAG_FILTER, GL.GL_LINEAR);
			GL.glTexParameteri(GL.GL_TEXTURE_2D, GL.GL_TEXTURE_WRAP_S, GL.GL_CLAMP_TO_EDGE);
			GL.glTexParameteri(GL.GL_TEXTURE_2D, GL.GL_TEXTURE_WRAP_T, GL.GL_CLAMP_TO_EDGE);
			GL.glTexImage2D(GL.GL_TEXTURE_2D, 0, GL.GL_DEPTH_COMPONENT, Width, Height, 0, GL.GL_DEPTH_COMPONENT, GL.GL_UNSIGNED_SHORT, null);
		}

		static public GLRenderTarget Create(int Width, int Height)
		{
			return new GLRenderTarget(Width, Height);
		}

		private void Initialize()
		{
			fixed (uint* FrameBufferPtr = &FrameBuffer)
			fixed (uint* TextureColorPtr = &TextureColor)
			fixed (uint* TextureDepthPtr = &TextureDepth)
			fixed (uint* TextureStencilPtr = &TextureStencil)
			{
				GL.glGenFramebuffers(1, FrameBufferPtr);
				GL.glGenTextures(1, TextureColorPtr);
				GL.glGenTextures(1, TextureDepthPtr);
				GL.glGenTextures(1, TextureStencilPtr);
			}
		}

		public void Dispose()
		{
			fixed (uint* FrameBufferPtr = &FrameBuffer)
			fixed (uint* TextureColorPtr = &TextureColor)
			fixed (uint* TextureDepthPtr = &TextureDepth)
			fixed (uint* TextureStencilPtr = &TextureStencil)
			{
				GL.glDeleteFramebuffers(1, FrameBufferPtr);
				GL.glDeleteTextures(1, TextureColorPtr);
				GL.glDeleteTextures(1, TextureDepthPtr);
				GL.glDeleteTextures(1, TextureStencilPtr);
				FrameBuffer = 0;
				TextureColor = 0;
				TextureDepth = 0;
				TextureStencil = 0;
			}
		}

		public GLRenderTarget Bind()
		{
			if (FrameBuffer != 0)
			{
				GL.glBindFramebuffer(GL.GL_FRAMEBUFFER, FrameBuffer);
				GL.glFramebufferTexture2D(GL.GL_FRAMEBUFFER, GL.GL_COLOR_ATTACHMENT0, GL.GL_TEXTURE_2D, TextureColor, 0);
				GL.glFramebufferTexture2D(GL.GL_FRAMEBUFFER, GL.GL_DEPTH_ATTACHMENT, GL.GL_TEXTURE_2D, TextureDepth, 0);
				GL.glFramebufferTexture2D(GL.GL_FRAMEBUFFER, GL.GL_STENCIL_ATTACHMENT, GL.GL_TEXTURE_2D, TextureStencil, 0);
				GL.glViewport(0, 0, Width, Height);
			}
			else
			{
				GL.glBindFramebuffer(GL.GL_FRAMEBUFFER, 0);
			}
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
