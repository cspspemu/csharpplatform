using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpPlatform.GL.Impl
{
	public interface IOpenglContext
	{
		void MakeCurrent();
		void SwapBuffers();
	}
}
