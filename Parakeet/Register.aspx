<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="Parakeet.Register" %>

<%@ Register Src="~/Controls/ContentPreview.ascx" TagName="ContentPreview" TagPrefix="uc" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
	<div class="popup">
		<div class="container">
			<div class="d-flex justify-content-center h-100">
				<div class="card">
					<div class="card-header">
						<h3>Register</h3>
					</div>

					<div id="divCardBody" class="card-body" runat="server">
						<div class="input-group form-group">
							<div class="input-group-prepend">
								<span class="input-group-text"><i class="fas fa-user"></i></span>
							</div>

							<asp:TextBox ID="tbUsername" CssClass="form-control" placeholder="Username" type="text" runat="server"></asp:TextBox>
						</div>

						<div class="input-group form-group">
							<div class="input-group-prepend">
								<span class="input-group-text"><i class="fas fa-key"></i></span>
							</div>

							<asp:TextBox ID="tbPassword" CssClass="form-control" placeholder="Password" type="password" runat="server"></asp:TextBox>
						</div>

						<div class="input-group form-group">
							<div class="input-group-prepend">
								<span class="input-group-text"><i class="fas fa-key"></i></span>
							</div>

							<asp:TextBox ID="tbPassword2" CssClass="form-control" placeholder="Repeat Password" type="password" runat="server"></asp:TextBox>
						</div>

						<div class="row align-items-center remember">
							<asp:CheckBox ID="cbRememberMe" runat="server" Text="Remember Me" />
						</div>

					</div>

					<div class="card-footer">
						<div class="form-group">
							<asp:Button ID="btnRegister" CssClass="btn float-right login_btn" runat="server" Text="Register" OnClick="btnRegister_Click" />
						</div>
					</div>
				</div>
			</div>
		</div>
	</div>
</asp:Content>
