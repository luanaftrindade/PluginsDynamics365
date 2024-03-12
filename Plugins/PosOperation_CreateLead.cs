using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plugins
{
    public class PosOperation_CreateLead : IPlugin
    {

            public void Execute(IServiceProvider serviceProvider)
            {
                ITracingService tracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));
                IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
           
                if (context.InputParameters.Contains("Target") && context.InputParameters["target"] is Entity)
                {
                    IOrganizationServiceFactory serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
                    IOrganizationService service = serviceFactory.CreateOrganizationService(context.UserId);

                    Entity entity = (Entity)context.InputParameters["target"];

                    #region pluginLogic

                    if (entity.Attributes.Contains("emailaddress1"))
                    {
                        String email = (String)entity.Attributes["emailaddress1"];

                        QueryExpression query = new QueryExpression("contact");
                        query.ColumnSet = new ColumnSet("emailaddress1", "mobilephone", "parentcustomerid");
                        query.Criteria.AddCondition("emailaddress", ConditionOperator.Equal, email);

                        EntityCollection results = service.RetrieveMultiple(query);

                        if (results.Entities.Count > 0)
                        {

                            Entity contact = results.Entities[0];

                            entity["mobilephone"] = contact.GetAttributeValue<String>("mobilephone");
                            entity["parentaccountid"] = (EntityReference)contact["parentcustomerid"];
                        }
                    }

                    service.Update(entity);

                    #endregion
                }
            }
        }
    }
