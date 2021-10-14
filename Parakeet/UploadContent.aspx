<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="UploadContent.aspx.cs" Inherits="Parakeet.UploadContent" %>

<%@ Register Src="~/Controls/ContentPreview.ascx" TagName="ContentPreview" TagPrefix="uc" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
	<div class="center">
		<div class="mr-sm-2 mb-4">
			<%--<input id="inSearchText" class="form-control" type="search" placeholder="Search" aria-label="Search" data-role="tagsinput" runat="server">--%>

			<asp:TextBox ID="inTags" CssClass="form-control" type="search" placeholder="Tags" aria-label="Tags" data-role="tagsinput" runat="server" />
		</div>

		<asp:FileUpload ID="fileUpload" onchange="onFileBrowse(event)" runat="server" />
		<asp:Button ID="btnUpload" type="submit" CssClass="upload-button" Text="Upload" OnClick="btnUpload_Click" runat="server" disabled="disabled"></asp:Button>

		<div class="mt-4">
			<div class="thumb m-auto thumb-maxsize">
				<figure class="figure thumb-figure">
					<asp:Image CssClass="figure-img img-fluid rounded thumb-image" ID="imgPreview" ImageUrl="~/Content/Images/missing.png" runat="server" />
				</figure>
			</div>
		</div>

		<div id="divInfo" class="info-text-div" runat="server">
			<asp:Label ID="lblInfo" CssClass="info-text" Text="Nepoznata poruka (-69)" runat="server" />
		</div>
	</div>
</asp:Content>
