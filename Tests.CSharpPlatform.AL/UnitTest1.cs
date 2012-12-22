using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CSharpPlatform.AL;

namespace Tests.CSharpPlatform
{
	[TestClass]
	unsafe public class UnitTest1
	{
		[TestMethod]
		public void TestMethod1()
		{
			var device = AL.alcOpenDevice(AL.alcGetString(null, AL.ALC_DEFAULT_DEVICE_SPECIFIER));
			var context = AL.alcCreateContext(null, null);
			AL.alcDestroyContext(context);
			AL.alcCloseDevice(device);
		}
	}
}
