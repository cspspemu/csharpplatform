using CSharpPlatform.Library;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace CSharpPlatform.GL.Impl
{
	unsafe public class WinOpenglContext : IOpenglContext
	{
		IntPtr DC;
		IntPtr Context;

		[DllImport("User32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr GetWindowDC(IntPtr hWnd);

		[DllImport("User32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr GetDC(IntPtr hWnd);

		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool ReleaseDC(IntPtr hwnd, IntPtr DC);

		[DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
		internal static extern ushort RegisterClassEx(ref ExtendedWindowClass window_class);

		[DllImport("User32.dll", CharSet = CharSet.Auto)]
		public extern static IntPtr DefWindowProc(IntPtr hWnd, WindowMessage msg, IntPtr wParam, IntPtr lParam);

		[DllImport("user32.dll")]
		public static extern IntPtr LoadCursor(IntPtr hInstance, IntPtr lpCursorName);

		[DllImport("user32.dll", EntryPoint = "AdjustWindowRectEx", CallingConvention = CallingConvention.StdCall, SetLastError = true), SuppressUnmanagedCodeSecurity]
		internal static extern bool AdjustWindowRectEx(ref RECT lpRect, WindowStyle dwStyle, bool bMenu, ExtendedWindowStyle dwExStyle);

		[DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
		internal static extern IntPtr CreateWindowEx(
			ExtendedWindowStyle ExStyle,
			IntPtr ClassAtom,
			IntPtr WindowName,
			WindowStyle Style,
			int X, int Y,
			int Width, int Height,
			IntPtr HandleToParentWindow,
			IntPtr Menu,
			IntPtr Instance,
			IntPtr Param
		);

		[DllImport("Gdi32.dll")]
		internal static extern IntPtr GetCurrentObject(
			IntPtr hdc,
			uint uObjectType
		);

		[DllImport("Gdi32.dll")]
		internal static extern int GetObject(
			IntPtr hgdiobj,
			int cbBuffer,
			void* lpvObject
		);

		const ClassStyle DefaultClassStyle = ClassStyle.OwnDC;

		private static bool class_registered = false;

		static readonly IntPtr Instance = Marshal.GetHINSTANCE(typeof(WinOpenglContext).Module);
		static readonly IntPtr ClassName = Marshal.StringToHGlobalAuto(Guid.NewGuid().ToString());
		const ExtendedWindowStyle ParentStyleEx = ExtendedWindowStyle.WindowEdge | ExtendedWindowStyle.ApplicationWindow;

		static private void RegisterClassOnce()
		{
			if (!class_registered)
			{
				ExtendedWindowClass wc = new ExtendedWindowClass();
				wc.Size = ExtendedWindowClass.SizeInBytes;
				wc.Style = DefaultClassStyle;
				wc.Instance = Instance;
				wc.WndProc = WindowProcedure;
				wc.ClassName = ClassName;
				wc.Icon = IntPtr.Zero;
				wc.IconSm = IntPtr.Zero;
				wc.Cursor = LoadCursor(IntPtr.Zero, (IntPtr)CursorName.Arrow);
				ushort atom = RegisterClassEx(ref wc);

				if (atom == 0)
					throw new Exception(String.Format("Failed to register window class. Error: {0}", Marshal.GetLastWin32Error()));

				class_registered = true;
			}
		}

		static IntPtr WindowProcedure(IntPtr handle, WindowMessage message, IntPtr wParam, IntPtr lParam)
		{
			return DefWindowProc(handle, message, wParam, lParam);
		}

		[SuppressUnmanagedCodeSecurity, DllImport("GDI32.dll", ExactSpelling = true, SetLastError = true)]
		public extern static unsafe int ChoosePixelFormat(IntPtr hDc, PixelFormatDescriptor* pPfd);
		[SuppressUnmanagedCodeSecurity, DllImport("GDI32.dll", ExactSpelling = true, SetLastError = true)]
		public extern static unsafe Boolean SetPixelFormat(IntPtr hdc, int ipfd, PixelFormatDescriptor* ppfd);

		static IntPtr SharedContext;

		static public WinOpenglContext FromWindowHandle(IntPtr WindowHandle)
		{
			return FromDeviceContext(GetDC(WindowHandle));
		}

		static public WinOpenglContext FromDeviceContext(IntPtr DC)
		{
			return new WinOpenglContext(DC);
		}

		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern bool AdjustWindowRectEx(ref RECT lpRect, int dwStyle, bool bMenu, int dwExStyle);

		private WinOpenglContext(IntPtr DC)
		{
			RegisterClassOnce();
			int Width = 64;
			int Height = 64;

			if (DC == IntPtr.Zero)
			{
				
				WindowStyle style = WindowStyle.OverlappedWindow | WindowStyle.ClipChildren | WindowStyle.ClipSiblings;
				ExtendedWindowStyle ex_style = ParentStyleEx;

				var rect = new RECT()
				{
					left = 0,
					top = 0,
					right = Width,
					bottom = Height,
				};
				AdjustWindowRectEx(ref rect, style, false, ex_style);

				IntPtr window_name = Marshal.StringToHGlobalAuto("Title");
				IntPtr hWnd = CreateWindowEx(
					ex_style, ClassName, window_name, style,
					rect.left, rect.top, rect.right - rect.left, rect.bottom - rect.top,
					IntPtr.Zero, IntPtr.Zero, Instance, IntPtr.Zero
				);

				if (hWnd == IntPtr.Zero)
				{
					throw new Exception(String.Format("Failed to create window. Error: {0}", Marshal.GetLastWin32Error()));
				}

				//return handle;
				//
				//var Form = new Form();
				DC = GetDC(hWnd);
			}

			var pfd = new PixelFormatDescriptor();
			pfd.Size = (short)sizeof(PixelFormatDescriptor);
			pfd.Version = 1;
			pfd.Flags = PixelFormatDescriptorFlags.DRAW_TO_WINDOW | PixelFormatDescriptorFlags.SUPPORT_OPENGL | PixelFormatDescriptorFlags.DOUBLEBUFFER | PixelFormatDescriptorFlags.SUPPORT_COMPOSITION;
			pfd.LayerType = 0;
			pfd.PixelType = PixelType.RGBA; // PFD_TYPE_RGBA
			pfd.ColorBits = 24;
			pfd.DepthBits = 16;
			pfd.StencilBits = 8;

			var pf = ChoosePixelFormat(DC, &pfd);

			if (!SetPixelFormat(DC, pf, &pfd))
			{
				Console.WriteLine("Error SetPixelFormat failed.");
			}

			this.DC = DC;

			this.Context = WGL.wglCreateContext(DC);
			if (SharedContext != IntPtr.Zero)
			{
				//Console.WriteLine("SharedContext!"); Console.ReadKey();
				if (!WGL.wglShareLists(SharedContext, this.Context))
				{
					throw(new InvalidOperationException("Can't share lists"));
				}
			}
			MakeCurrent();
			DynamicLibraryFactory.MapLibraryToType<Extension>(new DynamicLibraryOpengl());
			GL.LoadAllOnce();

#if false
			if (Extension.wglCreateContextAttribsARB != null)
			{
				ReleaseCurrent();
				WGL.wglDeleteContext(this.Context);
				fixed (int* AttribListPtr = new int[] { (int)ArbCreateContext.MajorVersion, 3, (int)ArbCreateContext.MinorVersion, 1, 0, 0 })
				{
					this.Context = Extension.wglCreateContextAttribsARB(DC, SharedContext, AttribListPtr);
				}
				if (this.Context == IntPtr.Zero) throw(new Exception("Error creating context"));
				MakeCurrent();

				Console.WriteLine("OpenGL Version: {0}", Marshal.PtrToStringAnsi(new IntPtr(GL.glGetString(GL.GL_VERSION))));
				//Console.ReadKey();
			}
#endif

			if (SharedContext == IntPtr.Zero)
			{
				SharedContext = this.Context;
			}

			if (Extension.wglSwapIntervalEXT != null)
			{
				Extension.wglSwapIntervalEXT(0);
			}

			//RECT clientRect;
			//GetClientRect(hWnd, &clientRect);
		}

		public GLContextSize Size
		{
			get
			{
				var bitmapHeader = default(BITMAP);
				var hBitmap = GetCurrentObject(DC, 7);
				GetObject(hBitmap, sizeof(BITMAP), &bitmapHeader);
				return new GLContextSize() { Width = (int)bitmapHeader.bmWidth, Height = (int)bitmapHeader.bmHeight };
			}
		}

		public enum ArbCreateContext : int
		{
			DebugBit = 0x0001,
			ForwardCompatibleBit = 0x0002,
			MajorVersion = 0x2091,
			MinorVersion = 0x2092,
			LayerPlane = 0x2093,
			Flags = 0x2094,
			ErrorInvalidVersion = 0x2095,
		}


		public class Extension
		{
			public static readonly wglCreateContextAttribsARB wglCreateContextAttribsARB;
			public static readonly wglSwapIntervalEXT wglSwapIntervalEXT;
			public static readonly wglGetSwapIntervalEXT wglGetSwapIntervalEXT;
		}

		public delegate IntPtr wglCreateContextAttribsARB(IntPtr hDC, IntPtr hShareContext, int* attribList);
		public delegate Boolean wglSwapIntervalEXT(int interval);
		public delegate int wglGetSwapIntervalEXT();

		public void MakeCurrent()
		{
			if (!WGL.wglMakeCurrent(DC, Context))
			{
				throw(new Exception("Can't MakeCurrent"));
			}
			//OpenglContextFactory.Current = this;
		}

		public void ReleaseCurrent()
		{
			if (!WGL.wglMakeCurrent(DC, IntPtr.Zero))
			{
				throw (new Exception("Can't MakeCurrent"));
			}
			//OpenglContextFactory.Current = null;
		}

		public void SwapBuffers()
		{
			WGL.wglSwapBuffers(DC);
		}

		public void Dispose()
		{
			WGL.wglDeleteContext(this.Context);
			this.Context = IntPtr.Zero;
			//throw new NotImplementedException();
		}
	}

	[DebuggerDisplay("{Width}x{Height}")]
	public struct GLContextSize
	{
		public int Width;
		public int Height;
	}

	public struct RECT
	{
		public int left;
		public int top;
		public int right;
		public int bottom;
	}

	unsafe public struct BITMAP
	{
		public uint bmType;
		public uint bmWidth;
		public uint bmHeight;
		public uint bmWidthBytes;
		public uint bmPlanes;
		public uint bmBitsPixel;
		public void* bmBits;
	}

	[Flags]
	public enum WindowStyle : uint
	{
		Overlapped = 0x00000000,
		Popup = 0x80000000,
		Child = 0x40000000,
		Minimize = 0x20000000,
		Visible = 0x10000000,
		Disabled = 0x08000000,
		ClipSiblings = 0x04000000,
		ClipChildren = 0x02000000,
		Maximize = 0x01000000,
		Caption = 0x00C00000,    // Border | DialogFrame
		Border = 0x00800000,
		DialogFrame = 0x00400000,
		VScroll = 0x00200000,
		HScreen = 0x00100000,
		SystemMenu = 0x00080000,
		ThickFrame = 0x00040000,
		Group = 0x00020000,
		TabStop = 0x00010000,

		MinimizeBox = 0x00020000,
		MaximizeBox = 0x00010000,

		Tiled = Overlapped,
		Iconic = Minimize,
		SizeBox = ThickFrame,
		TiledWindow = OverlappedWindow,

		// Common window styles:
		OverlappedWindow = Overlapped | Caption | SystemMenu | ThickFrame | MinimizeBox | MaximizeBox,
		PopupWindow = Popup | Border | SystemMenu,
		ChildWindow = Child
	}

	[Flags]
	public enum ExtendedWindowStyle : uint
	{
		DialogModalFrame = 0x00000001,
		NoParentNotify = 0x00000004,
		Topmost = 0x00000008,
		AcceptFiles = 0x00000010,
		Transparent = 0x00000020,

		// #if(WINVER >= 0x0400)
		MdiChild = 0x00000040,
		ToolWindow = 0x00000080,
		WindowEdge = 0x00000100,
		ClientEdge = 0x00000200,
		ContextHelp = 0x00000400,
		// #endif

		// #if(WINVER >= 0x0400)
		Right = 0x00001000,
		Left = 0x00000000,
		RightToLeftReading = 0x00002000,
		LeftToRightReading = 0x00000000,
		LeftScrollbar = 0x00004000,
		RightScrollbar = 0x00000000,

		ControlParent = 0x00010000,
		StaticEdge = 0x00020000,
		ApplicationWindow = 0x00040000,

		OverlappedWindow = WindowEdge | ClientEdge,
		PaletteWindow = WindowEdge | ToolWindow | Topmost,
		// #endif

		// #if(_WIN32_WINNT >= 0x0500)
		Layered = 0x00080000,
		// #endif

		// #if(WINVER >= 0x0500)
		NoInheritLayout = 0x00100000, // Disable inheritence of mirroring by children
		RightToLeftLayout = 0x00400000, // Right to left mirroring
		// #endif /* WINVER >= 0x0500 */

		// #if(_WIN32_WINNT >= 0x0501)
		Composited = 0x02000000,
		// #endif /* _WIN32_WINNT >= 0x0501 */

		// #if(_WIN32_WINNT >= 0x0500)
		NoActivate = 0x08000000
		// #endif /* _WIN32_WINNT >= 0x0500 */
	}

	public enum WindowMessage : uint
	{
	}

	public enum CursorName : int
	{
		Arrow = 32512
	}

	[Flags]
	public enum ClassStyle
	{
		//None            = 0x0000,
		VRedraw = 0x0001,
		HRedraw = 0x0002,
		DoubleClicks = 0x0008,
		OwnDC = 0x0020,
		ClassDC = 0x0040,
		ParentDC = 0x0080,
		NoClose = 0x0200,
		SaveBits = 0x0800,
		ByteAlignClient = 0x1000,
		ByteAlignWindow = 0x2000,
		GlobalClass = 0x4000,

		Ime = 0x00010000,

		// #if(_WIN32_WINNT >= 0x0501)
		DropShadow = 0x00020000
		// #endif /* _WIN32_WINNT >= 0x0501 */
	}

	public delegate IntPtr WindowProcedure(IntPtr handle, WindowMessage message, IntPtr wParam, IntPtr lParam);

	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
	public struct ExtendedWindowClass
	{
		public uint Size;
		public ClassStyle Style;
		//public WNDPROC WndProc;
		[MarshalAs(UnmanagedType.FunctionPtr)]
		public WindowProcedure WndProc;
		public int cbClsExtra;
		public int cbWndExtra;
		public IntPtr Instance;
		public IntPtr Icon;
		public IntPtr Cursor;
		public IntPtr Background;
		public IntPtr MenuName;
		public IntPtr ClassName;
		public IntPtr IconSm;

		public static uint SizeInBytes = (uint)Marshal.SizeOf(default(ExtendedWindowClass));
	}

	public enum WindowClass : uint
	{
		Alert = 1,             /* "I need your attention now."*/
		MovableAlert = 2,             /* "I need your attention now, but I'm kind enough to let you switch out of this app to do other things."*/
		Modal = 3,             /* system modal, not draggable*/
		MovableModal = 4,             /* application modal, draggable*/
		Floating = 5,             /* floats above all other application windows*/
		Document = 6,             /* document windows*/
		Desktop = 7,             /* desktop window (usually only one of these exists) - OS X only in CarbonLib 1.0*/
		Utility = 8,             /* Available in CarbonLib 1.1 and later, and in Mac OS X*/
		Help = 10,            /* Available in CarbonLib 1.1 and later, and in Mac OS X*/
		Sheet = 11,            /* Available in CarbonLib 1.3 and later, and in Mac OS X*/
		Toolbar = 12,            /* Available in CarbonLib 1.1 and later, and in Mac OS X*/
		Plain = 13,            /* Available in CarbonLib 1.2.5 and later, and Mac OS X*/
		Overlay = 14,            /* Available in Mac OS X*/
		SheetAlert = 15,            /* Available in CarbonLib 1.3 and later, and in Mac OS X 10.1 and later*/
		AltPlain = 16,            /* Available in CarbonLib 1.3 and later, and in Mac OS X 10.1 and later*/
		Drawer = 20,            /* Available in Mac OS X 10.2 or later*/
		All = 0xFFFFFFFFu    /* for use with GetFrontWindowOfClass, FindWindowOfClass, GetNextWindowOfClass*/
	}
}
