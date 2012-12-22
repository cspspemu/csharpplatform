using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

using ALboolean = System.Boolean;
using ALchar = System.Byte;

using ALbyte = System.SByte;
using ALubyte = System.Byte;

using ALshort = System.Int16;
using ALushort = System.UInt16;

using ALint = System.Int32;
using ALuint = System.UInt32;

using ALsizei = System.Int32;
using ALenum = System.Int32;

using ALfloat = System.Single;
using ALdouble = System.Double;

namespace CSharpPlatform.AL
{
	unsafe public partial class AL
	{
		const string DLL = "OpenAL32";
		/**
		 * OpenAL cross platform audio library
		 * Copyright (C) 1999-2000 by authors.
		 * This library is free software; you can redistribute it and/or
		 *  modify it under the terms of the GNU Library General Public
		 *  License as published by the Free Software Foundation; either
		 *  version 2 of the License, or (at your option) any later version.
		 *
		 * This library is distributed in the hope that it will be useful,
		 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
		 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
		 *  Library General Public License for more details.
		 *
		 * You should have received a copy of the GNU Library General Public
		 *  License along with this library; if not, write to the
		 *  Free Software Foundation, Inc., 59 Temple Place - Suite 330,
		 *  Boston, MA  02111-1307, USA.
		 * Or go to http://www.gnu.org/copyleft/lgpl.html
		 */

		/*
		 * The OPENAL, ALAPI, ALAPIENTRY, AL_INVALID, AL_ILLEGAL_ENUM, and
		 * AL_ILLEGAL_COMMAND macros are deprecated, but are included for
		 * applications porting code from AL 1.0
		 */
		public const int AL_INVALID                                = (-1);
		public const int AL_ILLEGAL_ENUM                           = AL_INVALID_ENUM;
		public const int AL_ILLEGAL_COMMAND                        = AL_INVALID_OPERATION;

		public const int AL_VERSION_1_0 = 1;
		public const int AL_VERSION_1_1 = 1;


		/* Enumerant values begin at column 50. No tabs. */

		/* "no distance model" or "no buffer" */
		public const int AL_NONE = 0;

		/* Boolean False. */
		public const int AL_FALSE = 0;

		/** Boolean True. */
		public const int AL_TRUE = 1;

		/** Indicate Source has relative coordinates. */
		public const ALenum AL_SOURCE_RELATIVE = 0x202;

		/**
		 * Directional source, inner cone angle, in degrees.
		 * Range:    [0-360] 
		 * Default:  360
		 */
		public const ALenum AL_CONE_INNER_ANGLE = 0x1001;

		/**
		 * Directional source, outer cone angle, in degrees.
		 * Range:    [0-360] 
		 * Default:  360
		 */
		public const ALenum AL_CONE_OUTER_ANGLE = 0x1002;

		/**
		 * Specify the pitch to be applied, either at source,
		 *  or on mixer results, at listener.
		 * Range:   [0.5-2.0]
		 * Default: 1.0
		 */
		public const ALenum AL_PITCH = 0x1003;
  
		/** 
		 * Specify the current location in three dimensional space.
		 * OpenAL, like OpenGL, uses a right handed coordinate system,
		 *  where in a frontal default view X (thumb) points right, 
		 *  Y points up (index finger), and Z points towards the
		 *  viewer/camera (middle finger). 
		 * To switch from a left handed coordinate system, flip the
		 *  sign on the Z coordinate.
		 * Listener position is always in the world coordinate system.
		 */
		public const ALenum AL_POSITION = 0x1004;
  
		/** Specify the current direction. */
		public const ALenum AL_DIRECTION = 0x1005;
  
		/** Specify the current velocity in three dimensional space. */
		public const ALenum AL_VELOCITY = 0x1006;

		/**
		 * Indicate whether source is looping.
		 * Type: ALboolean?
		 * Range:   [AL_TRUE, AL_FALSE]
		 * Default: FALSE.
		 */
		public const ALenum AL_LOOPING = 0x1007;

		/**
		 * Indicate the buffer to provide sound samples. 
		 * Type: ALuint.
		 * Range: any valid Buffer id.
		 */
		public const ALenum AL_BUFFER = 0x1009;
  
		/**
		 * Indicate the gain (volume amplification) applied. 
		 * Type:   ALfloat.
		 * Range:  ]0.0-  ]
		 * A value of 1.0 means un-attenuated/unchanged.
		 * Each division by 2 equals an attenuation of -6dB.
		 * Each multiplicaton with 2 equals an amplification of +6dB.
		 * A value of 0.0 is meaningless with respect to a logarithmic
		 *  scale; it is interpreted as zero volume - the channel
		 *  is effectively disabled.
		 */
		public const ALenum AL_GAIN = 0x100A;

		/*
		 * Indicate minimum source attenuation
		 * Type: ALfloat
		 * Range:  [0.0 - 1.0]
		 *
		 * Logarthmic
		 */
		public const ALenum AL_MIN_GAIN = 0x100D;

		/**
		 * Indicate maximum source attenuation
		 * Type: ALfloat
		 * Range:  [0.0 - 1.0]
		 *
		 * Logarthmic
		 */
		public const ALenum AL_MAX_GAIN = 0x100E;

		/**
		 * Indicate listener orientation.
		 *
		 * at/up 
		 */
		public const ALenum AL_ORIENTATION = 0x100F;

		/**
		 * Source state information.
		 */
		public const ALenum AL_SOURCE_STATE = 0x1010;
		public const ALenum AL_INITIAL = 0x1011;
		public const ALenum AL_PLAYING = 0x1012;
		public const ALenum AL_PAUSED = 0x1013;
		public const ALenum AL_STOPPED = 0x1014;

		/**
		 * Buffer Queue params
		 */
		public const ALenum AL_BUFFERS_QUEUED = 0x1015;
		public const ALenum AL_BUFFERS_PROCESSED = 0x1016;

		/**
		 * Source buffer position information
		 */
		public const ALenum AL_SEC_OFFSET = 0x1024;
		public const ALenum AL_SAMPLE_OFFSET = 0x1025;
		public const ALenum AL_BYTE_OFFSET = 0x1026;

		/*
		 * Source type (Static, Streaming or undetermined)
		 * Source is Static if a Buffer has been attached using AL_BUFFER
		 * Source is Streaming if one or more Buffers have been attached using alSourceQueueBuffers
		 * Source is undetermined when it has the NULL buffer attached
		 */
		public const ALenum AL_SOURCE_TYPE = 0x1027;
		public const ALenum AL_STATIC = 0x1028;
		public const ALenum AL_STREAMING = 0x1029;
		public const ALenum AL_UNDETERMINED = 0x1030;

		/** Sound samples: format specifier. */
		public const ALenum AL_FORMAT_MONO8 = 0x1100;
		public const ALenum AL_FORMAT_MONO16 = 0x1101;
		public const ALenum AL_FORMAT_STEREO8 = 0x1102;
		public const ALenum AL_FORMAT_STEREO16 = 0x1103;

		/**
		 * source specific reference distance
		 * Type: ALfloat
		 * Range:  0.0 - +inf
		 *
		 * At 0.0, no distance attenuation occurs.  Default is
		 * 1.0.
		 */
		public const ALenum AL_REFERENCE_DISTANCE = 0x1020;

		/**
		 * source specific rolloff factor
		 * Type: ALfloat
		 * Range:  0.0 - +inf
		 *
		 */
		public const ALenum AL_ROLLOFF_FACTOR = 0x1021;

		/**
		 * Directional source, outer cone gain.
		 *
		 * Default:  0.0
		 * Range:    [0.0 - 1.0]
		 * Logarithmic
		 */
		public const ALenum AL_CONE_OUTER_GAIN = 0x1022;

		/**
		 * Indicate distance above which sources are not
		 * attenuated using the inverse clamped distance model.
		 *
		 * Default: +inf
		 * Type: ALfloat
		 * Range:  0.0 - +inf
		 */
		public const ALenum AL_MAX_DISTANCE = 0x1023;

		/** 
		 * Sound samples: frequency, in units of Hertz [Hz].
		 * This is the number of samples per second. Half of the
		 *  sample frequency marks the maximum significant
		 *  frequency component.
		 */
		public const ALenum AL_FREQUENCY = 0x2001;
		public const ALenum AL_BITS = 0x2002;
		public const ALenum AL_CHANNELS = 0x2003;
		public const ALenum AL_SIZE = 0x2004;

		/**
		 * Buffer state.
		 *
		 * Not supported for public use (yet).
		 */
		public const ALenum AL_UNUSED = 0x2010;
		public const ALenum AL_PENDING = 0x2011;
		public const ALenum AL_PROCESSED = 0x2012;


		/** Errors: No Error. */
		public const ALenum AL_NO_ERROR = AL_FALSE;

		/** 
		 * Invalid Name paramater passed to AL call.
		 */
		public const ALenum AL_INVALID_NAME = 0xA001;

		/** 
		 * Invalid parameter passed to AL call.
		 */
		public const ALenum AL_INVALID_ENUM = 0xA002;

		/** 
		 * Invalid enum parameter value.
		 */
		public const ALenum AL_INVALID_VALUE = 0xA003;

		/** 
		 * Illegal call.
		 */
		public const ALenum AL_INVALID_OPERATION = 0xA004;

  
		/**
		 * No mojo.
		 */
		public const ALenum AL_OUT_OF_MEMORY = 0xA005;


		/** Context strings: Vendor Name. */
		public const ALenum AL_VENDOR = 0xB001;
		public const ALenum AL_VERSION = 0xB002;
		public const ALenum AL_RENDERER = 0xB003;
		public const ALenum AL_EXTENSIONS = 0xB004;

		/** Global tweakage. */

		/**
		 * Doppler scale.  Default 1.0
		 */
		public const ALenum AL_DOPPLER_FACTOR = 0xC000;

		/**
		 * Tweaks speed of propagation.
		 */
		public const ALenum AL_DOPPLER_VELOCITY = 0xC001;

		/**
		 * Speed of Sound in units per second
		 */
		public const ALenum AL_SPEED_OF_SOUND = 0xC003;

		/**
		 * Distance models
		 *
		 * used in conjunction with DistanceModel
		 *
		 * implicit: NONE, which disances distance attenuation.
		 */
		public const ALenum AL_DISTANCE_MODEL = 0xD000;
		public const ALenum AL_INVERSE_DISTANCE = 0xD001;
		public const ALenum AL_INVERSE_DISTANCE_CLAMPED = 0xD002;
		public const ALenum AL_LINEAR_DISTANCE = 0xD003;
		public const ALenum AL_LINEAR_DISTANCE_CLAMPED = 0xD004;
		public const ALenum AL_EXPONENT_DISTANCE = 0xD005;
		public const ALenum AL_EXPONENT_DISTANCE_CLAMPED = 0xD006;

		/*
		 * Renderer State management
		 */
		[DllImport(DLL)] static public extern void alEnable( ALenum capability );
		[DllImport(DLL)] static public extern void alDisable( ALenum capability ); 
		[DllImport(DLL)] static public extern ALboolean alIsEnabled( ALenum capability ); 

		/*
		 * State retrieval
		 */
		[DllImport(DLL)] static public extern ALchar* alGetString( ALenum param );
		[DllImport(DLL)] static public extern void alGetBooleanv( ALenum param, ALboolean* data );
		[DllImport(DLL)] static public extern void alGetIntegerv( ALenum param, ALint* data );
		[DllImport(DLL)] static public extern void alGetFloatv( ALenum param, ALfloat* data );
		[DllImport(DLL)] static public extern void alGetDoublev( ALenum param, ALdouble* data );
		[DllImport(DLL)] static public extern ALboolean alGetBoolean( ALenum param );
		[DllImport(DLL)] static public extern ALint alGetInteger( ALenum param );
		[DllImport(DLL)] static public extern ALfloat alGetFloat( ALenum param );
		[DllImport(DLL)] static public extern ALdouble alGetDouble( ALenum param );

		/*
		 * Error support.
		 * Obtain the most recent error generated in the AL state machine.
		 */
		[DllImport(DLL)] static public extern ALenum alGetError( );

		/* 
		 * Extension support.
		 * Query for the presence of an extension, and obtain any appropriate
		 * function pointers and enum values.
		 */
		[DllImport(DLL)] static public extern ALboolean alIsExtensionPresent( ALchar* extname );
		[DllImport(DLL)] static public extern void* alGetProcAddress( ALchar* fname );
		[DllImport(DLL)] static public extern ALenum alGetEnumValue( ALchar* ename );


		/*
		 * LISTENER
		 * Listener represents the location and orientation of the
		 * 'user' in 3D-space.
		 *
		 * Properties include: -
		 *
		 * Gain         AL_GAIN         ALfloat
		 * Position     AL_POSITION     ALfloat[3]
		 * Velocity     AL_VELOCITY     ALfloat[3]
		 * Orientation  AL_ORIENTATION  ALfloat[6] (Forward then Up vectors)
		*/

		/*
		 * Set Listener parameters
		 */
		[DllImport(DLL)] static public extern void alListenerf( ALenum param, ALfloat value );
		[DllImport(DLL)] static public extern void alListener3f( ALenum param, ALfloat value1, ALfloat value2, ALfloat value3 );
		[DllImport(DLL)] static public extern void alListenerfv( ALenum param, ALfloat* values ); 
		[DllImport(DLL)] static public extern void alListeneri( ALenum param, ALint value );
		[DllImport(DLL)] static public extern void alListener3i( ALenum param, ALint value1, ALint value2, ALint value3 );
		[DllImport(DLL)] static public extern void alListeneriv( ALenum param, ALint* values );

		/*
		 * Get Listener parameters
		 */
		[DllImport(DLL)] static public extern void alGetListenerf( ALenum param, ALfloat* value );
		[DllImport(DLL)] static public extern void alGetListener3f( ALenum param, ALfloat *value1, ALfloat *value2, ALfloat *value3 );
		[DllImport(DLL)] static public extern void alGetListenerfv( ALenum param, ALfloat* values );
		[DllImport(DLL)] static public extern void alGetListeneri( ALenum param, ALint* value );
		[DllImport(DLL)] static public extern void alGetListener3i( ALenum param, ALint *value1, ALint *value2, ALint *value3 );
		[DllImport(DLL)] static public extern void alGetListeneriv( ALenum param, ALint* values );


		/**
		 * SOURCE
		 * Sources represent individual sound objects in 3D-space.
		 * Sources take the PCM data provided in the specified Buffer,
		 * apply Source-specific modifications, and then
		 * submit them to be mixed according to spatial arrangement etc.
		 * 
		 * Properties include: -
		 *
		 * Gain                              AL_GAIN                 ALfloat
		 * Min Gain                          AL_MIN_GAIN             ALfloat
		 * Max Gain                          AL_MAX_GAIN             ALfloat
		 * Position                          AL_POSITION             ALfloat[3]
		 * Velocity                          AL_VELOCITY             ALfloat[3]
		 * Direction                         AL_DIRECTION            ALfloat[3]
		 * Head Relative Mode                AL_SOURCE_RELATIVE      ALint (AL_TRUE or AL_FALSE)
		 * Reference Distance                AL_REFERENCE_DISTANCE   ALfloat
		 * Max Distance                      AL_MAX_DISTANCE         ALfloat
		 * RollOff Factor                    AL_ROLLOFF_FACTOR       ALfloat
		 * Inner Angle                       AL_CONE_INNER_ANGLE     ALint or ALfloat
		 * Outer Angle                       AL_CONE_OUTER_ANGLE     ALint or ALfloat
		 * Cone Outer Gain                   AL_CONE_OUTER_GAIN      ALint or ALfloat
		 * Pitch                             AL_PITCH                ALfloat
		 * Looping                           AL_LOOPING              ALint (AL_TRUE or AL_FALSE)
		 * MS Offset                         AL_MSEC_OFFSET          ALint or ALfloat
		 * Byte Offset                       AL_BYTE_OFFSET          ALint or ALfloat
		 * Sample Offset                     AL_SAMPLE_OFFSET        ALint or ALfloat
		 * Attached Buffer                   AL_BUFFER               ALint
		 * State (Query only)                AL_SOURCE_STATE         ALint
		 * Buffers Queued (Query only)       AL_BUFFERS_QUEUED       ALint
		 * Buffers Processed (Query only)    AL_BUFFERS_PROCESSED    ALint
		 */

		/* Create Source objects */
		[DllImport(DLL)] static public extern void alGenSources( ALsizei n, ALuint* sources ); 

		/* Delete Source objects */
		[DllImport(DLL)] static public extern void alDeleteSources( ALsizei n, ALuint* sources );

		/* Verify a handle is a valid Source */ 
		[DllImport(DLL)] static public extern ALboolean  alIsSource( ALuint sid ); 

		/*
		 * Set Source parameters
		 */
		[DllImport(DLL)] static public extern void  alSourcef( ALuint sid, ALenum param, ALfloat value ); 
		[DllImport(DLL)] static public extern void  alSource3f( ALuint sid, ALenum param, ALfloat value1, ALfloat value2, ALfloat value3 );
		[DllImport(DLL)] static public extern void  alSourcefv( ALuint sid, ALenum param, ALfloat* values ); 
		[DllImport(DLL)] static public extern void  alSourcei( ALuint sid, ALenum param, ALint value ); 
		[DllImport(DLL)] static public extern void  alSource3i( ALuint sid, ALenum param, ALint value1, ALint value2, ALint value3 );
		[DllImport(DLL)] static public extern void  alSourceiv( ALuint sid, ALenum param, ALint* values );

		/*
		 * Get Source parameters
		 */
		[DllImport(DLL)] static public extern void  alGetSourcef( ALuint sid, ALenum param, ALfloat* value );
		[DllImport(DLL)] static public extern void  alGetSource3f( ALuint sid, ALenum param, ALfloat* value1, ALfloat* value2, ALfloat* value3);
		[DllImport(DLL)] static public extern void  alGetSourcefv( ALuint sid, ALenum param, ALfloat* values );
		[DllImport(DLL)] static public extern void  alGetSourcei( ALuint sid,  ALenum param, ALint* value );
		[DllImport(DLL)] static public extern void  alGetSource3i( ALuint sid, ALenum param, ALint* value1, ALint* value2, ALint* value3);
		[DllImport(DLL)] static public extern void  alGetSourceiv( ALuint sid,  ALenum param, ALint* values );


		/*
		 * Source vector based playback calls
		 */

		/* Play, replay, or resume (if paused) a list of Sources */
		[DllImport(DLL)] static public extern void  alSourcePlayv( ALsizei ns, ALuint *sids );
		/* Stop a list of Sources */
		[DllImport(DLL)] static public extern void  alSourceStopv( ALsizei ns, ALuint *sids );
		/* Rewind a list of Sources */
		[DllImport(DLL)] static public extern void  alSourceRewindv( ALsizei ns, ALuint *sids );
		/* Pause a list of Sources */
		[DllImport(DLL)] static public extern void  alSourcePausev( ALsizei ns, ALuint *sids );

		/*
		 * Source based playback calls
		 */

		/* Play, replay, or resume a Source */
		[DllImport(DLL)] static public extern void  alSourcePlay( ALuint sid );

		/* Stop a Source */
		[DllImport(DLL)] static public extern void  alSourceStop( ALuint sid );

		/* Rewind a Source (set playback postiton to beginning) */
		[DllImport(DLL)] static public extern void  alSourceRewind( ALuint sid );

		/* Pause a Source */
		[DllImport(DLL)] static public extern void  alSourcePause( ALuint sid );

		/*
		 * Source Queuing 
		 */
		[DllImport(DLL)] static public extern void  alSourceQueueBuffers( ALuint sid, ALsizei numEntries, ALuint *bids );
		[DllImport(DLL)] static public extern void  alSourceUnqueueBuffers( ALuint sid, ALsizei numEntries, ALuint *bids );


		/**
		 * BUFFER
		 * Buffer objects are storage space for sample data.
		 * Buffers are referred to by Sources. One Buffer can be used
		 * by multiple Sources.
		 *
		 * Properties include: -
		 *
		 * Frequency (Query only)    AL_FREQUENCY      ALint
		 * Size (Query only)         AL_SIZE           ALint
		 * Bits (Query only)         AL_BITS           ALint
		 * Channels (Query only)     AL_CHANNELS       ALint
		 */

		/* Create Buffer objects */
		[DllImport(DLL)] static public extern void alGenBuffers( ALsizei n, ALuint* buffers );

		/* Delete Buffer objects */
		[DllImport(DLL)] static public extern void alDeleteBuffers( ALsizei n, ALuint* buffers );

		/* Verify a handle is a valid Buffer */
		[DllImport(DLL)] static public extern ALboolean alIsBuffer( ALuint bid );

		/* Specify the data to be copied into a buffer */
		[DllImport(DLL)] static public extern void alBufferData( ALuint bid, ALenum format, void* data, ALsizei size, ALsizei freq );

		/*
		 * Set Buffer parameters
		 */
		[DllImport(DLL)] static public extern void alBufferf( ALuint bid, ALenum param, ALfloat value );
		[DllImport(DLL)] static public extern void alBuffer3f( ALuint bid, ALenum param, ALfloat value1, ALfloat value2, ALfloat value3 );
		[DllImport(DLL)] static public extern void alBufferfv( ALuint bid, ALenum param, ALfloat* values );
		[DllImport(DLL)] static public extern void alBufferi( ALuint bid, ALenum param, ALint value );
		[DllImport(DLL)] static public extern void alBuffer3i( ALuint bid, ALenum param, ALint value1, ALint value2, ALint value3 );
		[DllImport(DLL)] static public extern void alBufferiv( ALuint bid, ALenum param, ALint* values );

		/*
		 * Get Buffer parameters
		 */
		[DllImport(DLL)] static public extern void alGetBufferf( ALuint bid, ALenum param, ALfloat* value );
		[DllImport(DLL)] static public extern void alGetBuffer3f( ALuint bid, ALenum param, ALfloat* value1, ALfloat* value2, ALfloat* value3);
		[DllImport(DLL)] static public extern void alGetBufferfv( ALuint bid, ALenum param, ALfloat* values );
		[DllImport(DLL)] static public extern void alGetBufferi( ALuint bid, ALenum param, ALint* value );
		[DllImport(DLL)] static public extern void alGetBuffer3i( ALuint bid, ALenum param, ALint* value1, ALint* value2, ALint* value3);
		[DllImport(DLL)] static public extern void alGetBufferiv( ALuint bid, ALenum param, ALint* values );

		/*
		 * Global Parameters
		 */
		[DllImport(DLL)] static public extern void alDopplerFactor( ALfloat value );
		[DllImport(DLL)] static public extern void alDopplerVelocity( ALfloat value );
		[DllImport(DLL)] static public extern void alSpeedOfSound( ALfloat value );
		[DllImport(DLL)] static public extern void alDistanceModel( ALenum distanceModel );

		static public string alGetErrorString(ALenum error)
		{
			return "Unknown(" + error + ")";
		}
	}
}
