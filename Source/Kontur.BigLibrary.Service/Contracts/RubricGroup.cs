using System.Runtime.Serialization;

namespace Kontur.BigLibrary.Service.Contracts
{
    [DataContract]
    public class RubricSummaryGroup
    {
        [DataMember]
        public RubricSummary ParentRubric { get; set; }

        [DataMember]
        public RubricSummary[] Rubrics { get; set; }
    }
}