using PrivilegeCoreLibrary.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrivilegeCoreLibrary
{
    public abstract class PrivilegeContentStore
    {
        internal abstract void CreateCompany(Company company);
        internal abstract void UpdateCompany(Company company);
        internal abstract Company GetCompany(string name);
        internal abstract bool DeleteCompany(string id);

        internal abstract void CreateCustomer(Customer customer);
        internal abstract void UpdateCustomer(Customer customer);
        internal abstract Customer GetCustomer(string email);
        internal abstract bool DeleteCustomer(string id);
    }
    public sealed class PrivilegeContentCollections
    {
        public const string company = "company";
        public const string branch = "branch";
        public const string promotion = "promotion";
        public const string review = "review";
        public const string staff = "staff";
        public const string customer = "customer";
    }
}
