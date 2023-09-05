using LibraryManagementApplication.Model;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NHibernate.Driver;
using NHibernate.Tool.hbm2ddl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Mappings;
using NHibernate;

namespace LibraryManagementApplication.Database
{
    public class Connection
    {
        public void Connect()
        {
            var cfg = new Configuration();

            cfg.Configure();

            //Allows for NHibernate to auto update tables
            cfg.Properties["hbm2ddl.auto"] = "update";
        }
    }
}
