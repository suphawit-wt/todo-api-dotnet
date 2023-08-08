using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace TodoApp.Core.Exceptions
{
    public static class ExceptionHandler
    {
        public static void Handle(Exception ex)
        {
            if (ex is DbUpdateException dbEx)
            {
                if (dbEx.InnerException is SqlException sqlEx)
                {
                    if (sqlEx.Message.Contains("IX_users_username"))
                    {
                        throw new ConflictError("This Username already taken");
                    }

                    if (sqlEx.Message.Contains("IX_users_email"))
                    {
                        throw new ConflictError("This Email already taken");
                    }

                    throw new DatabaseError("SQL operation error.");
                }

                throw new DatabaseError("Database operation error.");
            }

            throw new UnexpectedError();
        }

    }
}