using PrivilegeCoreLibrary.Model;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;

namespace PrivilegeCoreLibrary
{
    public interface IPrivilegeStore
    {
        void CreateCompany(Company company);
        bool UpdateCompany(Company company);
        Company GetCompany(string name);
        bool DeleteCompany(string id);

        void CreateCustomer(Customer customer);
        bool UpdateCustomer(Customer customer);
        Customer GetCustomer(string email);
        bool DeleteCustomer(string id);

        void CreateCompanyStaff(Company company);
        Staff GetCompanyStaff(string email);

        Promotion GetPromotion(string id);
        List<Promotion> GetPromotions(string company);
        bool UpdatePromotion(Promotion promotion);
        void CreatePromotion(Promotion promotion);
        bool DeletePromotion(string id);
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
