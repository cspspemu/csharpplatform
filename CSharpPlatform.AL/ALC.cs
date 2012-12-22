using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ALCdevice = System.IntPtr;
using ALCcontext = System.IntPtr;

using ALCboolean = System.Boolean;
using ALCchar = System.Byte;

using ALCbyte = System.SByte;
using ALCubyte = System.Byte;

using ALCshort = System.Int16;
using ALCushort = System.UInt16;

using ALCint = System.Int32;
using ALCuint = System.UInt32;

using ALCsizei = System.Int32;
using ALCenum = System.Int32;

using ALCfloat = System.Single;
using ALCdouble = System.Double;
using System.Runtime.InteropServices;


namespace CSharpPlatform.AL
{
	unsafe public partial class AL
	{
		/*
		 * The ALCAPI, ALCAPIENTRY, and ALC_INVALID macros are deprecated, but are
		 * included for applications porting code from AL 1.0
		 */
		public const int ALC_INVALID = 0;


		public const int ALC_VERSION_0_1         = 1;

		/* Enumerant values begin at column 50. No tabs. */

		/* Boolean False. */
		public const int ALC_FALSE                                = 0;

		/* Boolean True. */
		public const int ALC_TRUE                                 = 1;

		/**
		 * followed by <int> Hz
		 */
		public const int ALC_FREQUENCY                            = 0x1007;

		/**
		 * followed by <int> Hz
		 */
		public const int ALC_REFRESH                              = 0x1008;

		/**
		 * followed by AL_TRUE, AL_FALSE
		 */
		public const int ALC_SYNC                                 = 0x1009;

		/**
		 * followed by <int> Num of requested Mono (3D) Sources
		 */
		public const int ALC_MONO_SOURCES                         = 0x1010;

		/**
		 * followed by <int> Num of requested Stereo Sources
		 */
		public const int ALC_STEREO_SOURCES                       = 0x1011;

		/**
		 * errors
		 */

		/**
		 * No error
		 */
		public const int ALC_NO_ERROR                             = ALC_FALSE;

		/**
		 * No device
		 */
		public const int ALC_INVALID_DEVICE                       = 0xA001;

		/**
		 * invalid context ID
		 */
		public const int ALC_INVALID_CONTEXT                      = 0xA002;

		/**
		 * bad enum
		 */
		public const int ALC_INVALID_ENUM                         = 0xA003;

		/**
		 * bad value
		 */
		public const int ALC_INVALID_VALUE                        = 0xA004;

		/**
		 * Out of memory.
		 */
		public const int ALC_OUT_OF_MEMORY                        = 0xA005;


		/**
		 * The Specifier string for default device
		 */
		public const int ALC_DEFAULT_DEVICE_SPECIFIER             = 0x1004;
		public const int ALC_DEVICE_SPECIFIER                     = 0x1005;
		public const int ALC_EXTENSIONS                           = 0x1006;

		public const int ALC_MAJOR_VERSION                        = 0x1000;
		public const int ALC_MINOR_VERSION                        = 0x1001;

		public const int ALC_ATTRIBUTES_SIZE                      = 0x1002;
		public const int ALC_ALL_ATTRIBUTES                       = 0x1003;

		/**
		 * Capture extension
		 */
		public const int ALC_CAPTURE_DEVICE_SPECIFIER             = 0x310;
		public const int ALC_CAPTURE_DEFAULT_DEVICE_SPECIFIER     = 0x311;
		public const int ALC_CAPTURE_SAMPLES                      = 0x312;


		/*
		 * Context Management
		 */
		[DllImport(DLL)] static public extern ALCcontext *    alcCreateContext( ALCdevice *device, ALCint* attrlist );
		[DllImport(DLL)] static public extern ALCboolean      alcMakeContextCurrent( ALCcontext *context );
		[DllImport(DLL)] static public extern void            alcProcessContext( ALCcontext *context );
		[DllImport(DLL)] static public extern void            alcSuspendContext( ALCcontext *context );
		[DllImport(DLL)] static public extern void            alcDestroyContext( ALCcontext *context );
		[DllImport(DLL)] static public extern ALCcontext *    alcGetCurrentContext( );
		[DllImport(DLL)] static public extern ALCdevice*      alcGetContextsDevice( ALCcontext *context );


		/*
		 * Device Management
		 */
		[DllImport(DLL)] static public extern ALCdevice *     alcOpenDevice( ALCchar *devicename );
		[DllImport(DLL)] static public extern ALCboolean      alcCloseDevice( ALCdevice *device );


		/*
		 * Error support.
		 * Obtain the most recent Context error
		 */
		[DllImport(DLL)] static public extern ALCenum         alcGetError( ALCdevice *device );


		/* 
		 * Extension support.
		 * Query for the presence of an extension, and obtain any appropriate
		 * function pointers and enum values.
		 */
		[DllImport(DLL)] static public extern ALCboolean      alcIsExtensionPresent( ALCdevice *device, ALCchar *extname );
		[DllImport(DLL)] static public extern void  *         alcGetProcAddress( ALCdevice *device, ALCchar *funcname );
		[DllImport(DLL)] static public extern ALCenum         alcGetEnumValue( ALCdevice *device, ALCchar *enumname );


		/*
		 * Query functions
		 */
		[DllImport(DLL)] static public extern ALCchar * alcGetString( ALCdevice *device, ALCenum param );
		[DllImport(DLL)] static public extern void            alcGetIntegerv( ALCdevice *device, ALCenum param, ALCsizei size, ALCint *data );


		/*
		 * Capture functions
		 */
		[DllImport(DLL)] static public extern ALCdevice*      alcCaptureOpenDevice( ALCchar *devicename, ALCuint frequency, ALCenum format, ALCsizei buffersize );
		[DllImport(DLL)] static public extern ALCboolean      alcCaptureCloseDevice( ALCdevice *device );
		[DllImport(DLL)] static public extern void            alcCaptureStart( ALCdevice *device );
		[DllImport(DLL)] static public extern void            alcCaptureStop( ALCdevice *device );
		[DllImport(DLL)] static public extern void            alcCaptureSamples( ALCdevice *device, void *buffer, ALCsizei samples );
	}
}
