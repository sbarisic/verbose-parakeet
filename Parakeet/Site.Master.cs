using Parakeet.Code;
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

			if (Page != null) {
				foreach (PkControl WebCtrl in Page.GenerateWebControls()) {
					ulNavbarItems.Controls.Add(WebCtrl.GenerateNavbar());
				}
			}
		}
	}
}