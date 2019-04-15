namespace MQChatter.ViewModel.RuleGroup
{
    public interface IRuleUnit
    {
        bool ValidateUnit();

        int NumberOfErrors { get; set; }

        string ErrorMessage { get; set; }
    }
}