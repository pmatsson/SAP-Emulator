namespace Builder.Model
{
    interface IRule
    {
        string Param1 { get; set; }

        string Param2 { get; set; }

        bool ShouldSerializeParam1();

        bool ShouldSerializeParam2();
    }
}
