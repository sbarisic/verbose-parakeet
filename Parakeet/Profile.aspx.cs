﻿using System;
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
	public partial class ProfilePage : ParakeetPage {


		protected void Page_Load(object sender, EventArgs e) {
			ParakeetUser Admin = ParakeetDb.Select<ParakeetUser>(new DbFilter("Username", "admin")).First();
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