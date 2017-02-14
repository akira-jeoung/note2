/*
with html
validation rule
C:\Git\Barium\Source\Barium.Boost\Barium.Boost.Web\Scripts\jquery.validation.js
*/


        //data-validation-min="1"
        var minValue = self.element.data("validationMin");
        if((minValue === undefined) === false) {
            var value = getElementVal();
            if(value === '') {
                self.clearError();
                return true;
            }

            if(!$.isNumeric(value)) {
                self.reportError(i18n._TheValueIsNotANumber);
                return false;
            }
            
            var number = parseFloat(value);
            if(number < minValue) {
                self.reportError(i18n._TheValueCannotBeLessThan2+" " + minValue);
                return false;
            }
        }

        //data-validation-max="100"
        var maxValue = self.element.data("validationMax");
        if((maxValue === undefined) === false) {
            var value = getElementVal();
            if(value === '') {
                self.clearError();
                return true;
            }

            if(!$.isNumeric(value)) {
                self.reportError(i18n._TheValueIsNotANumber);
                return false;
            }
            
            var number = parseFloat(value);
            if(number > maxValue) {
                self.reportError(i18n._TheValueCannotBeGreaterThan2+" " + maxValue);
                return false;
            }
        }


        //data-validation-max="100"
        var NumericValue = self.element.data("validationNumeric");
        if((NumericValue === undefined) === false) {
            var value = getElementVal();
            if(value === '') {
                self.clearError();
                return true;
            }
            if(!$.isNumeric(value)) {
                self.reportError(i18n._TheValueIsNotANumber);
                return false;
            }
        }

        //data-validation-length-min="5"
        var lengthValueMin = self.element.data("validationLengthMin");
        if((lengthValueMin === undefined) === false) {
            var value = getElementVal();
            if(value === '') {
                self.clearError();
                return true;
            }
            
            if(value.length < lengthValueMin) {
                self.reportError(i18n._TheValueMustBeLongerThan2 + " " + lengthValueMin + " " + i18n._Characters);
                return false;
            }
        }
		
		
/*
with html
validation rule
make legend button and modal
C:\Git\Barium\Source\Barium.Boost\Barium.Boost.Web\Areas\COMM\Views\Meeting\List.cshtml
*/
        self.LoadLegendModal = function () {
            $('#resisterdEvent-legend-modal').modal('toggle');
        }
	
/*
add "Permissions List" to the end of each
C:\Git\Barium\Source\Barium.Boost\Barium.Boost.Web\Areas\WFM\Views\Positions\Display.cshtml	
C:\Git\Barium\Source\Barium.Boost\Barium.Boost.Web\Areas\WFM\Views\PositionsTemplate\Display.cshtml	
*/
    // Standard Roles
	self.StandardRoles = ko.observableArray(data.StandardRoles);
	$.each(self.StandardRoles(), function (i, item) {
		item.Key += @Html.Raw(BariumTranslate.Language._PermissionsListing);
	}); 
	// will make error because
	// @Html.Raw(BariumTranslate.Language._PermissionsListing) =  Permissions List

	// Standard Roles
	self.StandardRoles = ko.observableArray(data.StandardRoles);
	$.each(self.StandardRoles(), function (i, item) {
		item.Key += "@Html.Raw(BariumTranslate.Language._PermissionsListing)";
	});

	
/* 7123
with html
Improve message on WFS: input the filter name to message
C:\Git\Barium\Source\Barium.Boost\Barium.Boost.Web\Areas\Scheduling\Views\ShiftChangeHistory\List.cshtml
*/
            // variable for the filter name for roles
            self.GetNameBySelectedFilter = ko.computed(function () {
                if (self.AvailableFilters()[self.SelectedFilter()])
                    return self.AvailableFilters()[self.SelectedFilter()].Name + "s";
                return; // nothing to do
            }, self);



/*
Improve message on WFS: input the filter name to message
C:\Git\Barium\Source\Barium.Boost\Barium.Boost.Web\Areas\COMM\Views\LetterTemplate\List.cshtml
*/		

		if (dataList.Items.length == 0) {
			self.Messages.push("@Html.SanitizeJSString(BariumTranslate.Language._TheListIsEmptyThereAreNoItemsT2)");
		}	

		if (dataList.Items[0].ScheduledLetters.length == 0) {
			self.Messages.push("@Html.SanitizeJSString(BariumTranslate.Language._TheListIsEmptyThereAreNoItemsT2)");
		}	
			
			
			
			
/*
bug: before execution, removing selected
C:\Git\Barium\Source\Barium.Boost\Barium.Boost.Web\Areas\Admin\Views\Profiles\EmployeeActivityLogList.cshtml
In same scope, JavaScript executes the fallowing lines when it is waiting for dialog box for user to response
*/				
		// Bulk Action
		self.AvailableActions = ko.observableArray();
		self.SelectedAction = ko.observable();
		self.ActionExporttoExcel = "@Html.SanitizeJSString(BariumTranslate.Language._ExportToExcel)";	
			
		self.SelectedAction.subscribe(function (action) {
                if (action == undefined || action == null) {
                    return; //Do Nothing...
                }
				
				// Setup
				self.ClearMessages();
				self.ClearErrors();

				// get selected items
				var selected = [];
				ko.utils.arrayForEach(self.Items(), function (item) {
					if (item.IsChecked()) { selected.push(item.Id()); }
				});
				if (selected.length <= 0) {
					self.Errors.push("Please select items.");
					self.SelectedAction(null);
					return;
				}

                var msg = "Are you sure you wish to invoke this action on the selected items?";
                bootbox.dialog({
                    message: msg,
                    buttons: {
                        main: {
                            label: "Ok",
                            className: "btn-primary",
                            callback: function (result) {
                                if (result) {                                    
                                    self.Loading(true);
                                    if (action == self.ActionExporttoExcel) {

                                        $.ajax({
                                            url: '/Admin/Profiles/ExportEmployeeActivityLog',
                                            data: JSON.stringify({
                                                selection: selected
                                            }),
                                            contentType: 'application/json',
                                            type: "POST",
                                            cache: false
                                        })
                                        .fail(function (qxhr, status, errorThrown) {
                                            if (errorThrown) { self.Errors.push(errorThrown); }
                                        })
                                        .done(function (data) {
                                            self.Loading(false);
                                            window.open("/WFM/Documents/GetFile?file=" + url + 'Download?cacheKey=' + data, '_blank');
                                            window.focus();
                                        });

                                    } else {
                                        self.Errors.push("Unhandled action selected");
                                    }

                                    self.SelectedAction(null);
                                    self.SelectAll(false);
                                }
                            }//end callback
                        },//end main button
                        cancel: {
                            label: "Cancel",
                            className: "btn-inverse"
                        }
                    }//end buttons
                });//end dialog
                
				// these lines will be executed before dialog box
				self.SelectedAction(null);
                self.SelectAll(false);				
				
            });
			
			
			
/*
sort dropdown list
C:\Git\Barium\Source\Barium.Boost\Barium.Boost.Web\Areas\WFM\Views\EmployeeDirectory\MyEmployees.cshtml
WFM/EmployeeDirectory/MyEmployees
*/				
			
			
			self.AvailableFilters.sort(function (left, right) {
				return left.Name == right.Name ? 0 : (left.Name < right.Name ? -1 : 1)
			});
			
		/*	
			<div class="input-group-btn">
				<button type="button" class="btn btn-primary dropdown-toggle" data-toggle="dropdown" aria-expanded="false"><span><!-- ko text: DepartmentName--><!--/ko-->&nbsp;</span><span class="caret"></span></button>
				<ul class="dropdown-menu" role="menu" data-bind="foreach: AvailableFilters">
					<li><a href="#" data-bind="text: Name, click: function(data, event){$parent.SelectedFilter(data); $parent.DepartmentName(data.Name);}"></a></li>
				</ul>
			</div>
		*/	
			
			
/* 7287
validation for at least one checked 
C:\Git\Barium\Source\Barium.Boost\Barium.Boost.Web\Areas\WFM\Views\Positions\ Create  Edit
C:\Git\Barium\Source\Barium.Boost\Barium.Boost.Web\Areas\WFM\Views\PositionsTemplate\ Create  Edit
   /WFM/PositionsTemplate/Create
*/				
		var countSelecetedRoles = 0;
		ko.utils.arrayForEach(self.Item().StandardRoles(), function (item) {
			ko.utils.arrayForEach(item.Roles, function (item) {
				if (item.IsSelected) { countSelecetedRoles++; }
			});
		});
		ko.utils.arrayForEach(self.Item().CustomRoles(), function (item) {
			if (item.IsSelected) { countSelecetedRoles++; }
		});

		if (countSelecetedRoles == 0) {
			self.Errors.push("There are Validation error in the Roles. Please choose more than one role from either the Standard Roles, or Custom Roles and try again.");
			return;
		}

		
/* 7298 change success message after creating custom rule
		/Admin/Role/Create
		C:\Git\Barium\Source\Barium.Boost\Barium.Boost.Web\Areas\Admin\Views\Role\Create.cshtml
*/				
	
		$.ajax({
			url: '@Url.Action("Edit", "Role")',
			contentType: 'application/json',
			type: "POST",
			cache: false,
			data: viewData
		})
		.fail(function (qxhr, status, errorThrown) {
			if (errorThrown) { self.Errors.push(errorThrown); }
		})
		.done(function (data) {
			self.Messages.push("The record has been saved.");
			setTimeout(function () { window.location = "/Admin/Role/List?selectedTab=2" }, 5000);
		})
		.complete(function () {
			$("#save-button").prop("disabled", false);
		});

		
		
/* 7312 make link alive after getting error message from server
		WFM/TM/Application/List
		C:\Git\Barium\Source\Barium.Boost\Barium.Boost.Web\Areas\WFMTM\Views\Application\List.cshtml
*/		

	self.CanEditEntity = ko.computed(function () {
            return $.inArray(Barium.Permisions.TMApplicationsEdit, self.Permissions()) >= 0 ; // && self.Errors().length == 0;
			
								<td>
                                    <span data-bind="visible: !$parent.CanEditEntity(), text: Name"></span>
                                    <a targer="_blank" href="#" data-bind="visible: $parent.CanEditEntity, click: $parent.Display, text: Name"></a>
                                </td>
	
	
	
	
	
	
	
	
	
	
	
	