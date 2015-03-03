$.validator.methods.date = function (value, element) {
	return this.optional(element) || !isNaN(parseDate(value));
}

// parse a date in dd-MM-yyyy format
function parseDate(input) {
	var parts = input.split('-');
	return new Date(parts[2], parts[1] - 1, parts[0]); // Note: months are 0-based
}