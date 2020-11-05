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
			ParakeetUser Admin = ParakeetDb.Select<ParakeetUser>(new DbFilter("Username", "admin")).First();

			ContentItem[] Items = ParakeetDb.Select<ContentID>().Select(CID => new ContentItem(CID)).ToArray();

			int OldLength = Items.Length;
			Array.Resize(ref Items, OldLength * 2);
			Array.Copy(Items, 0, Items, OldLength, OldLength);

			rptImages.DataSource = Items;
			rptImages.DataBind();
		}

		public override IEnumerable<PkControl> GenerateWebControls() {
			yield return new PkButton("Hello World!", PkButtonStyle.Light, (S, E) => {
				Debug.WriteLine("Hello World!");
			});

			yield return PkCustomControl.MarginLeft(2);
			yield return new PkButton("Append Button", PkButtonStyle.Light, new PkWebMethod(nameof(AppendButton), @" function(ret) { $('#new_here').empty(); $('#new_here').append($(ret)); }"));


			PkCustomControl ListItem = PkCustomControl.ListItem("nav-item");
			ListItem.ID = "new_here";

			yield return PkCustomControl.MarginLeft(2);
			yield return ListItem;

			yield return PkCustomControl.MarginLeft(2);
			yield return new PkButton("Crappy button!", PkButtonStyle.Secondary, (S, E) => {
				Debug.WriteLine("Hello World!");
			});
		}

		static int Counter = 0;

		[WebMethod]
		public static string AppendButton() {
			return new PkButton("Button #" + (Counter++).ToString(), PkButtonStyle.Success, new PkWebMethod(nameof(WriteHueHue))).ToHTML();
		}

		[WebMethod]
		public static void WriteHueHue() {
			Debug.WriteLine("Hue hue hue!");
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