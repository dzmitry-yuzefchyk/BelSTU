namespace BusinessLogic.Models.Project
{
    public class CreateProjectModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int AccessToDeleteBoard { get; set; }
        public int AccessToChangeProject { get; set; }
        public int AccessToCreateBoard { get; set; }
    }
}
