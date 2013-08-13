using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpPlatform.GL.Impl.Android
{
	public class AndroidOpenglContext : IOpenglContext
	{
		private int Display;
		private IntPtr WindowHandle;

		public AndroidOpenglContext(IntPtr WindowHandle)
		{
			this.WindowHandle = WindowHandle;
			this.Display = EGL.eglGetDisplay(EGL.EGL_DEFAULT_DISPLAY);
			//EGL.eglCreateContext(Display);
			throw new NotImplementedException();
		}

		static public AndroidOpenglContext FromWindowHandle(IntPtr WindowHandle)
		{
			return new AndroidOpenglContext(WindowHandle);
		}

		public GLContextSize Size
		{
			get { throw new NotImplementedException(); }
		}

		public void MakeCurrent()
		{
			throw new NotImplementedException();
		}

		public void ReleaseCurrent()
		{
			throw new NotImplementedException();
		}

		public void SwapBuffers()
		{
			throw new NotImplementedException();
		}

		public void Dispose()
		{
			throw new NotImplementedException();
		}
	}
}
