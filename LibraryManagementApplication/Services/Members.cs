using LibraryManagementApplication.Exceptions;
using LibraryManagementApplication.Model;
using LibraryManagementApplication.Services.Interfaces;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementApplication.Services
{
    public class Members : IUsersService
    {
        private readonly List<Member> _users;

        public Members()
        {
            _users = GetAllUsers();
        }

        //get all users
        public List<Member> GetAllUsers()
        {

            var cfg = new Configuration();
            cfg.Configure();

            using (ISession session = cfg.BuildSessionFactory().OpenSession())

            using (ITransaction transaction = session.BeginTransaction())
            {
                ICriteria criteria = session.CreateCriteria<Member>();

                List<Member> usersDB = (List<Member>)criteria.List<Member>();

                try
                {
                    session.SaveOrUpdate(usersDB);

                    transaction.Commit(); //commit transaction
                }
                catch
                {
                    transaction.Rollback();
                }

                return usersDB;
            }
        }

        //Adding user, need to check if conflicts with existing user
        public void AddUser(Member user)
        {
            var cfg = new Configuration();
            cfg.Configure();

            using (ISession session = cfg.BuildSessionFactory().OpenSession())

            using (ITransaction transaction = session.BeginTransaction())
            {
                ICriteria criteria = session.CreateCriteria<Member>();

                criteria.Add(Restrictions.Eq("EmailAddress", user.EmailAddress)); //if userDB == user.EmailAddress

                List<Member> usersDB = criteria.List<Member>().ToList();

                try
                {
                    if(usersDB.Count == 0)
                    {
                        session.SaveOrUpdate(user);
                    }
                    else if(usersDB.Count > 0)
                    {
                        throw new UserConflictException(user);
                    }

                    transaction.Commit(); //commit transaction
                }
                catch (UserConflictException)
                {
                    transaction.Rollback();
                    throw new UserConflictException(user);
                }
            }    
        }



        //Loop through users, if user ID == given ID, and user ID is not in loan table, delete
        public void DeleteUserByID(Guid userID)
        {

            var cfg = new Configuration();

            cfg.Configure();

            using (ISession session = cfg.BuildSessionFactory().OpenSession())

            using (ITransaction transaction = session.BeginTransaction())
            {

                ICriteria criteria = session.CreateCriteria<BookLoan>();
                criteria.Add(Restrictions.Eq("UserID", userID));
                List<BookLoan> bookLoansDB = criteria.List<BookLoan>().ToList();

                Member memberDB = (Member)session.Get("Member", userID); //used this to get MemberObject using passed memberID

                try
                {
                    if (bookLoansDB.Count == 0) //if user has no loans, delete
                    {
                        session.Delete(memberDB);
                    }
                    else if (bookLoansDB.Count() > 0)
                    {
                        throw new UserEditHasLoanException();
                    }

                    transaction.Commit(); //commit transaction
                }
                catch (UserEditHasLoanException)
                {
                    transaction.Rollback();
                    throw new UserEditHasLoanException();
                }
            }
        }

        //Can edit user if email address does not exist unless it is existing users email
        public void EditUser(Member user)
        {
            var cfg = new Configuration();
            cfg.Configure();

            using (ISession session = cfg.BuildSessionFactory().OpenSession())

            using (ITransaction transaction = session.BeginTransaction())
            {

                ICriteria criteria = session.CreateCriteria<Member>();

                criteria.Add(!Restrictions.Eq("ID", user.ID)); //if user ID != user.ID, then don't let them use a duplicate email
                criteria.Add(Restrictions.Eq("EmailAddress", user.EmailAddress));

                List<Member> usersDB = criteria.List<Member>().ToList();

                try
                {

                    if (usersDB.Count == 0) //if user has no loan, edit user
                    {
                        session.SaveOrUpdate(user);
                    }
                    else if (usersDB.Count() > 0)
                    {
                        throw new UserConflictException();
                    }

                    transaction.Commit(); //commit transaction
                }
                catch (UserConflictException)
                {
                    transaction.Rollback();
                    throw new UserConflictException();
                }
            }
        }
    }
}
