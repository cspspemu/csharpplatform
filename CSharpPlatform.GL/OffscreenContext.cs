﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GLES
{
	unsafe public class OffscreenContext
	{
		const int EGL_POST_SUB_BUFFER_SUPPORTED_NV = 0x30BE;

		public readonly int Width;
		public readonly int Height;

		IntPtr display = IntPtr.Zero;
		IntPtr surface = IntPtr.Zero;
		IntPtr context = IntPtr.Zero;

		IntPtr hWnd;
		IntPtr dc;

		//[DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "GetDC", ExactSpelling = true, SetLastError = true)]
		//private static extern IntPtr GetDC(IntPtr hWnd);

		virtual protected void InitializeNativeDisplay()
		{
			var Form = new Form();
			Form.ClientSize = new Size(this.Width, this.Height);
			//var Graphics = Form.CreateGraphics();
			//Form.CreateControl();

			//Microsoft.Win32.methods
			this.hWnd = Form.Handle;

			this.dc = Form.CreateGraphics().GetHdc();

			//this.dc = GetDC(hWnd);
		}

		static public bool IsWorking
		{
			get
			{
				try
				{
					OpenglEs.LoadLibraryOnce();
					GL.eglGetError();
					//return GL.eglInitialize(IntPtr.Zero, &Major, &Minor) != 0;
					return true;
				}
				catch
				{
					return false;
				}
			}
		}

		private void Init()
		{
			int[] contextAttribs = new int[]
			{
				GL.EGL_CONTEXT_CLIENT_VERSION, 2,
				GL.EGL_NONE, GL.EGL_NONE
			};

			int[] configAttribList = new int[]
			{
				GL.EGL_RED_SIZE,       8,
				GL.EGL_GREEN_SIZE,     8,
				GL.EGL_BLUE_SIZE,      8,
				GL.EGL_ALPHA_SIZE,     GL.EGL_DONT_CARE,
				GL.EGL_DEPTH_SIZE,     16,
				GL.EGL_STENCIL_SIZE,   8,
				GL.EGL_SAMPLE_BUFFERS, 0,
				//GL.EGL_ALPHA_SIZE,     (flags & GL.ES_WINDOW_ALPHA) ? 8 : GL.EGL_DONT_CARE,
				//GL.EGL_DEPTH_SIZE,     (flags & GL.ES_WINDOW_DEPTH) ? 8 : GL.EGL_DONT_CARE,
				//GL.EGL_STENCIL_SIZE,   (flags & GL.ES_WINDOW_STENCIL) ? 8 : GL.EGL_DONT_CARE,
				//GL.EGL_SAMPLE_BUFFERS, (flags & GL.ES_WINDOW_MULTISAMPLE) ? 1 : 0,
				GL.EGL_NONE,
			};

			int[] surfaceAttribList = new int[]
			{
				EGL_POST_SUB_BUFFER_SUPPORTED_NV, 0,
				GL.EGL_NONE, GL.EGL_NONE,
			};

			InitializeNativeDisplay();

			IntPtr config = IntPtr.Zero;
			int Major, Minor;
			int numConfigs;

			display = EglExpectNotEquals(GL.EGL_NO_DISPLAY, GL.eglGetDisplay(this.dc));

			EglExpectNotEquals(false, GL.eglInitialize(display, &Major, &Minor));
			//Assert.AreEqual(1, Major);
			//Assert.AreEqual(4, Minor);

			EglExpectNotEquals(false, GL.eglGetConfigs(display, null, 0, &numConfigs));

			fixed (int* configAttribListPtr = configAttribList)
			{
				EglExpectNotEquals(false, GL.eglChooseConfig(display, configAttribListPtr, &config, 1, &numConfigs));
			}

			Console.WriteLine("EGL_RED_SIZE: {0}", GL.eglGetConfigAttrib(display, config, GL.EGL_RED_SIZE));
			Console.WriteLine("EGL_GREEN_SIZE: {0}", GL.eglGetConfigAttrib(display, config, GL.EGL_GREEN_SIZE));
			Console.WriteLine("EGL_BLUE_SIZE: {0}", GL.eglGetConfigAttrib(display, config, GL.EGL_BLUE_SIZE));
			Console.WriteLine("EGL_ALPHA_SIZE: {0}", GL.eglGetConfigAttrib(display, config, GL.EGL_ALPHA_SIZE));
			Console.WriteLine("EGL_DEPTH_SIZE: {0}", GL.eglGetConfigAttrib(display, config, GL.EGL_DEPTH_SIZE));
			Console.WriteLine("EGL_STENCIL_SIZE: {0}", GL.eglGetConfigAttrib(display, config, GL.EGL_STENCIL_SIZE));
			Console.WriteLine("EGL_SAMPLE_BUFFERS: {0}", GL.eglGetConfigAttrib(display, config, GL.EGL_SAMPLE_BUFFERS));

			//GL.eglCreateWindowSurface(


			fixed (int* surfaceAttribListPtr = surfaceAttribList)
			{
				surface = EglExpectNotEquals(GL.EGL_NO_SURFACE, GL.eglCreateWindowSurface(display, config, this.hWnd, surfaceAttribListPtr));
				//Console.WriteLine("eglCreateWindowSurface: {0}", GL.eglGetErrorString());
			}

			fixed (int* contextAttribsPtr = contextAttribs)
			{
				context = EglExpectNotEquals(GL.EGL_NO_CONTEXT, GL.eglCreateContext(display, config, GL.EGL_NO_CONTEXT, contextAttribsPtr));
			}
		}

		public void SetCurrent()
		{
			EglExpectNotEquals(false, GL.eglMakeCurrent(display, surface, surface, context));
		}

		public void UnsetCurrent()
		{
			//GL.eglMakeCurrent(IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);
			//GL.eglMakeCurrent(IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, context);
			//EglExpectNotEquals(0, GL.eglMakeCurrent(display, surface, surface, context));
		}

		T EglExpectEquals<T>(T notExpected, T given)
		{
			if (!Object.Equals(notExpected, given)) throw (new Exception(GL.eglGetErrorString()));
			return given;
		}

		T EglExpectNotEquals<T>(T notExpected, T given)
		{
			if (Object.Equals(notExpected, given)) throw (new Exception(GL.eglGetErrorString()));
			return given;
		}

		public OffscreenContext(int Width, int Height)
		{
			this.Width = Width;
			this.Height = Height;

			OpenglEs.LoadLibraryOnce();
			Init();
		}

		public void SwapBuffers()
		{
			GL.eglSwapBuffers(display, surface);
		}
	}
}
