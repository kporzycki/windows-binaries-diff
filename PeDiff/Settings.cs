namespace PeDiff
{
    public class Settings
    {
        public bool CompareClasses { get; set; }
        public bool CompareInterfaces { get; set; }
        public bool CompareValueTypes { get; set; }
        public bool CompareEnums { get; set; }
        public bool ReverseFilesOrder { get; set; }
        public string NoEntryPointError { get; set; }
    }
}
