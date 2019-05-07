using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsertData
{
    class DataGenerator
    {
        private int dataAmount;
        private int[] creatorIndexes = { 0, 5, 7 };
        private string[] userEmails = { "dmitry.yuzefchik", "k.yuzefchik", "a.lenevsky", "a.yaroshevich", "a.babich", "a.pochebut", "n.chernyak", "e.lenevsky" };
        private string[] userNames = { "Dmitry", "Kate", "Antron", "Andrew", "Artyom", "Alexander", "Nikolay", "Eduard" };
        private string[] teamNames = { "super team", "dev team", "test team", "mega team", "student team", "just team", "idk", ""};
        private string[] projectNames = { "Taskboard", "Test system", "Filemanager", "Website", "Hummingbird", "Ferret", "Startup", "Project" };
        private string emailDomain = "@outlook.com";
        private string password = "12345";

        public DataGenerator(int dataAmount)
        {
            this.dataAmount = dataAmount;
        }

        public void Generate()
        {


            RegisterUsers();
        }

        private void RegisterUsers()
        {
            
        }
    }
}
