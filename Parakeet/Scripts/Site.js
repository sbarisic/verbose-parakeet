$(document).ready(function () {
	console.log("jQuery " + jQuery().jquery);

	if (typeof $().modal == 'function')
		console.log("Bootstrap loaded");
	else
		console.log("Bootstrap NOT loaded");

	$('[data-toggle="tooltip"]').tooltip();
});
