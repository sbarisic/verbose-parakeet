<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="UploadContent.aspx.cs" Inherits="Parakeet.UploadContent" %>

<%@ Register Src="~/Controls/ContentPreview.ascx" TagName="ContentPreview" TagPrefix="uc" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
	<div class="center">
		<asp:FileUpload ID="fileUpload" runat="server" />
		<asp:Button ID="btnUpload" type="submit" CssClass="upload-button" Text="Upload" OnClick="btnUpload_Click" runat="server"></asp:Button>

		<div id="divInfo" class="info-text-div" runat="server">
			<asp:Label ID="lblInfo" CssClass="info-text" Text="Nepoznata poruka (-69)" runat="server" />
		</div>
	</div>
</asp:Content>
