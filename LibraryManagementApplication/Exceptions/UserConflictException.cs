using LibraryManagementApplication.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementApplication.Exceptions
{
    class UserConflictException : Exception
    {
        public Member ExistingUser { get; }

        public Member IncomingUser { get; }

        public UserConflictException() { }

        public UserConflictException(Member incomingUser)
        {
            IncomingUser = incomingUser;
        }

        public UserConflictException(Member existingUser, Member incomingUser)
        {
            ExistingUser = existingUser;
            IncomingUser = incomingUser;
        }

        public UserConflictException(string message, Member existingUser, Member incomingUser) : base(message)
        {
            ExistingUser = existingUser;
            IncomingUser = incomingUser;
        }

        public UserConflictException(string message, Exception innerException, Member existingUser, Member incomingUser) : base(message, innerException)
        {
            ExistingUser = existingUser;
            IncomingUser = incomingUser;
        }
    }
}
