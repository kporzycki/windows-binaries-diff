namespace PeDiff 
{ 
    public class ComparisonResult 
    { 
        public string PropertyName { get; set; } 
        public string OriginalValue { get; set; } 
        public string NewValue { get; set; } 
        public bool HasValueChanged { get; set; } 
 
        public static ComparisonResult CompareValues<T>(string propertyName, T originalValue, T newValue) 
        { 
            return new ComparisonResult 
            { 
                PropertyName = propertyName, 
                OriginalValue = originalValue.ToString(), 
                NewValue = newValue.ToString(), 
                HasValueChanged = !originalValue.Equals(newValue) 
            }; 
        } 
    } 
}