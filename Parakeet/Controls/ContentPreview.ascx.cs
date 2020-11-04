using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Parakeet.Controls {
	public partial class ContentPreview : System.Web.UI.UserControl {
		public string Name {
			get; set;
		}

		public string FilePath {
			get; set;
		}

		protected void Page_Load(object sender, EventArgs e) {
			// ~/Content/Images/missing.png
		}
	}
}