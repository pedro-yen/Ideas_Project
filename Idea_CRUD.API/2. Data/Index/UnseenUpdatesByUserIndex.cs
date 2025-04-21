using Backend.Challenge.Data.Models;
using Raven.Client.Documents.Indexes;
using System.Collections.Generic;
using System.Linq;

namespace Backend.Challenge._2._Data.Index
{
    public class UnseenUpdatesByUserIndex :  AbstractIndexCreationTask<StatusUpdate, UnseenUpdatesByUserIndex.Result>
    {
        public class Result
        {
            public string IdeaId { get; set; }
            public string StatusUpdateId { get; set; }
            public List<string> SeenByUserIds { get; set; }
        }

        //public UnseenUpdatesByUserIndex()
        //{
        //    Map = statusUpdates => from status in statusUpdates
        //                           from userId in status.SeenByUserIds
        //                           where !status.SeenByUserIds.Contains(userId)  // Filter unseen updates
        //                           select new
        //                           {
        //                               status.IdeaId,
        //                               status.Id,
        //                               status.SeenByUserIds
        //                           };
        //}
    }
}
