using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Diagnostics;
using System.Reflection;
using Parakeet.Code;

namespace Parakeet {
	public partial class _Default : ParakeetPage {
		protected void Page_Load(object sender, EventArgs e) {
			ParakeetUser User = null;

			if (!CheckLoggedIn(out User)) {
				Response.Redirect("~/Login.aspx");
				return;
			}

			ContentItem[] Items = ParakeetDb.Select<ContentID>().Select(CID => new ContentItem(CID)).ToArray();

			int OldLength = Items.Length;
			Array.Resize(ref Items, OldLength * 2);
			Array.Copy(Items, 0, Items, OldLength, OldLength);

			rptImages.DataSource = Items;
			rptImages.DataBind();
		}
	}

	class ContentItem {
		public string Name {
			get; set;
		}

		public string FilePath {
			get; set;
		}

		public ContentItem(ContentID CID) {
			Name = CID.Name;

			string RootFolder = "~/DataFolder/";

			if (CID.ContentType == ContentIDType.Image) {
				RootFolder += "Image/";
			} else if (CID.ContentType == ContentIDType.Video) {
				RootFolder += "Video/";
			} else
				throw new Exception();

			FilePath = string.Format(RootFolder + CID.FileName);
		}
	}
}