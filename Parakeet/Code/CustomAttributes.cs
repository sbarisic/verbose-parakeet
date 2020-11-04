using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Parakeet.Code {
	[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
	sealed class DatabaseTableAttribute : Attribute {
		public string TableName;

		public DatabaseTableAttribute(string TableName) {
			this.TableName = TableName;
		}
	}
}