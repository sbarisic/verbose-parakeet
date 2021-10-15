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
	public partial class UploadContent : ParakeetPage {
		ParakeetUser User;

		protected void Page_Load(object sender, EventArgs e) {
			if (!CheckLoggedIn(out User)) {
				Response.Redirect("~/Default.aspx");
				return;
			}

			divInfo.Visible = false;
		}

		void PrintMessage(string Msg) {
			divInfo.Visible = true;
			lblInfo.Text = Msg;
		}

		void Clear() {
			inTags.Text = "";
		}

		protected void btnUpload_Click(object sender, EventArgs e) {
			if (!fileUpload.HasFile) {
				PrintMessage("File was not attached");
				return;
			}

			byte[] Bytes = fileUpload.FileBytes;
			string Name = Files.GetFileName(fileUpload.FileName);
			string Ext = Path.GetExtension(fileUpload.FileName).ToLower();
			string NewName = Files.GenFileName(Ext);

			string[] Formats = new[] { ".jpg", ".png", ".gif" };

			if (!Formats.Contains(Ext)) {
				PrintMessage(string.Format("Unsupported file format ({0})", string.Join(", ", Formats)));
				return;
			}

			if (Bytes.Length > 16000000) {
				PrintMessage("File size too big! 16 megabytes or less supported");
				return;
			}

			HashSet<string> Tags = Utils.ParseTags(inTags.Text);
			Tags.Add(Name);
			Files.SaveImageFile(NewName, Bytes);

			ContentID Content = new ContentID();
			Content.ContentType = ContentIDType.Image;
			Content.Name = Name;
			Content.OwnerID = User.ID;
			Content.Tags = Utils.JoinTags(Tags);
			Content.FileName = NewName;
			ParakeetDb.Insert(Content);

			Clear();
			PrintMessage("Upload success!");
		}

		/*public override IEnumerable<PkControl> GenerateWebControls() {
			yield return new PkButton("Main Page", PkButtonStyle.Light, (S, E) => {
				
			});

			yield return PkCustomControl.MarginLeft(2);
			yield return new PkButton("View Profile", PkButtonStyle.Light, (S, E) => {
			
			});
		}*/
	}
}