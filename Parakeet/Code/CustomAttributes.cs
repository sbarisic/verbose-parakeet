using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Parakeet.Code {
	[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
	sealed class DbTableAttribute : Attribute {
		public string TableName;

		public DbTableAttribute(string TableName) {
			this.TableName = TableName;
		}
	}

	[AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
	sealed class DbIDColumn : Attribute {
		public DbIDColumn() {
		}
	}

	[AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
	sealed class CssStyleAttribute : Attribute {
		public string Style;

		public CssStyleAttribute(string Style) {
			this.Style = Style;
		}
	}
}