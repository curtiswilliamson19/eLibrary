using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementApplication.Model
{
    public class Member
    {
        public virtual Guid ID { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual string EmailAddress { get; set; }
        public virtual string PhoneNumber { get; set; }

        //could add some sort of admin type
        //e.g. public bool IsAdmin { get; set; }

        public Member() { }

        public Member(Guid iD, string firstName, string lastName, string emailAddress, string phoneNumber)
        {
            ID = iD;
            FirstName = firstName;
            LastName = lastName;
            EmailAddress = emailAddress;
            PhoneNumber = phoneNumber;
        }

        public override string ToString()
        {
            return $"{FirstName} {LastName} ({ID})"; //needed to shorten for display in list
        }

        public override bool Equals(object obj)
        {
            return obj is Member user && EmailAddress == user.EmailAddress;
        }

        //Had to because overrides equals
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static bool operator ==(Member user1, Member user2)
        {
            if (user1 is null && user2 is null)
            {
                return true;
            }
            return !(user1 is null) && user1.Equals(user2);
        }

        public static bool operator !=(Member user1, Member user2)
        {
            return !(user1 == user2);
        }
    }
}
