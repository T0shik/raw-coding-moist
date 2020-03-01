namespace Moist.Core.Code
{
    public class RedemptionValidationResult : ValidationResult
    {
        public string UserId { get; set; }
        public int ProgressId { get; set; }
    }
}