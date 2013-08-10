using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace CSharpPlatform.GL.Impl
{
	public class WGL
	{
		[SuppressUnmanagedCodeSecurity, DllImport(GL.DllWindows, ExactSpelling = true, SetLastError = true)] public extern static IntPtr wglCreateContext(IntPtr hDc);
		[SuppressUnmanagedCodeSecurity, DllImport(GL.DllWindows, ExactSpelling = true, SetLastError = true)] public extern static Boolean wglDeleteContext(IntPtr oldContext);
		[SuppressUnmanagedCodeSecurity, DllImport(GL.DllWindows, ExactSpelling = true, SetLastError = true)] public extern static IntPtr wglGetCurrentContext();
		[SuppressUnmanagedCodeSecurity, DllImport(GL.DllWindows, ExactSpelling = true, SetLastError = true)] public extern static Boolean wglMakeCurrent(IntPtr hDc, IntPtr newContext);
		[SuppressUnmanagedCodeSecurity, DllImport(GL.DllWindows, ExactSpelling = true, SetLastError = true)] public extern static Boolean wglCopyContext(IntPtr hglrcSrc, IntPtr hglrcDst, UInt32 mask);
		[SuppressUnmanagedCodeSecurity, DllImport(GL.DllWindows, ExactSpelling = true, SetLastError = true)] public extern static unsafe int wglChoosePixelFormat(IntPtr hDc, PixelFormatDescriptor* pPfd);
		[SuppressUnmanagedCodeSecurity, DllImport(GL.DllWindows, ExactSpelling = true, SetLastError = true)] public extern static unsafe int wglDescribePixelFormat(IntPtr hdc, int ipfd, UInt32 cjpfd, PixelFormatDescriptor* ppfd);
		[SuppressUnmanagedCodeSecurity, DllImport(GL.DllWindows, ExactSpelling = true, SetLastError = true)] public extern static IntPtr wglGetCurrentDC();
		[SuppressUnmanagedCodeSecurity, DllImport(GL.DllWindows, ExactSpelling = true, SetLastError = true)] public extern static IntPtr wglGetDefaultProcAddress(String lpszProc);
		[SuppressUnmanagedCodeSecurity, DllImport(GL.DllWindows, ExactSpelling = true, SetLastError = true)] public extern static IntPtr wglGetProcAddress(String lpszProc);
		[SuppressUnmanagedCodeSecurity, DllImport(GL.DllWindows, ExactSpelling = true, SetLastError = true)] public extern static int wglGetPixelFormat(IntPtr hdc);
		[SuppressUnmanagedCodeSecurity, DllImport(GL.DllWindows, ExactSpelling = true, SetLastError = true)] public extern static unsafe Boolean wglSetPixelFormat(IntPtr hdc, int ipfd, PixelFormatDescriptor* ppfd);
		[SuppressUnmanagedCodeSecurity, DllImport(GL.DllWindows, ExactSpelling = true, SetLastError = true)] public extern static Boolean wglSwapBuffers(IntPtr hdc);
		[SuppressUnmanagedCodeSecurity, DllImport(GL.DllWindows, ExactSpelling = true, SetLastError = true)] public extern static Boolean wglShareLists(IntPtr hrcSrvShare, IntPtr hrcSrvSource);
		[SuppressUnmanagedCodeSecurity, DllImport(GL.DllWindows, ExactSpelling = true, SetLastError = true)] public extern static IntPtr wglCreateLayerContext(IntPtr hDc, int level);
		//[SuppressUnmanagedCodeSecurity, DllImport(GL.DllWindows, ExactSpelling = true)] public extern static unsafe Boolean wglDescribeLayerPlane(IntPtr hDc, int pixelFormat, int layerPlane, UInt32 nBytes, LayerPlaneDescriptor* plpd);
		[SuppressUnmanagedCodeSecurity, DllImport(GL.DllWindows, ExactSpelling = true)] public extern static unsafe int wglSetLayerPaletteEntries(IntPtr hdc, int iLayerPlane, int iStart, int cEntries, Int32* pcr);
		[SuppressUnmanagedCodeSecurity, DllImport(GL.DllWindows, ExactSpelling = true)] public extern static unsafe int wglGetLayerPaletteEntries(IntPtr hdc, int iLayerPlane, int iStart, int cEntries, Int32* pcr);
		[SuppressUnmanagedCodeSecurity, DllImport(GL.DllWindows, ExactSpelling = true)] public extern static Boolean wglRealizeLayerPalette(IntPtr hdc, int iLayerPlane, Boolean bRealize);
		[SuppressUnmanagedCodeSecurity, DllImport(GL.DllWindows, ExactSpelling = true)] public extern static Boolean wglSwapLayerBuffers(IntPtr hdc, UInt32 fuFlags);
		[SuppressUnmanagedCodeSecurity, DllImport(GL.DllWindows, CharSet = CharSet.Auto)] public extern static Boolean wglUseFontBitmapsA(IntPtr hDC, Int32 first, Int32 count, Int32 listBase);
		[SuppressUnmanagedCodeSecurity, DllImport(GL.DllWindows, CharSet = CharSet.Auto)] public extern static Boolean wglUseFontBitmapsW(IntPtr hDC, Int32 first, Int32 count, Int32 listBase);
		//[SuppressUnmanagedCodeSecurity, DllImport(GL.DllWindows, CharSet = CharSet.Auto)] public extern static unsafe Boolean wglUseFontOutlinesA(IntPtr hDC, Int32 first, Int32 count, Int32 listBase, float thickness, float deviation, Int32 fontMode, GlyphMetricsFloat* glyphMetrics);
		//[SuppressUnmanagedCodeSecurity, DllImport(GL.DllWindows, CharSet = CharSet.Auto)] public extern static unsafe Boolean wglUseFontOutlinesW(IntPtr hDC, Int32 first, Int32 count, Int32 listBase, float thickness, float deviation, Int32 fontMode, GlyphMetricsFloat* glyphMetrics);
		[SuppressUnmanagedCodeSecurity, DllImport(GL.DllWindows, ExactSpelling = true, SetLastError = true)] public extern static Boolean wglMakeContextCurrentEXT(IntPtr hDrawDC, IntPtr hReadDC, IntPtr hglrc);
		[SuppressUnmanagedCodeSecurity, DllImport(GL.DllWindows, ExactSpelling = true, SetLastError = true)] public extern static unsafe Boolean wglChoosePixelFormatEXT(IntPtr hdc, int* piAttribIList, Single* pfAttribFList, UInt32 nMaxFormats, [Out] int* piFormats, [Out] UInt32* nNumFormats);
		
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct PixelFormatDescriptor
	{
		public short Size;
		public short Version;
		public PixelFormatDescriptorFlags Flags;
		public PixelType PixelType;
		public byte ColorBits;
		public byte RedBits;
		public byte RedShift;
		public byte GreenBits;
		public byte GreenShift;
		public byte BlueBits;
		public byte BlueShift;
		public byte AlphaBits;
		public byte AlphaShift;
		public byte AccumBits;
		public byte AccumRedBits;
		public byte AccumGreenBits;
		public byte AccumBlueBits;
		public byte AccumAlphaBits;
		public byte DepthBits;
		public byte StencilBits;
		public byte AuxBuffers;
		public byte LayerType;
		private byte Reserved;
		public int LayerMask;
		public int VisibleMask;
		public int DamageMask;
	}

	public enum PixelType : byte
	{
		RGBA = 0,
		INDEXED = 1
	}

	[Flags]
	public enum PixelFormatDescriptorFlags : int
	{
		// PixelFormatDescriptor flags
		DOUBLEBUFFER = 0x01,
		STEREO = 0x02,
		DRAW_TO_WINDOW = 0x04,
		DRAW_TO_BITMAP = 0x08,
		SUPPORT_GDI = 0x10,
		SUPPORT_OPENGL = 0x20,
		GENERIC_FORMAT = 0x40,
		NEED_PALETTE = 0x80,
		NEED_SYSTEM_PALETTE = 0x100,
		SWAP_EXCHANGE = 0x200,
		SWAP_COPY = 0x400,
		SWAP_LAYER_BUFFERS = 0x800,
		GENERIC_ACCELERATED = 0x1000,
		SUPPORT_DIRECTDRAW = 0x2000,
		SUPPORT_COMPOSITION = 0x8000,

		// PixelFormatDescriptor flags for use in ChoosePixelFormat only
		DEPTH_DONTCARE = unchecked((int)0x20000000),
		DOUBLEBUFFER_DONTCARE = unchecked((int)0x40000000),
		STEREO_DONTCARE = unchecked((int)0x80000000)
	}
}
