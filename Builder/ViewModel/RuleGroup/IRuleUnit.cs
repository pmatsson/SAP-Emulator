namespace MQChatter.ViewModel.RuleGroup
{
    public interface IRuleUnit
    {
        bool ValidateUnit();

        int ErrorsInConfiguration { get; set; }

        //string ErrorMessage { get; set; }
    }
}