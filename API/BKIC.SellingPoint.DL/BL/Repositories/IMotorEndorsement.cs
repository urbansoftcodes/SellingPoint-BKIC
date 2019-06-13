using BKIC.SellingPoint.DL.BO;

namespace BKIC.SellingPoint.DL.BL.Repositories
{
    public interface IMotorEndorsement
    {
        MotorEndorsementQuoteResult GetMotorEndorsementQuote(MotorEndorsementQuote motorEndorsement);
        MotorEndorsementResult PostMotorEndorsement(MotorEndorsement motorEndorsement);
        MotorEndorsementResult PostAdminMotorEndorsement(MotorEndorsement motorEndorsement);
        MotorEndorsementPreCheckResponse EndorsementPrecheck(MotorEndorsementPreCheckRequest request);
        MotorEndoResult GetAllEndorsements(MotorEndoRequest request);
        MotorEndorsementOperationResponse EndorsementOperation(MotorEndorsementOperation request);
    }
}