using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Specialized;
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

			NameValueCollection QS = Request.QueryString;
			string[] Tags = (QS.Get("tags") ?? "").Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

			if (Tags.Length == 0)
				Tags = null;

			ContentItem[] Items = ParakeetDb.Select<ContentID>().Where(CID => CID.HasTags(Tags)).Select(CID => new ContentItem(CID)).ToArray();
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
			string RootFolder;

			if (CID.ContentType == ContentIDType.Image) {
				RootFolder = Cfg.ImageDir;
			} else if (CID.ContentType == ContentIDType.Video) {
				RootFolder = Cfg.VideoDir;
			} else
				throw new Exception();

			RootFolder += "/";
			FilePath = string.Format(RootFolder + CID.FileName);
		}
	}
}