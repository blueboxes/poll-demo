using Azure;
using Azure.Data.Tables;
using System;

namespace Api.Models
{
    internal class VoteDataRow : ITableEntity
    {
        public string PartitionKey { get; set; } = string.Empty; //QuestionId
        public string RowKey { get; set; } //Guid of Vote
        public int OptionId { get; set; }  
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }
    }
}
