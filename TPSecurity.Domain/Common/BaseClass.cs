namespace TPSecurity.Domain.Common;

public class BaseClass
{
    public override int GetHashCode()
    {
        int hash = 0;
        var type = this.GetType();
        var properties = type.GetProperties();

        foreach (var property in properties)
        {
            hash = HashCode.Combine(hash, property.GetValue(this));
        }

        return hash;
    }

    public string GetHashCodeAsString()
    {
       return this.GetHashCode().ToString();
    }
}
