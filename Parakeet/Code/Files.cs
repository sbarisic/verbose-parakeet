using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace Parakeet.Code {
	public static class Files {
		public static string[] GetImageFiles() {
			return Directory.GetFiles(Cfg.ImageDir);
		}

		public static string[] GetVideoFiles() {
			return Directory.GetFiles(Cfg.VideoDir);
		}

		public static string GetFileName(string Pth, bool Extension = false) {
			if (Extension)
				return Path.GetFileName(Pth);

			return Path.GetFileNameWithoutExtension(Pth);
		}


	}
}