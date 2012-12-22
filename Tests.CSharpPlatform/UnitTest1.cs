using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GLES;

namespace Tests.CSharpPlatform
{
	[TestClass]
	public class UnitTest1
	{
		[TestMethod]
		unsafe public void TestMatrix()
		{
			Console.WriteLine(Matrix4.Identity.Translate(2, 2, 0));
		}
	}
}
