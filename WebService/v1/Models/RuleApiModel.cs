// Copyright (c) Microsoft. All rights reserved.

using System;
using System.Collections.Generic;
using Microsoft.Azure.IoTSolutions.DeviceTelemetry.Services.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Microsoft.Azure.IoTSolutions.DeviceTelemetry.WebService.v1.Models
{
    public class RuleApiModel
    {
        [JsonProperty(PropertyName = "ETag")]
        public string ETag { get; set; }

        [JsonProperty(PropertyName = "Id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "Name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "DateCreated")]
        public string DateCreated { get; set; }

        [JsonProperty(PropertyName = "DateModified")]
        public string DateModified { get; set; }

        [JsonProperty(PropertyName = "Enabled")]
        public bool Enabled { get; set; }

        [JsonProperty(PropertyName = "Description")]
        public string Description { get; set; }

        [JsonProperty(PropertyName = "GroupId")]
        public string GroupId { get; set; }

        [JsonProperty(PropertyName = "Severity")]
        public string Severity { get; set; }

        [JsonProperty(PropertyName = "Conditions")]
        public IList<ConditionApiModel> Conditions { get; set; } = new List<ConditionApiModel>();

        [JsonProperty(PropertyName = "$metadata", Order = 1000)]
        public IDictionary<string, string> Metadata => new Dictionary<string, string>
        {
            { "$type", "Rule;" + Version.NUMBER },
            { "$uri", "/" + Version.PATH + "/rules/" + this.Id },
            { "$created", this.DateCreated },
            { "$modified", this.DateModified }
        };

        public RuleApiModel() { }

        public RuleApiModel(Rule rule)
        {
            if (rule != null)
            {
                this.ETag = rule.ETag;
                this.Id = rule.Id;
                this.Name = rule.Name;
                this.DateCreated = rule.DateCreated;
                this.DateModified = rule.DateModified;
                this.Enabled = rule.Enabled;
                this.Description = rule.Description;
                this.GroupId = rule.GroupId;
                this.Severity = rule.Severity;

                foreach (Condition condition in rule.Conditions)
                {
                    this.Conditions.Add(new ConditionApiModel(condition));
                }
            }
        }

        public Rule ToServiceModel()
        {
            List<Condition> conditions = new List<Condition>();
            foreach (ConditionApiModel condition in this.Conditions)
            {
                conditions.Add(new Condition(
                    condition.Field,
                    condition.Operator,
                    condition.Value));
            }

            return new Rule()
            {
                ETag = this.ETag,
                Id = this.Id,
                Name = this.Name,
                DateCreated = this.DateCreated,
                DateModified = this.DateModified,
                Enabled = this.Enabled,
                Description = this.Description,
                GroupId = this.GroupId,
                Severity = this.Severity,
                Conditions = conditions
            };
        }
    }
}
