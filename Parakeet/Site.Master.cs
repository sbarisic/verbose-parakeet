using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Parakeet.Code;

namespace Parakeet {
	public partial class SiteMaster : MasterPage {
		protected void Page_Load(object sender, EventArgs e) {
			ParakeetPage Page = MainContent.Page as ParakeetPage;

			if (Page == null) {
				Response.Redirect("~/Default.aspx");
				return;
			}

			EmitControl(new PkButton("Main Page", PkButtonStyle.Info, (S, E) => {
				Response.Redirect("~/Default.aspx");
			}));


			if (Page.CheckLoggedIn(out ParakeetUser Usr)) {
				EmitControl(PkCustomControl.MarginLeft(2));
				EmitControl(new PkButton("View Profile - " + Usr.Username, PkButtonStyle.Success, (S, E) => {
					Response.Redirect("~/Profile.aspx");
				}));

				EmitControl(PkCustomControl.MarginLeft(2));
				EmitControl(new PkButton("Upload Content", PkButtonStyle.Warning, (S, E) => {
					Response.Redirect("~/UploadContent.aspx");
				}));

				EmitControl(PkCustomControl.MarginLeft(2));
				EmitControl(new PkButton("Logout", PkButtonStyle.Danger, (S, E) => {
					Page.SetUser(null);
					Response.Redirect("~/Default.aspx");
				}));
			} else {
				EmitControl(PkCustomControl.MarginLeft(2));
				EmitControl(new PkButton("Log In", PkButtonStyle.Success, (S, E) => {
					Response.Redirect("~/Login.aspx");
				}));

				EmitControl(PkCustomControl.MarginLeft(2));
				EmitControl(new PkButton("Register", PkButtonStyle.Warning, (S, E) => {
					Response.Redirect("~/Register.aspx");
				}));
			}

			EmitControl(PkCustomControl.MarginLeft(2));
			if (Page != null) {
				foreach (PkControl WebCtrl in Page.GenerateWebControls())
					EmitControl(WebCtrl);
			}
		}

		void EmitControl(PkControl WebCtrl) {
			ulNavbarItems.Controls.Add(WebCtrl.GenerateNavbar());
		}

		protected void Search_Click(object sender, EventArgs e) {
			HashSet<string> Tags = new HashSet<string>(inSearchText.Text.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(T=>T.Trim()));

			Response.Redirect("~/Default.aspx?tags=" + string.Join(",", Tags.ToArray()));
		}
	}
}