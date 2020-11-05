using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Parakeet.Code {
	public delegate void OnClickFunc(object Sender, EventArgs Args);

	public abstract class PkControl {
		public abstract HtmlControl Generate();

		public abstract HtmlControl GenerateNavbar();

		public string ToHTML() {
			return Utils.ToHTML(Generate());
		}

		public static HtmlGenericControl GenerateDiv(string CssClass) {
			HtmlGenericControl Div = new HtmlGenericControl("div");
			Div.Attributes["class"] = CssClass;
			return Div;
		}

		public static HtmlLink GenerateLink(string CssClass) {
			HtmlLink Link = new HtmlLink();
			Link.Attributes["class"] = CssClass;
			return Link;
		}

		public static HtmlButton GenerateButton(string CssClass) {
			HtmlButton Button = new HtmlButton();
			Button.Attributes["class"] = CssClass;
			return Button;
		}
	}

	public class PkCustomControl : PkControl {
		public string TagName;
		public string CssClass;
		public string ID;

		List<HtmlControl> Controls = new List<HtmlControl>();

		public PkCustomControl(string TagName, string CssClass) {
			this.TagName = TagName;
			this.CssClass = CssClass;
		}

		public void AddControl(HtmlControl Ctrl) {
			Controls.Add(Ctrl);
		}

		public void AddControl(PkControl Ctrl) {
			AddControl(Ctrl.Generate());
		}

		public override HtmlControl Generate() {
			HtmlGenericControl Ctrl = new HtmlGenericControl(TagName);

			if (!string.IsNullOrEmpty(CssClass))
				Ctrl.Attributes["class"] = CssClass;

			if (!string.IsNullOrEmpty(ID))
				Ctrl.ID = ID;

			foreach (var C in Controls)
				Ctrl.Controls.Add(C);

			return Ctrl;
		}

		public override HtmlControl GenerateNavbar() {
			return Generate();
		}

		public static PkCustomControl MarginLeft(int Val) {
			return new PkCustomControl("div", "ml-" + Val.ToString());
		}

		public static PkCustomControl MarginRight(int Val) {
			return new PkCustomControl("div", "mr-" + Val.ToString());
		}

		public static PkCustomControl MarginTop(int Val) {
			return new PkCustomControl("div", "mt-" + Val.ToString());
		}

		public static PkCustomControl MarginBottom(int Val) {
			return new PkCustomControl("div", "mb-" + Val.ToString());
		}

		public static PkCustomControl DivID(string ID) {
			PkCustomControl DivID = new PkCustomControl("div", "");
			DivID.ID = ID;
			return DivID;
		}

		public static PkCustomControl ListItem(string CssClass) {
			return new PkCustomControl("li", CssClass);
		}
	}

	public enum PkButtonStyle {
		None = 0,

		[CssStyle("btn btn-primary")]
		Primary,

		[CssStyle("btn btn-secondary")]
		Secondary,

		[CssStyle("btn btn-success")]
		Success,

		[CssStyle("btn btn-danger")]
		Danger,

		[CssStyle("btn btn-warning")]
		Warning,

		[CssStyle("btn btn-info")]
		Info,

		[CssStyle("btn btn-light")]
		Light,

		[CssStyle("btn btn-dark")]
		Dark,

		[CssStyle("btn btn-link")]
		Link,

		[CssStyle("dropdown-item")]
		DropdownItem
	}

	public class PkWebMethod {
		public string JS;

		public PkWebMethod(string MethodName) {
			JS = string.Format("PageMethods.{0}(PageMethodSuccess, PageMethodError)", MethodName);
		}
		public PkWebMethod(string MethodName, string Callback) {
			JS = string.Format("PageMethods.{0}(PageMethodSuccess, PageMethodError, {1})", MethodName, Callback.Trim());
		}
	}

	public class PkButton : PkControl {
		public event OnClickFunc OnClick;
		public string Text;

		string StyleStr;
		string ClientJS;

		public PkButton(string Text, PkButtonStyle Style) {
			this.Text = Text;
			SetButtonStyle(Style);
		}

		public PkButton(string Text, PkButtonStyle Style, OnClickFunc Func) : this(Text, Style) {
			OnClick += Func;
		}

		public PkButton(string Text, PkButtonStyle Style, PkWebMethod WebFunc) : this(Text, Style) {
			ClientJS = WebFunc.JS;
		}

		public PkButton(string Text, PkButtonStyle Style, string ClientJS) : this(Text, Style) {
			this.ClientJS = ClientJS;
		}

		public void SetButtonStyle(PkButtonStyle Style) {
			if (Style == PkButtonStyle.None)
				return;

			StyleStr = Utils.GetEnumAttr<CssStyleAttribute>(Style).Style;
		}

		public override HtmlControl GenerateNavbar() {
			PkCustomControl Li = PkCustomControl.ListItem("nav-item");
			Li.AddControl(this);
			return Li.GenerateNavbar();
		}

		public override HtmlControl Generate() {
			HtmlButton Btn = GenerateButton(StyleStr);
			Btn.InnerText = Text;

			if (!string.IsNullOrWhiteSpace(ClientJS)) {
				Btn.Attributes["onclick"] = ClientJS + "; return false;";
			} else {
				Btn.ServerClick += (S, E) => {
					OnClick?.Invoke(this, E);
				};
			}

			return Btn;
		}
	}

	public class PkDropdown : PkControl {
		public string Text;
		string StyleStr;

		List<PkControl> Controls;

		public PkDropdown(string Text, PkButtonStyle Style) {
			Controls = new List<PkControl>();

			this.Text = Text;
			StyleStr = Utils.GetEnumAttr<CssStyleAttribute>(Style).Style + " dropdown-toggle";
		}

		public void AddButton(PkButton Btn) {
			Btn.SetButtonStyle(PkButtonStyle.DropdownItem);
			Controls.Add(Btn);
		}

		public void AddDivider() {
			Controls.Add(new PkCustomControl("div", "dropdown-divider"));
		}

		public override HtmlControl Generate() {
			PkCustomControl Li = PkCustomControl.ListItem("nav-item dropdown");

			HtmlButton DropdownButton = GenerateButton(StyleStr);
			DropdownButton.InnerText = Text;
			DropdownButton.Attributes["type"] = "button";
			DropdownButton.Attributes["data-toggle"] = "dropdown";
			DropdownButton.Attributes["aria-haspopup"] = "true";
			DropdownButton.Attributes["aria-expanded"] = "false";
			Li.AddControl(DropdownButton);

			HtmlGenericControl Div = GenerateDiv("dropdown-menu");

			foreach (var C in Controls) {
				Div.Controls.Add(C.Generate());
			}

			Li.AddControl(Div);
			return Li.Generate();
		}

		public override HtmlControl GenerateNavbar() {
			return Generate();
		}
	}
}