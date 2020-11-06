using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.SessionState;

namespace Parakeet.Code {
	public class ParakeetPage : Page {
		ParakeetUser PkUser;

		public ParakeetUser GetUser() {
			return Session[nameof(ParakeetUser)] as ParakeetUser;
		}

		public void SetUser(ParakeetUser User) {
			Session[nameof(ParakeetUser)] = User;
		}

		public bool CheckLoggedIn() {
			ParakeetUser U = GetUser();

			if (U != null) {
				PkUser = U;
				return true;
			}

			return false;
		}

		public void Download(string FileName, byte[] Data) {
			Response.ContentType = "application/force-download";
			Response.AppendHeader("Content-Disposition", "attachment;filename=" + FileName);

			Response.BinaryWrite(Data);
			Response.End();
		}

		public virtual IEnumerable<PkControl> GenerateWebControls() {
			yield break;
		}
	}
}