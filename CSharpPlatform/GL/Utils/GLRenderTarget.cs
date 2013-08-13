using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpPlatform.GL.Utils
{
	unsafe public class GLRenderBuffer : IDisposable
	{
		public readonly int Width, Height;
		public uint Index { get { return _Index; } }
		private uint _Index;

		public GLRenderBuffer(int Width, int Height, int Format)
		{
			this.Width = Width;
			this.Height = Height;
			fixed (uint* IndexPtr = &_Index)
			{
				GL.glGenRenderbuffers(1, IndexPtr);
				GL.glBindRenderbuffer(GL.GL_RENDERBUFFER, _Index);
				GL.glRenderbufferStorage(GL.GL_RENDERBUFFER, Format, Width, Height);
			}

		}

		public void Dispose()
		{
			fixed (uint* IndexPtr = &_Index)
			{
				GL.glDeleteRenderbuffers(1, IndexPtr);
			}
		}
	}

	unsafe public class GLRenderTarget : IDisposable
	{
		[ThreadStatic]
		public static GLRenderTarget Current;

		private uint FrameBufferId;
		public GLTexture TextureColor { get; private set; }
		public GLRenderBuffer RenderBufferDepth { get; private set; }
		public GLRenderBuffer RenderBufferStencil { get; private set; }
		private int _Width;
		private int _Height;
		const bool EnableBuffered = false;
		private GLTexture _TextureColorBuffered;
		public GLTexture TextureColorBuffered { get { return EnableBuffered ? _TextureColorBuffered : TextureColor; } }


		public int Width
		{
			get
			{
				//if (FrameBuffer == 0) return OpenglContextFactory.Current.Size.Width;
				if (FrameBufferId == 0) return 64;
				return _Width;
			}
		}

		public int Height
		{
			get
			{
				//if (FrameBuffer == 0) return OpenglContextFactory.Current.Size.Height;
				if (FrameBufferId == 0) return 64;
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
			fixed (uint* FrameBufferPtr = &FrameBufferId)
			{
				GL.glGenFramebuffers(1, FrameBufferPtr);
				TextureColor = GLTexture.Create().SetFormat(TextureFormat.RGBA).SetSize(_Width, _Height);
				RenderBufferDepth = new GLRenderBuffer(_Width, _Height, GL.GL_DEPTH_COMPONENT16);
				RenderBufferStencil = new GLRenderBuffer(_Width, _Height, GL.GL_STENCIL_INDEX8);

				if (EnableBuffered) _TextureColorBuffered = GLTexture.Create().SetFormat(TextureFormat.RGBA).SetSize(_Width, _Height);
			}
		}

		public void Dispose()
		{
			if (EnableBuffered)
			{
				fixed (uint* FrameBufferPtr = &FrameBufferId)
				{
					GL.glDeleteFramebuffers(1, FrameBufferPtr);
					TextureColor.Dispose();
					_TextureColorBuffered.Dispose();
					RenderBufferDepth.Dispose();
					RenderBufferStencil.Dispose();
				}
			}
		}

		private void Unbind()
		{
			if (EnableBuffered)
			{
				if (_TextureColorBuffered != null)
				{
					_TextureColorBuffered.BindTemp(() =>
					{
						GL.glCopyTexImage2D(GL.GL_TEXTURE_2D, 0, GL.GL_RGBA, 0, 0, Width, Height, 0);
					});
				}
			}
		}

		public GLRenderTarget Bind()
		{
			if (Current != this && Current != null)
			{
				Current.Unbind();
			}
			Current = this;
			GL.glBindFramebuffer(GL.GL_FRAMEBUFFER, FrameBufferId);
			if (FrameBufferId != 0)
			{
				GL.glFramebufferTexture2D(GL.GL_FRAMEBUFFER, GL.GL_COLOR_ATTACHMENT0, GL.GL_TEXTURE_2D, TextureColor.Texture, 0);
				GL.glFramebufferRenderbuffer(GL.GL_FRAMEBUFFER, GL.GL_DEPTH_ATTACHMENT, GL.GL_RENDERBUFFER, RenderBufferDepth.Index);
				//GL.glFramebufferRenderbuffer(GL.GL_FRAMEBUFFER, GL.GL_STENCIL_ATTACHMENT, GL.GL_RENDERBUFFER, RenderBufferStencil.Index);

				int Status = GL.glCheckFramebufferStatus(GL.GL_FRAMEBUFFER);
				if (Status != GL.GL_FRAMEBUFFER_COMPLETE)
				{
					throw (new Exception(String.Format("Unsupported FrameBuffer 0x{0:X4}", Status)));
				}
			}
			GL.glViewport(0, 0, Width, Height);
			GL.glClearColor(0, 0, 0, 1);
			GL.glClear(GL.GL_COLOR_CLEAR_VALUE | GL.GL_DEPTH_CLEAR_VALUE | GL.GL_STENCIL_CLEAR_VALUE);
			GL.glFlush();
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

		public override string ToString()
		{
			return String.Format("GLRenderTarget({0}, Size({1}x{2}))", this.FrameBufferId, this.Width, this.Height);
		}
	}
}
