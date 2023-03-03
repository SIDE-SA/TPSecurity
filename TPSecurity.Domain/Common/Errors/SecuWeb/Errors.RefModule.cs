using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPSecurity.Domain.Common.Errors
{
	public static partial class Errors
	{
		public static class RefModule
		{
			public static Error RefApplicationNotFound => Error.Conflict(code: "RefModule.RefApplicationNotfound", description: "L'application n'existe pas");
		}
	}
}
