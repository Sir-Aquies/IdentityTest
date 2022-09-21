const emptyinput = document.createElement("input");
emptyinput.type = "file";

function ShowPreview(input) {
	const [file] = input.files;

	if (file) {
		const frame = document.getElementById("PreviewFrame");

		if (frame.style.display == "block") {
			frame.src = URL.createObjectURL(file);
			return;
		}

		frame.src = URL.createObjectURL(file);
		frame.style.display = "block";
		document.getElementById("deleteBtn").style.display = "block";

	}
}

function DeletePreview() {
	const input = document.getElementById("FileFrame");
	const frame = document.getElementById("PreviewFrame");

	input.files = emptyinput.files;

	frame.src = "";
	frame.style.display = "none";

	document.getElementById("deleteBtn").style.display = "none";
}