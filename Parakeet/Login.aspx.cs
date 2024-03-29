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
	public partial class Login : ParakeetPage {
		protected void Page_Load(object sender, EventArgs e) {
			if (CheckLoggedIn()) {
				Response.Redirect("~/Default.aspx");
				return;
			}
		}

		protected void btnLogin_Click(object sender, EventArgs e) {
			string Username = tbUsername.Text;
			string Password = tbPassword.Text;
			bool RememberMe = cbRememberMe.Checked;

			ParakeetUser Usr = ParakeetDb.Select<ParakeetUser>(new DbFilter("Username", Username)).First();

			if (Usr == null)
				ThrowInvalidUserOrPass();

			if (!PasswordManager.IsValidPassword(Password, Usr.Salt, Usr.Hash))
				ThrowInvalidUserOrPass();

			SetUser(Usr);
			Response.Redirect("~/Default.aspx");
		}
	}
}