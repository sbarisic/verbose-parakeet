using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace Parakeet.Code {
	public static class Utils {
		public static object ToEnum(int Value, Type EnumType) {
			return Enum.Parse(EnumType, Value.ToString());
		}

		public static T GetEnumAttr<T>(Enum EnumVal) where T : Attribute {
			FieldInfo FI = EnumVal.GetType().GetField(EnumVal.ToString());
			return FI.GetCustomAttribute<T>();
		}

		public static string Serialize(object Obj) {
			return JsonConvert.SerializeObject(Obj);
		}

		public static T Deserialize<T>(string Str) {
			return (T)JsonConvert.DeserializeObject(Str, typeof(T));
		}

		public static string ToHTML(HtmlControl Ctrl) {
			StringWriter SW = new StringWriter();
			HtmlTextWriter HTMLWriter = new HtmlTextWriter(SW);
			Ctrl.RenderControl(HTMLWriter);
			return SW.ToString();
		}
	}
}