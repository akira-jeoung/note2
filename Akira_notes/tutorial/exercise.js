	
// data-bind text
	// This is a simple *viewmodel* - JavaScript that defines the data and behavior of your UI
	function AppViewModel() {
		this.firstName = "Bert";
		this.lastName = "Bertington";
	}

	// Activates knockout.js
	ko.applyBindings(new AppViewModel());
	

// data-bind input value
	// This is a simple *viewmodel* - JavaScript that defines the data and behavior of your UI
	function AppViewModel() {
		this.firstName = ko.observable("Bert");
		this.lastName = ko.observable("Bertington");
	}

	// Activates knockout.js
	ko.applyBindings(new AppViewModel());
	
	
//  computed properties to conbine or convert multiple observable
	// This is a simple *viewmodel* - JavaScript that defines the data and behavior of your UI
	function AppViewModel() {
		this.firstName = ko.observable("Bert");
		this.lastName = ko.observable("Bertington");
		this.fullName = ko.computed(function() {
		return this.firstName() + " " + this.lastName();    
	}, this);
	}

	// Activates knockout.js
	ko.applyBindings(new AppViewModel());
	

// capitalize the value of property
	// This is a simple *viewmodel* - JavaScript that defines the data and behavior of your UI
	function AppViewModel() {
		this.firstName = ko.observable("");
		this.lastName = ko.observable("");
		this.fullName = ko.computed(function() {
			return this.firstName() + " " + this.lastName();
		}, this);
		
		this.capitalizeLastName = function() {
			var currentVal = this.lastName();        // Read the current value
			this.lastName(currentVal.toUpperCase()); // Write back a modified value
		};
	}

	// Activates knockout.js
	ko.applyBindings(new AppViewModel());
