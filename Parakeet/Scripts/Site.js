$(document).ready(function () {
	console.log("jQuery " + jQuery().jquery);

	if (typeof $().modal == 'function')
		console.log("Bootstrap loaded");
	else
		console.log("Bootstrap NOT loaded");

	$('[data-toggle="tooltip"]').tooltip();
});

function PageMethodSuccess(res, ctx) {
	console.log("Page method success!" + res);

	if (typeof ctx === "function") {
		ctx(res);
	}
}

function PageMethodError(res) {
	console.log("Page method error!" + res);
}