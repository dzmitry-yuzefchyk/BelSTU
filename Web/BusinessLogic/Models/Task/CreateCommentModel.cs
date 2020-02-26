using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace BusinessLogic.Models.Task
{
    public class CreateCommentModel
    {
        public int TaskId { get; set; }
        public int ProjectId { get; set; }
        public string Content { get; set; }
        public List<IFormFile> Attachments {get;set;}
    }
}
