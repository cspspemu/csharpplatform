using CSharpPlatform.GL.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpPlatform.GL
{
	public class OpenglContextFactory
	{
		//[ThreadStatic]
		//public static IOpenglContext Current;

		static public IOpenglContext CreateWindowless()
		{
			return CreateFromDeviceContext(IntPtr.Zero);
		}

		static public IOpenglContext CreateFromDeviceContext(IntPtr DeviceContext)
		{
			return WinOpenglContext.FromDeviceContext(DeviceContext);
		}

		static public IOpenglContext CreateFromWindowHandle(IntPtr WindowHandle)
		{
			return WinOpenglContext.FromWindowHandle(WindowHandle);
		}
	}
}
