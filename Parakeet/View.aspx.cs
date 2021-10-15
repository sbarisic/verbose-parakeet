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
	public partial class View : ParakeetPage {
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
	}
}