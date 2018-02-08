// Copyright (c) Microsoft. All rights reserved.

using System;
using System.Collections.Generic;
using Microsoft.Azure.IoTSolutions.DeviceTelemetry.Services.Exceptions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Microsoft.Azure.IoTSolutions.DeviceTelemetry.Services.Models
{
    public class Rule : IComparable<Rule>
    {
        private const string DATE_FORMAT = "yyyy-MM-dd'T'HH:mm:sszzz";
        [JsonIgnore]
        public string ETag { get; set; } = string.Empty;
        [JsonIgnore]
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string DateCreated { get; set; } = DateTimeOffset.UtcNow.ToString(DATE_FORMAT);
        public string DateModified { get; set; } = DateTimeOffset.UtcNow.ToString(DATE_FORMAT);
        public bool Enabled { get; set; } = false;
        public string Description { get; set; } = string.Empty;
        public string GroupId { get; set; } = string.Empty;
        public string Severity { get; set; } = string.Empty;
        public IList<Condition> Conditions { get; set; } = new List<Condition>();

        public Rule() { }

        public static Rule CreateNew(JToken json)
        {
            Rule rule = new Rule();

            Rule rule2 = JsonConvert.DeserializeObject<Rule>(json.ToString());

            try
            {
                JToken jsonRule = json;
                JArray jsonConditions = (JArray) jsonRule["Conditions"];

                rule.Name = jsonRule["Name"].ToString();
                rule.Enabled = jsonRule["Enabled"].ToObject<bool>();
                rule.Description = jsonRule["Description"].ToString();
                rule.GroupId = jsonRule["GroupId"].ToString();
                rule.Severity = jsonRule["Severity"].ToString();

                List<Condition> conditions = new List<Condition>();
                foreach (var condition in jsonConditions)
                {
                    conditions.Add(new Condition(
                        condition["Field"].ToString(),
                        condition["Operator"].ToString(),
                        condition["Value"].ToString()));
                }
                rule.Conditions = conditions;
            }
            catch (Exception e)
            {
                throw new InvalidInputException("Invalid input for rule", e);
            }
            return rule;
        }

        public int CompareTo(Rule other)
        {
            if (other == null) return 1;

            return DateTimeOffset.Parse(other.DateCreated)
                .CompareTo(DateTimeOffset.Parse(this.DateCreated));
        }
    }
}
