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
	public partial class Register : ParakeetPage {
		protected void Page_Load(object sender, EventArgs e) {
			if (CheckLoggedIn()) {
				Response.Redirect("~/Default.aspx");
				return;
			}

		}

		protected void btnRegister_Click(object sender, EventArgs e) {

		}
	}
}