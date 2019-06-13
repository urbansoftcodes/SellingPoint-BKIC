using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BKIC.SellingPoint.DL.StoredProcedures
{
    public class UsersSP
    {
        public const string PostUserMaster = "SP_InsertUserMaster";
        public const string UserNamePrecheck = "SP_PrecheckUserName";
        public const string FetchUserDetails = "SP_FetchUserID";
        public const string LoginAudit = "SP_LoginAudit";
        public const string InsertInvitaClaimDetails = "INSERTINVITACLAIMINFO";
        public const string InsertClaimentInfo = "Bulk_INVITACLAIMENTINFO";
        public const string InsertClaimHistoryInfo = "Bulk_INVITACLAIMHISTORYINFO";
        public const string InsertClaimInfo = "Bulk_INVITACLAIMINFO";
        public const string InsertClaimEstimateInfo = "Bulk_INVITAESTIMATEINFO";
        public const string InsertClaimSettlmentInfo = "Bulk_INVITASETTLEMENTINFO";
        public const string GetClaimsByUniqId = "GETClaimsByUniqId";
    }
}
