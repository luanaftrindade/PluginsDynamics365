using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plugins
{
    public class PosOperation_ConvertLead : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {

            ITracingService tracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));

            IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));

            if (context.MessageName.Equals("ConvertLead") && context.InputParameters.Contains("LeadId"))
            {
                IOrganizationServiceFactory serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
                IOrganizationService service = serviceFactory.CreateOrganizationService(context.UserId);

                Entity lead = (Entity)context.InputParameters["Target"];

                if (lead.Attributes.Contains("emailaddress1"))
                {

                   
                    String emailAddress = (String)lead["emailaddress1"];

                    QueryExpression queryExpression = new QueryExpression("contact");

                    queryExpression.ColumnSet = new ColumnSet(false);

                    queryExpression.Criteria.AddCondition("emailaddress1", ConditionOperator.Equal, emailAddress);

                    EntityCollection results = service.RetrieveMultiple(queryExpression);

                    // checking if there is any contact with the same email address
                    
                    if(results.Entities.Count > 0)
                    {
                        return;
                    }


                    #region PluginLogic

                    // get the lead contact information
                    string fullName = lead.GetAttributeValue<string>("fullname");
                    string email = lead.GetAttributeValue<string>("emailaddress1");
                    string phoneNumber = lead.GetAttributeValue<string>("mobilephone");

                    // create a new contact record with the lead information 
                    Entity contact = new Entity("contact");
                    contact["fullname"] = fullName;
                    contact["emailaddress1"] = email;
                    contact["mobilephone"] = phoneNumber;

                    service.Create(contact);


                    #endregion
                }

            }
            throw new NotImplementedException();
        }
    }
}
