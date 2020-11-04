<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ContentPreview.ascx.cs" Inherits="Parakeet.Controls.ContentPreview" %>

<div class="thumb mt-1 mb-1">
	<figure class="figure thumb-figure">

		<asp:Image CssClass="figure-img img-fluid rounded thumb-image" ID="imgTest" ImageUrl='<%# FilePath %>' runat="server" />
		<figcaption class="figure-caption text-center thumb-figcaption"><%# Name %></figcaption>

	</figure>
</div>

