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
			string Username = tbUsername.Text;
			string Password = tbPassword.Text;
			string Password2 = tbPassword2.Text;

			if (Password != Password2)
				throw new Exception("Passwords do not match");

			ParakeetUser Usr = ParakeetDb.Select<ParakeetUser>(new DbFilter("Username", Username)).FirstOrDefault();

			if (Usr != null)
				throw new Exception("Username has been reserved");


			ParakeetUser NewUser = new ParakeetUser();
			NewUser.Username = Username;
			NewUser.Salt = PasswordManager.GenerateSalt();
			NewUser.Hash = PasswordManager.HashPassword(Password, NewUser.Salt);

			ParakeetDb.Insert(NewUser);
			SetUser(NewUser);
			Response.Redirect("~/Default.aspx");
		}
	}
}