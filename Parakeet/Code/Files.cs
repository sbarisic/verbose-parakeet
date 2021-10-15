using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace Parakeet.Code {
	public static class Files {
		public static string[] GetImageFiles() {
			return Directory.GetFiles(MapPath(Cfg.ImageDir));
		}

		public static string[] GetVideoFiles() {
			return Directory.GetFiles(MapPath(Cfg.VideoDir));
		}

		public static string GetFileName(string Pth, bool Extension = false) {
			if (Extension)
				return Path.GetFileName(Pth);

			return Path.GetFileNameWithoutExtension(Pth);
		}

		public static void SaveFile(string FileName, byte[] Bytes) {

		}

		public static void SaveImageFile(string FileName, byte[] Bytes) {
			string FullPath = MapPath(Cfg.ImageDir);
			File.WriteAllBytes(Path.Combine(FullPath, FileName), Bytes);
		}

		public static string GenFileName(string Ext) {
			return DateTime.Now.ToString("yyyyMMdd_HHmmssfff") + Ext;
		}

		public static string MapPath(string Path) {
			return HttpContext.Current.Server.MapPath(Path);
		}
	}
}