using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogic.Models.Project
{
    public class UpdateProjectModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Preview { get; set; }
        public bool UseAdvancedSecuritySettings { get; set; }
        public int AccessToChangeProject { get; set; }
        public int AccessToChangeBoard { get; set; }
        public int AccessToChangeTask { get; set; }
    }
}
