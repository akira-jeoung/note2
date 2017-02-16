/* 6728
variating duplicate username and throw exception to show 'oh snap message'
C:\Git\Barium\Source\Barium\Barium.Library.Core\Modules\Workforce\Employees\Provisioners.cs
 Barium\Barium.Library\Modules\Workforce\Employees\Provisioners.cs
*/

namespace Barium.Library.Modules.Workforce.Employees
{
    using AccrualRule = Barium.Library.Modules.Workforce.Attendence.AccrualRule;
    using Employees = Barium.Library.Modules.Workforce.Employees;
    using Accrual = Barium.Library.Modules.Workforce.Attendence.EmployeeAccrual;
    using Barium.Library.Core.Resources.Configuration;
    using Barium.Library.Modules.Workforce.Attendence.Utilities;

    #region Lists

    public class EmployeeEntityProvisioner : EntityProvisioner<EmployeeEntityProvisioner>
    {
        public EmployeeEntityProvisioner()
        {
            // Adding            
            this.AddRule(SPEventReceiverType.ItemAdding, ValidationgUserName);
			
			

		private void ValidationgUserName()
        {
            using (var context = new Barium.Library.Models.BariumModelsDataContext(this.Provisioner.Web.Url))
            using (var employeeRepository = this.Provisioner.ServiceLocator.TryResolve<Employees.IEmployeeRepository, IInitializeDCRepository>(r => r.Initialize(context)))

            {
                var username = EmployeeFieldUsername.Instance.GetValueFromEventProperties(this.Properties, SPGENItemEventPropertiesType.AfterProperties);    
                var employee = employeeRepository.GetFirstByFilter( x  => x.Username == username);

                if (employee != null)
                    throw new Exception(BariumTranslate.Language._UsernameAlreadyExistsPleaseTry);
            }
        }
		
		
/*
update'oh snap message'
to find the code throwing exception
1. go to controller
	C:\Git\Barium\Source\Barium.Boost\Barium.Boost.Web\Areas\WFM\Controllers\Data\PositionsTemplateController.cs
	find GetItems and go to definition

2. C:\Git\Barium\Source\Barium.Boost\Barium.Boost.Services\ISAPI\WFM\PositionsTemplateService.cs
	find GetItems and check _serviceUrl : _vti_bin/Barium/WFM/PositionsTemplateService.svc
	pick the file name without extention, PositionsTemplateService
	search the file in Boost.
3. C:\Git\Barium\Source\Barium\Barium.Web.Core\Service\WFM\PositionsTemplateService.cs
	find throw exception, or GetItems
*/

		if (usersProfilePermissions.Contains(PermissionNames.EmployeePositionEdit) == false)
		{
			var usersDepartmentPermissions = this.CheckDepartmentPermissions(out employeeID, PermissionNames.EmployeePositionRead);
			if (usersDepartmentPermissions.Contains(PermissionNames.EmployeePositionRead) == false)
				throw new PermissionException(PermissionNames.EmployeePositionRead); 
		}



/*	6515
update'oh snap message'

find the code throwing exception
	1. open the corresponding html: WFM/EmployeeWizard/Wizard
	2. find the method name using network in developer tools (red coler service) -> getting method name and controller name from ajax call
	3. go to controller, find method, and go to definition using F12 -> getting corresponding method
	4. at the method, find "_serviceUrl" and read it and go to Barium solution Barium\Source\Barium\Barium.Web.Core\Service\ + Service Url
	5. find and check the method 
	6. if you can not find any association between the method and Oh-snap message, you need to check previsoner
*/

	// Barium\Barium.Livrary\Core\Modules\Workforce\Employees\Provisioners.cs
	EmployeeFieldFirstName.Instance.TrimText(this.Properties).Validate(this.Properties).EnsureValue().ContainsInvalidCharacters();
	
		// Barium\Barium.Livrary\Backbone\Utilities\FieldValidatorUtility.cs
		public FluentValidationInterface<K, F, V> ContainsInvalidCharacters()
        {
            if (!this.HasValue)
                return this;

            var mode = this.GetProperty<Barium.Library.Backbone.Utilities.CharacterWhiteListMode>(Microsoft.Practices.SharePoint.Common.Configuration.ConfigLevel.CurrentSPSite,
                Barium.Library.Core.Resources.Configuration.Global.CharacterWhiteListMode);

            var lhs = this.Field.GetValueFromEventProperties(this.Properties, SPGENItemEventPropertiesType.AfterProperties);
            if (lhs != null)
            {
                lhs.ToString().ContainNonWhiteListedCharacters(mode, this.Field.InstanceDefinition.DisplayName);
            }

            return this;
		}
		
		
			// Barium\Barium.Livrary\Backbone\Utilities\CharacterWhiteListedHelper.cs
			public static class CharacterWhiteListedHelper
			{
				private const string _ALPHANUMERIC_REGEX = "^[a-zA-z0-9 ]*$";

				/// <summary>
				/// Checks to see if the string contains non whitelisted characters
				/// </summary>
				/// <param name="text"></param>
				/// <param name="mode"></param>
				/// <param name="field"></param>
				public static void ContainNonWhiteListedCharacters(this string text, CharacterWhiteListMode mode, string field)
				{
					var regex = GetRegex(mode);

					bool isValid = true;
					if (!string.IsNullOrEmpty(regex))
						isValid = Regex.IsMatch(text, regex);

					if(!isValid)
					{
						var message = GetExceptionMessage(mode, field);
						throw new Barium.Library.Backbone.Exceptions.HumanizedException(message);
					}
				}
								
				
				 private static string GetExceptionMessage(CharacterWhiteListMode mode, string field)
				{
					string format = string.Empty;

					switch (mode)
					{
						case CharacterWhiteListMode.AlphaNumeric:
							format = BariumTranslate.Language.__0ContainsInvlaidCharacterOnly;
							break;
						case CharacterWhiteListMode.Normal:
						default:
							format = BariumTranslate.Language.__0ContainsInvlaidCharacter;
							break;
					}

					return string.Format(format, field);
				}
			}
	


/*	7311
add validating rule for start date, internal expiry date, and end date into the adding and updating rules in posting Provisioners.cs
C:\Git\Barium\Source\Barium\Barium.Library.Core\Modules\Workforce\TalentManager\Postings\Postings\Provisioners.cs
*/

		var startTime = TMPostingFieldStartDate.Instance.GetValue(this.Properties);
		var endTime = TMPostingFieldEndDate.Instance.GetValue(this.Properties);
		var internalExpiryDate = TMPostingFieldInExpiryDate.Instance.GetValue(this.Properties);
		var isInternal = TMPostingFieldIsInternal.Instance.GetValue(this.Properties);
		if (isInternal == true)
		{
			if (internalExpiryDate <= startTime)
				throw new GreaterThanFieldException(TMPostingFieldInExpiryDate.Definition.DisplayName, TMPostingFieldStartDate.Definition.DisplayName, false);
			if (endTime <= internalExpiryDate)
				throw new GreaterThanFieldException(TMPostingFieldEndDate.Definition.DisplayName, TMPostingFieldInExpiryDate.Definition.DisplayName, false);
		}
		else if (endTime <= startTime)
			throw new GreaterThanFieldException(TMPostingFieldEndDate.Definition.DisplayName, TMPostingFieldStartDate.Definition.DisplayName, false);


	// Genesis.cs

			[SPGENField(ID = "{06222351-1567-426A-971E-3362D203A3D9}",
			Type = SPFieldType.DateTime,
			DateFormat = SPDateTimeFieldFormatType.DateOnly,
			DisplayName = "Internal Expiry Date",
			Group = "Barium - Postings")]
		public class TMPostingFieldInExpiryDate : SPGENField<TMPostingFieldInExpiryDate, SPFieldDateTime, DateTime?>
		{
		}
				
	// C:\Git\Barium\Source\Barium\Barium.Library.Core\Backbone\Utilities\FieldValidatorUtility.cs

			public static V GetValue<K, F, V>(this SPGENField<K, F, V> field, SPItemEventProperties properties)
				where K : SPGENField<K, F, V>, new()
				where F : SPField
			{
				try 
				{
					if (properties.AfterProperties[field.InstanceDefinition.InternalName] != null)
						return field.GetValueFromEventProperties(properties, SPGENItemEventPropertiesType.AfterProperties);
					else
						return field.GetItemValue(properties.ListItem);
				}
				catch (Exception)
				{
					return default(V);
				}
			}
				
	// C:\Git\Barium\Source\Barium\Barium.Library.Core\Backbone\Exception\GreaterThanFieldException.cs	

		public class GreaterThanFieldException : HumanizedException
		{
			public GreaterThanFieldException(string field, string rightside, bool orEqualTo)
				: base(string.Format(BariumTranslate.Language.__0MustBeGreaterThan21, field, rightside, orEqualTo ? BariumTranslate.Language._OrEqualTo : string.Empty))
			{}

			public GreaterThanFieldException(string field, string rightside, bool orEqualTo, Exception ex)
				: base(string.Format(BariumTranslate.Language.__0MustBeGreaterThan21, field, rightside, orEqualTo ? BariumTranslate.Language._OrEqualTo : string.Empty), ex)
			{}
		}
		
		
		
		// C:\Git\Barium\Source\Barium\Barium.Library.Core\Modules\Workforce\TalentManager\Interview\Schedule\Provisioners.cs	
			var startTime = TMISFieldStartTime.Instance.GetValue(this.Properties);
            var endTime = TMISFieldEndTime.Instance.GetValue(this.Properties);
            if (endTime < startTime)
                throw new GreaterThanFieldException(TMISFieldEndTime.Definition.DisplayName, TMISFieldStartTime.Definition.DisplayName, true);
		
		
		// C:\Git\Barium\Source\Barium\Barium.Library.Core\Modules\Communication\Meeting\Provisioners.cs	
			MeetingsFieldEndDate.Instance.Validate(this.Properties).IsRequired().IsGreaterThan<MeetingsFieldStartDate>(MeetingsFieldStartDate.Instance);		
		
			bool hasRegistrationPeriod = MeetingsFieldHasRegTime.Instance.GetValueFromEventProperties(this.Properties, SPGENItemEventPropertiesType.AfterProperties).GetValueOrDefault();
			if (hasRegistrationPeriod)
			{
				MeetingsFieldRegStartDate.Instance.Validate(this.Properties)
					.IsRequired()
					.IsLessThan<MeetingsFieldStartDate>(MeetingsFieldStartDate.Instance);
				MeetingsFieldRegEndDate.Instance.Validate(this.Properties)
					.IsRequired()
					.IsGreaterThan<MeetingsFieldRegStartDate>(MeetingsFieldRegStartDate.Instance)
					.IsLessThan<MeetingsFieldStartDate>(MeetingsFieldStartDate.Instance);
			}
			
			
			
			
/*	7311
add a new permission WFS Election Punch Clock using Migration	
		Barium\Barium.Library\Migration\_20170215122210_WFSElectionPunchClock.cs
*/			

   [MigrationVersion(Version = "https://redmine.akirawavelength.com/issues/7323")] // ticket #
    public class _20170215122210_WFSElectionPunchClock : MigrationGenesisProvisioner<thisprovisioner>, IMigration
    {
        protected override void Upgrade()
        {
            // new permissions 
            this.AddRule(new AddPermissionsWFSElectionPunchClock(this));
        }
    }

    public class AddPermissionsWFSElectionPunchClock : AddPermissionBaseRule<thisprovisioner> // base function 
    {
        public AddPermissionsWFSElectionPunchClock(thisprovisioner p)
            : base(p, Core.Resources.Core.Modules.Core, Core.Resources.Core.Components.Locations)
        {
            this.Permissions.Add(new PermissionMetaData(PermissionNames.WFSElectionPunchClock, PermissionProfileType.Profile, "Scheduling", "Allows user to access the Election-Day Unassigned Punch Clock."));
			// name, type, group, description
			// Barium\Barium.Library\Modules\Scheduling\Setup\Rules\PermissionsRule.cs
        }
    }

