using CSharpPlatform.GL.Impl;
using CSharpPlatform.GL.Impl.Android;
using CSharpPlatform.GL.Impl.Linux;
using CSPspEmu.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpPlatform.GL
{
	public class OpenglContextFactory
	{
		[ThreadStatic]
		public static IOpenglContext Current;

		static public IOpenglContext CreateWindowless()
		{
			return CreateFromWindowHandle(IntPtr.Zero);
		}

		static public IOpenglContext CreateFromWindowHandle(IntPtr WindowHandle)
		{
			switch (Platform.OS)
			{
				case OS.Windows: return WinOpenglContext.FromWindowHandle(WindowHandle);
				case OS.Linux: return LinuxOpenglContext.FromWindowHandle(WindowHandle);
				case OS.Android: return AndroidOpenglContext.FromWindowHandle(WindowHandle);
				default: throw (new NotImplementedException(String.Format("Not implemented OS: {0}", Platform.OS)));
			}
		}
	}
}
