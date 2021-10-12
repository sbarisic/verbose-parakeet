<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ContentPreview.ascx.cs" Inherits="Parakeet.Controls.ContentPreview" %>

<div class="thumb mt-1 mb-1">
	<figure class="figure thumb-figure">

		<asp:Image CssClass="figure-img img-fluid rounded thumb-image" ID="imgTest" ImageUrl='<%# FilePath %>' runat="server" />

		<div class="d-flex justify-content-center mt-2">
			<%--<figcaption class="figure-caption text-center thumb-figcaption"><%# Name %></figcaption>--%>

			<div class="btn-group btn-group-toggle" data-toggle="buttons">
				<label class="btn btn-sm btn-secondary active">
					<input type="radio" name="options" id="option1" autocomplete="off" checked>
					<%# Name %>
				</label>

				<div class="btn-group">
					<button type="button" class="btn btn-sm btn-secondary dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
						Action
					</button>
					<div class="dropdown-menu">
						<a class="dropdown-item" href="#">Action</a>
						<a class="dropdown-item" href="#">Another action</a>
						<a class="dropdown-item" href="#">Something else here</a>
						<div class="dropdown-divider"></div>
						<a class="dropdown-item" href="#">Separated link</a>
					</div>
				</div>
			</div>


		</div>

	</figure>
</div>

