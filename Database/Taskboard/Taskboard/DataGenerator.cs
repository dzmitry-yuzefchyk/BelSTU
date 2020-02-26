using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Taskboard
{
    class DataGenerator
    {
        private int dataAmount;
        private int[] creatorIndexes = { 0, 5, 7 };
        private string[] userEmails = { "d.yuzefchik", "k.yuzefchik", "a.lenevsky", "a.yaroshevich", "a.babich", "a.pochebut", "n.chernyak", "e.lenevsky" };
        private string[] userNames = { "Dmitry", "Kate", "Antron", "Andrew", "Artyom", "Alexander", "Nikolay", "Eduard" };
        private string[] teamNames = { "super team", "dev team", "test team", "mega team", "student team", "just team", "idk", "temp team" };
        private string[] projectNames = { "Taskboard", "Test system", "Filemanager", "Website", "Hummingbird", "Ferret", "Startup", "Project" };
        private string[] boardNames = { "super board", "deb board", "test board", "finished board", "to do board", "just board", "temp board", "board" };
        private string emailDomain = "@outlook.com";
        private string password = "12345";

        private List<int> creatorsId = new List<int>();
        private List<int> teamsId = new List<int>();
        public DataGenerator(int dataAmount)
        {
            this.dataAmount = dataAmount;
        }

        public void Generate()
        {
            RegisterUsers();
            LoginUsers();
            CreateTeams();
            AddUsersToTeam();
            AddProjects();
            AddBoards();
            AddTasks();
        }

        private void RegisterUsers()
        {
            using (TaskboardEntities db = new TaskboardEntities())
            {
                for (int i = 0; i < dataAmount / 100; i++)
                {
                    try
                    {
                        string mail = $"{this.userEmails[i % 8]}{i}{emailDomain}";
                        SqlParameter email = new SqlParameter("@email", mail);
                        SqlParameter password = new SqlParameter("@password", this.password);
                        SqlParameter name = new SqlParameter("@name", this.userNames[i % 8]);
                        db.Database.ExecuteSqlCommand("[User.Register] @email, @password, @name", new[] { email, password, name });
                    }

                    catch
                    {
                        continue;
                    }
                }
            }
        }
        private void LoginUsers()
        {
            using (TaskboardEntities db = new TaskboardEntities())
            {
                for (int i = 0; i < dataAmount / 100; i++)
                {
                    try
                    {
                        SqlParameter email = new SqlParameter("@email", this.userEmails[i % 8] + i + emailDomain);
                        SqlParameter password = new SqlParameter("@password", this.password);
                        db.Database.ExecuteSqlCommand("[User.Login] @email, @password", new[] { email, password });
                    }

                    catch
                    {
                        continue;
                    }
                }
            }
        }
        private void CreateTeams()
        {
            using (TaskboardEntities db = new TaskboardEntities())
            {
                for (int i = 0; i < dataAmount / 100; i++)
                {
                    try
                    {
                        if (creatorIndexes.Contains(i % 8))
                        {
                            string uEmail = this.userEmails[i % 8] + i + emailDomain;
                            int uId = db.USER.FirstOrDefault(x => x.email == uEmail).id;
                            SqlParameter userId = new SqlParameter("@userId", uId);
                            SqlParameter title = new SqlParameter("@teamName", this.teamNames[i % 8]);

                            db.Database.ExecuteSqlCommand("[Team.Create] @userId, @teamName", userId, title);
                        }
                    }

                    catch
                    {

                        continue;
                    }
                }
            }
        }
        private void AddUsersToTeam()
        {
            using (TaskboardEntities db = new TaskboardEntities())
            {
                List<int> creatorsId = db.TEAM_USER.Where(x => x.role == "CREATOR").Select(u => u.id).ToList();

                for (int j = 0; j < creatorsId.Count; j++)
                {

                    for (int i = 0; i < dataAmount / 100; i++)
                    {
                        try
                        {
                            SqlParameter userId = new SqlParameter("@creatorId", creatorsId[j]);
                            SqlParameter teamId = new SqlParameter("@teamId", i);
                            SqlParameter userEmail = new SqlParameter("@userEmail", this.userEmails[i % 8] + i + emailDomain);

                            var result = db.Database.ExecuteSqlCommand("[Team.AddUserToTeam] @creatorId, @teamId, @userEmail", new[] { userId, teamId, userEmail });

                        }

                        catch
                        {
                            continue;
                        }
                    }
                }

            }
        }
        private void AddProjects()
        {
            using (TaskboardEntities db = new TaskboardEntities())
            {
                List<int> creatorsId = db.TEAM_USER.Where(x => x.role == "CREATOR").Select(u => u.id).ToList();

                for (int j = 0; j < creatorsId.Count; j++)
                {

                    for (int i = 0; i < dataAmount / 100; i++)
                    {
                        try
                        {
                            SqlParameter userId = new SqlParameter("@userId", creatorsId[j]);
                            SqlParameter teamId = new SqlParameter("@teamId", i);
                            SqlParameter title = new SqlParameter("@title", projectNames[i % 8]);
                            SqlParameter about = new SqlParameter("@about", "Simple about");

                            db.Database.ExecuteSqlCommand("[Project.Create] @userId, @teamId, @title, @about", new[] { userId, teamId, title, about });
                        }

                        catch
                        {
                            continue;
                        }
                    }
                }
            }
        }
        private void AddBoards()
        {
            using (TaskboardEntities db = new TaskboardEntities())
            {
                List<int> creatorsId = db.TEAM_USER.Where(x => x.role == "CREATOR").Select(u => u.id).ToList();

                for (int j = 0; j < creatorsId.Count; j++)
                {

                    for (int i = 0; i < dataAmount / 1000; i++)
                    {
                        try
                        {
                            SqlParameter userId = new SqlParameter("@userId", creatorsId[j]);
                            SqlParameter title = new SqlParameter("@title", boardNames[i % 8]);
                            SqlParameter projectId = new SqlParameter("@projectId", i);
                            SqlParameter teamId = new SqlParameter("@teamId", i);

                            db.Database.ExecuteSqlCommand("[Board.Create] @userId, @title, @projectId, @teamId", new[] { userId, title, projectId, teamId });
                        }

                        catch
                        {
                            continue;
                        }
                    }
                }
            }
        }
        private void AddTasks()
        { }
    }
}
