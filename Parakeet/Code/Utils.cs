using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Parakeet.Code {
	public static class Utils {
		public static object ToEnum(int Value, Type EnumType) {
			return Enum.Parse(EnumType, Value.ToString());
		}
	}
}