using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.IO;

namespace Parakeet.Code {
	public static class Cfg {
		public static string GetKey(string Key) {
			return ConfigurationManager.AppSettings[Key];
		}

		public static string RootDir {
			get {
				return GetKey(nameof(RootDir));
			}
		}

		public static string VideoDir {
			get {
				return RootDir + "/" + GetKey(nameof(VideoDir));
			}
		}

		public static string ImageDir {
			get {
				return RootDir + "/" + GetKey(nameof(ImageDir));
			}
		}

		public static string DatabaseConnection {
			get {
				return GetKey(nameof(DatabaseConnection));
			}
		}
	}
}