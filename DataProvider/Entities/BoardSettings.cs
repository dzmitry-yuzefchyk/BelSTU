using System;

namespace DataProvider.Entities
{
    public class BoardSettings
    {
        public Guid Id { get; set; }
        public Board Board { get; set; }
        public int AccessToDeleteTask { get; set; }
        public int AccessToChangeTask { get; set; }
        public int AccessToCreateTask { get; set; }
        public int AccessToChangeBoard { get; set; }
    }
}
